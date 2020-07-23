using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevProject.Models
{
    public class UploadImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }


        public Guid UploadId { get; set; }

        public UploadDoc Upload { get; set; }
    }
}
