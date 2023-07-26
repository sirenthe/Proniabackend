using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Areas.Admin.ViewModels.FeatureViewModels;
using Pronia.Contexts;
using Pronia.Models;
using Pronia.Utils;

namespace Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public FeatureController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
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
            if (!createFeatureViewModel.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "sekil deyil");
                return View();
            }


        string filename=$"{Guid.NewGuid()}-{createFeatureViewModel.Image.FileName}";
           
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images",
                filename);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await createFeatureViewModel.Image.CopyToAsync(fileStream);
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



   //UPDATE UPDATE
        public async Task<IActionResult> Update(int id)  {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var feature = await _context.Features.FirstOrDefaultAsync(f=> f.Id == id);
            if(feature == null)
            {
                return NotFound();
            }


            UpdateFeatureViewModel updateFeatureViewModel = new UpdateFeatureViewModel
            {
                Title = feature.Title,
                Description = feature.Description,
                Image = feature.Image,
                Id = feature.Id
            };
            return View(updateFeatureViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update (int id, UpdateFeatureViewModel updateFeatureViewModel)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    var feature = await _context.Features.FirstOrDefaultAsync(f=>f.Id== id);
        //    if(feature == null)
        //    {
        //        return NotFound();
        //    }
        //    feature.Title = updateFeatureViewModel.Title;
        //    feature.Image = updateFeatureViewModel.Image;
        //    feature.Description = updateFeatureViewModel.Description;
        //    _context.Features.Update(feature);
        //    await _context.SaveChangesAsync();


        //    return RedirectToAction(nameof(Index));
        //}



        [HttpPost]


        public async Task<IActionResult> Update(int id, UpdateFeatureViewModel updateFeatureViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id);
            if (feature == null)
            {
                return NotFound();
            }

         
            if (updateFeatureViewModel.Image != null && updateFeatureViewModel.Image.Length > 0)
            {
                // Delete the old file if it exists
                if (!string.IsNullOrEmpty(feature.Image))
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", feature.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the new file
                string filename = $"{Guid.NewGuid()}-{Path.GetFileName(updateFeatureViewModel.Image.FileName)}";
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", filename);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await updateFeatureViewModel.Image.CopyToAsync(fileStream);
                }

                feature.Image = filename;
            }

            // Update other properties
            feature.Title = updateFeatureViewModel.Title;
            feature.Description = updateFeatureViewModel.Description;

            _context.Features.Update(feature);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
            if(System.IO.File.Exists(path)) { 
                System.IO.File.Delete(path);
            }


            feature.IsDeleted = true;
          await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




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