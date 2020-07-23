using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevProject.Models
{
    public class UploadDoc
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Full Name", Prompt = "Enter Full Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email", Prompt = "Enter Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string TransNumber { get; set; }

        public virtual ICollection<UploadImage> UploadImages { get; set; }
    }
}
