using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevProject.Models;
using DevProject.Services;
using DevProject.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace DevProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUpload _upload;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(IUpload upload, ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment)
        {
            _upload = upload;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var vm = new IndexViewModel()
            {
                UploadNames = _upload.GetUploadDocs()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(IndexViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string webrootPath = _hostEnvironment.WebRootPath;
                var result = new IndexViewModel
                {
                    Files = _upload.GetUploadFiles(vm.Email, vm.TransNumber),
                    href =  webrootPath,
                    UploadNames = _upload.GetUploadDocs()
                };
                return View(result);
            }
            return View(vm);
        }

        public IActionResult AddUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUpload(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                string unique = Guid.NewGuid().ToString();

                string UniqueTransId = "Upload-" + unique;             

                var upload = new UploadDoc
                {
                    Name = model.Name,
                    Email = model.Email,
                    TransNumber = UniqueTransId
                };

                _upload.AddUploadDoc(upload);

                string webrootPath = _hostEnvironment.WebRootPath;
                var files = model.FormFiles;

                var uploads = Path.Combine(webrootPath, "images");



                for (int i = 0; i < files.Count; i++)
                {
                    string fileName = files[i].FileName;
                    //var extension = Path.GetExtension(files[i].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        files[i].CopyTo(fileStreams);
                    }
                    var uploadFile = new UploadImage
                    {
                        UploadId = upload.Id,
                        ImagePath = @"\images\" + fileName
                    };                

                _upload.AddUploadFile(uploadFile);
                }

                #region Mail

                MailMessage msg = new MailMessage  // instance Mail sender service
                {
                    From = new MailAddress("paulbreakthru@gmail.com"),  // Server Email Address
                };
                msg.To.Add(model.Email); // receiver Email

                msg.Subject = "SDSD Developer Test";
                msg.Body = "Hello " + model.Name + ", these are your attachments";  // Message Body


                foreach (var filepath in files)
                {
                    string fileName = Path.GetFileName(filepath.FileName);

                    msg.Attachments.Add(new Attachment(filepath.OpenReadStream(), fileName));
                }

                SmtpClient client = new SmtpClient
                {
                    Host = "smtp.gmail.com"
                };
                NetworkCredential credential = new NetworkCredential
                {  // Server Email credentisal
                    UserName = "paulbreakthrusmtp@gmail.com",
                    Password = "@Alpha1234"
                };
                client.Credentials = credential;
                client.EnableSsl = true;
                client.Port = 587;
                client.Send(msg);


                ViewBag.Success = $"Email has been sent successfully to {model.Email}";
                _logger.LogInformation("User created ");

                #endregion


                if (await _upload.SaveChangesAsync())
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(model);

                
            }
            return View(model);
        }


       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
