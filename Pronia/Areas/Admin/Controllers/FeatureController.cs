using System.Drawing.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels.FeatureViewModels;
using Pronia.Contexts;
using Pronia.Exceptions;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.Utils;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;



       public FeatureController(AppDbContext context, IWebHostEnvironment webHostEnvironment,
           IFileService fileService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
     
        //INDEX
        public async Task<IActionResult> Index()
        {

            var features = await _context.Features.Where(f=> f.IsDeleted==false).ToListAsync();
            return View(features);
        }





        //CREATE
        public async Task<IActionResult> Create()

        {
            if(await _context.Features.CountAsync()==3) {
                return BadRequest();
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeatureViewModel createFeatureViewModel)
        {

            if (await _context.Features.CountAsync() == 3)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            //if (!createFeatureViewModel.Image.CheckFileType("image/"))
            //{
            //    ModelState.AddModelError("Image", "sekil deyil");
            //    return View();
            //}


            //string filename=$"{Guid.NewGuid()}-{createFeatureViewModel.Image.FileName}";

            //    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images",
            //        filename);
            //    using (FileStream fileStream = new FileStream(path, FileMode.Create))
            //    {
            //        await createFeatureViewModel.Image.CopyToAsync(fileStream);
            //    }

            string filename=string.Empty;
            try
            {
                 filename = await _fileService.CreateFileAsync(createFeatureViewModel.Image, Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images",
                               "website-images"));
          
            }
            catch (FileTypeException ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();
            }



            Feature feature = new Feature
            {
                Title = createFeatureViewModel.Title,
                Description = createFeatureViewModel.Description,
                Image = filename,


            };

            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private async Task<string> CreateFileAsync(IFormFile image, string path)
        //{
        //    string filename = $"{Guid.NewGuid()}-{image.FileName}";
        //    string resultPath = Path.Combine(path, filename);
        //    using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
        //    {
        //        await image.CopyToAsync(fileStream);
        //    }
        //    return filename;
        //}


        public async Task<IActionResult> Update(int Id)
        {
            var feature = await _context.Features.SingleOrDefaultAsync(x => x.Id == Id);
            if(feature is null)
            {
                return NotFound();
            }

            UpdateFeatureViewModel updateFeatureViewModel = new UpdateFeatureViewModel
            {
                Title = feature.Title,
                Description = feature.Description,

                Id = feature.Id
            };
            return View(updateFeatureViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int Id, UpdateFeatureViewModel updateFeatureViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
             var feature = await _context.Features.FirstOrDefaultAsync(x => x.Id == Id); 
            if(feature is null)
            {
                return NotFound();
            }
            if(updateFeatureViewModel.Image!= null)
            {
                //if (!updateFeatureViewModel.Image.CheckFileType("image/"))
                //{
                //    ModelState.AddModelError("Image", "sekil deyil");
                //    return View();
                //    }
                    string path = Path.Combine(_webHostEnvironment.WebRootPath,
          "assets", "images", "website-images", feature.Image);

                //if (System.IO.File.Exists(path))
                //{
                //    System.IO.File.Delete(path);
                //}
           _fileService.DeleteFile(path);
                //string fileName = $"{Guid.NewGuid()}-{updateFeatureViewModel.Image.FileName}";
                //string newPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", fileName);
                //using (FileStream stream = new FileStream(newPath, FileMode.Create))
                //{
                //    await updateFeatureViewModel.Image.CopyToAsync(stream);
                //}
                try
                {
                    string filename = await _fileService.CreateFileAsync(updateFeatureViewModel.Image, Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images",
                                   "website-images"));
                    feature.Image = filename;
                }
                catch(FileTypeException ex)
                {
                    ModelState.AddModelError("Image", ex.Message);
                    return View();
                }
            

               
            }

            feature.Title = updateFeatureViewModel.Title;
            feature.Description = updateFeatureViewModel.Description;
       
            feature.Id = updateFeatureViewModel.Id;
            _context.Features.Update(feature);
            _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        //DELETE
        public async  Task<IActionResult> delete (int id)
        {
            IQueryable<Feature> query =_context.Features.AsQueryable();
if(await query.CountAsync()==1)
                return BadRequest();
            var feature = await query.FirstOrDefaultAsync(f=> f.Id== id );
            if(feature == null)
            {
                return NotFound();
            }
            DeleteFeatureViewModel deleteFeatureViewModel = new DeleteFeatureViewModel()
            {
                Image = feature.Image,
                Description = feature.Description,
                Title = feature.Title,

            };
            return View(deleteFeatureViewModel);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deletefeature(int id)
        {
            var feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id );
            if (feature == null)
            {
                return NotFound();
            }
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images",
               feature.Image);
            //if (System.IO.File.Exists(path))
            //{
            //    System.IO.File.Delete(path);
            //}
            //Deleteasync(path);
            _fileService.DeleteFile(path);


            feature.IsDeleted = true;
          await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //private void Deleteasync(string path)
        //{
        //    if (System.IO.File.Exists(path))
        //    {
        //        System.IO.File.Delete(path);
        //    }

        //}



        // DETAIL
        public async Task<IActionResult> Detail(int id)
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id);
            if (feature is null)
            {
                return NotFound();
            }
            return View(feature);
        }
    }


   

}




//IgnoreQueryFilters() ile ise bu hasfilteri ignore edirik 









