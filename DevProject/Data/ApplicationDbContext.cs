using System;
using System.Collections.Generic;
using System.Text;
using DevProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UploadDoc> UploadDocs { get; set; }

        public DbSet<UploadImage> UploadImages { get; set; }
    }
}
