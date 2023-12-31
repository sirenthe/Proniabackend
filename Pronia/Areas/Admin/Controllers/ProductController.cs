﻿using System.Drawing;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels.FeatureViewModels;
using Pronia.Areas.Admin.ViewModels.ProductViewModel;
using Pronia.Contexts;
using Pronia.Exceptions;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.Utils;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;


        public ProductController(AppDbContext context, IFileService fileService,
            IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.AsNoTracking().ToListAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel createProductViewModel)
        {

            ViewBag.Categories = _context.Categories;
            if (!ModelState.IsValid)
            {
                return View();
            }

            //Product product = new Product
            //{
            //    //Name = createProductViewModel.Name,
            //    //Description = createProductViewModel.Description,
            //    //Price = createProductViewModel.Price,
            //    //Rating = createProductViewModel.Rating,
            //    //CategoryId = createProductViewModel.CategoryId,


            //};
            var product = _mapper.Map<Product>(createProductViewModel);
            try
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images");
                product.Image = await _fileService.CreateFileAsync(createProductViewModel.Image, path);
            }
            catch (FileTypeException ex)
            {
                ModelState.AddModelError("Image", ex.Message);

                return View();
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int Id)
        {

            Product? product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == Id);
            if (product == null)
                return NotFound();
            ProductDetailViewModel productDetailViewModel = new ProductDetailViewModel
            {
                Name = product.Name,
                Price = product.Price,
                Rating = product.Rating,
                Description = product.Description,
                CategoryName = product.Category.Name,
                CreatedBy = product.CreatedBy,
                UpdatedBy = product.UpdatedBy,
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate,


            };

            return View(productDetailViewModel);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            var updateProductViewModel = _mapper.Map<UpdateProductViewModel>(product);
            return View(updateProductViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int id, UpdateProductViewModel updateProductViewModel)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();
            string filename = product.Image;
            if (updateProductViewModel.Image is not null)
            {
                try
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets",
                        "images", "website-images");
                    _fileService.DeleteFile(Path.Combine(path, product.Image));
                    filename = await _fileService.CreateFileAsync(updateProductViewModel.Image, path);
                }
                catch (FileTypeException ex)
                {
                    ModelState.AddModelError("Image", ex.Message);
                    return View();
                }

            }
            _mapper.Map(updateProductViewModel, product);
            product.Image = filename;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}