using BusinessExam.Areas.Manage.ViewModels;
using BusinessExam.DAL;
using BusinessExam.Helper;
using BusinessExam.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

namespace BusinessExam.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IValidator<CreateBlogVM> _validator;
        private IValidator<UpdateBlogVM> _validatorUpdate;


        public BlogController(AppDbContext context, IWebHostEnvironment env , IValidator<CreateBlogVM> validator , IValidator<UpdateBlogVM> validatorUpdate)
        {
            _context = context;
            _env = env;
            _validator = validator;
            _validatorUpdate = validatorUpdate;

        }
        public async  Task<IActionResult> Index()
        {

            ReadBlogVM readBlogVM = new ReadBlogVM()
            {
                GetBlogs = _context.blogs.ToList()
            };


            return View(readBlogVM);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM createBlogVM)
        {
            ValidationResult result = await _validator.ValidateAsync(createBlogVM);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View("Create", createBlogVM);
            }

            if (!createBlogVM.Image.CheckContent("image/"))
            {
                ModelState.AddModelError("Image", "Duzgun format daxil edin");
                return View();

            }
            Blog blog = new Blog()
            {
                Title = createBlogVM.Title,
                Description = createBlogVM.Description,
                ImageUrl = createBlogVM.Image.UploadFile(envPath: _env.WebRootPath, "/Upload/Blog/"),
                CreatedAt = DateTime.Now
                
            };
            await _context.blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            TempData["error"] = "";
            if (id <= 0)
            {
                TempData["error"] = "Problem bas verdi";
                return RedirectToAction(nameof(Index));
            }
            Blog blog =await _context.blogs.Where(b=>b.Id==id).FirstOrDefaultAsync();
            if(blog is null)
            {
                TempData["error"] = "Problem bas verdi";
                return RedirectToAction(nameof(Index));
            }
            UpdateBlogVM updateBlogVM = new UpdateBlogVM()
            {
                Id = id,
                Title =blog.Title,
                Description=blog.Description,
                ImageUrl = blog.ImageUrl,
                CreatedAt =blog.CreatedAt,

            };

            return View(updateBlogVM);

        }

        [HttpPost]

        public async Task<IActionResult> Update(UpdateBlogVM updateBlogVM)
        {
            ValidationResult result = await _validatorUpdate.ValidateAsync(updateBlogVM);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View("Update", updateBlogVM);
            }
            if (!updateBlogVM.Image.CheckContent("image/"))
            {
                ModelState.AddModelError("Image", "Duzgun format daxil edin");
                return View();
            }
            Blog blog = await _context.blogs.FindAsync(updateBlogVM.Id);
            TempData["error"] = "";
            if(blog is null)
            {
                TempData["error"] = "Problem bas verdi";
                return RedirectToAction(nameof(Index));

            }
            blog.Title = updateBlogVM.Title;
            blog.Description = updateBlogVM.Description;
            blog.CreatedAt = updateBlogVM.CreatedAt;
            blog.ImageUrl = updateBlogVM.Image.UploadFile(_env.WebRootPath , "/Upload/Blog/");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Delete (int id)
        {
            Blog blog = await _context.blogs.FirstOrDefaultAsync(b => b.Id == id);
            TempData["error"] = "";

            if (blog is null)
            {
                TempData["error"] = "Problem Bas verdi";
                return RedirectToAction(nameof(Index));


            }
            if (blog.ImageUrl == null)
            {
                TempData["error"] = "Problem Bas verdi";
                return RedirectToAction(nameof(Index));
            }
            blog.ImageUrl.RemoveFile(_env.WebRootPath, @"\Upload\Blog\");
            _context.blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }


    }
}
