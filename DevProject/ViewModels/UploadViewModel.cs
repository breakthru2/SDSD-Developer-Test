using DevProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevProject.ViewModels
{
    public class UploadViewModel
    {
        [Required]
        [Display(Name = "Full Name", Prompt = "Enter Full Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email", Prompt = "Enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Upload Documents")]
        public List<IFormFile> FormFiles { get; set; }
    }
}
