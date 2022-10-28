using ImagesWithPasswords.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ImagesWithPasswords.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using ImagesWithPasswords.Data1;

namespace ImagesWithPasswords.Data.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ImagesWithPasswords;Integrated Security=true";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        IActionResult Upload(string password, IFormFile imageFile)
        {
            string fileName = $"{ Guid.NewGuid()}-{ imageFile.FileName}";

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fs);

            var repo = new ImageRepository(_connectionString);
            var image = new Image { FileName = fileName, Password = password, ViewCount = 1 };
            repo.AddImage(image);
            var imageLink = $"http://localhost:26638/home/ViewImage?id={image.Id}";
            var vm = new UploadViewModel
            {
                ImageLink = imageLink,
                ImagePassword = password

            };
            return View(vm);
        }

        public IActionResult ViewImage(int id)
        {
            var repo = new ImageRepository(_connectionString);
            var image = repo.GetImage(id);
            var ids = HttpContext.Session.Get<List<int>>("ids");
            if (ids == null)
            {
                ids = new List<int>();

            }

            if (ids.Contains(image.Id))
            {
                repo.IncrementImagesVIewCount(id);
            }
            var vm = new ViewImageViewModel
            {
                ImageId = id,
                PasswordCheck = !ids.Contains(image.Id),
                ImagePath = image.FileName,
                ImageViewCount = image.ViewCount,
                ErrorMessage = (string)TempData["message"]
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult ViewImage(int id, string password)
        {
            var repo = new ImageRepository(_connectionString);
            var ids = HttpContext.Session.Get<List<int>>("ids");
            if(!string.IsNullOrEmpty(password) && password == repo.GetPassword(id))
            {
                if(ids == null)
                {
                    ids = new List<int>();
                }
                ids.Add(id);
                HttpContext.Session.Set("ids", id);
            }
            else
            {
                TempData["message"] = "Invalid Password";
            }
            return Redirect($"/home/viewImage?id={id}");
        }
    }  
}
