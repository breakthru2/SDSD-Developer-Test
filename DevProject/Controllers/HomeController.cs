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
            return View();
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
                DateTime now = DateTime.Now;
                string dd = now.ToString("dd");
                string mm = now.ToString("MM");
                string yy = now.ToString("yyyy");

                string UniqueTransId = "Upload-" + dd + mm + yy;             

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

                MailMessage newMail = new MailMessage();
                newMail.From = new MailAddress("paulbreakthrough@gmail.com");
                newMail.To.Add(new MailAddress(model.Email));

                newMail.Subject = "SDSD Developer Test";
                newMail.Body = "Hello " + model.Name + ", these are your attachments";

                foreach (var filepath in files)
                {
                    string fileName = filepath.FileName;
                    //string fileNames = fileName;
                    newMail.Attachments.Add(new Attachment(filepath.OpenReadStream(), fileName));
                }
                SendEmail(newMail);
                

                if (await _upload.SaveChangesAsync())
                {
                    return View(nameof(Index));
                }

                return View(model);

                
            }
            return View(model);
        }


        public static void SendEmail(MailMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";     // These are the host connection properties  
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            // here you need to declare the credentials of the sender 
            client.Credentials = new System.Net.NetworkCredential("aptechweb2019@gmail.com", "aptech2019@");   
             
            try
            {
                client.Send(message);      // And finally this is the line which executes our process and sends mail             
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
