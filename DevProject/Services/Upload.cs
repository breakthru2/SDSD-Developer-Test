using DevProject.Data;
using DevProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevProject.Services
{
    public class Upload : IUpload
    {
        private readonly ApplicationDbContext _db;

        public Upload(ApplicationDbContext db)
        {
            _db = db;
        }
        public void AddUploadDoc(UploadDoc upload)
        {
            _db.UploadDocs.Add(upload);
        }

        public void AddUploadFile(UploadImage upload)
        {
            _db.UploadImages.Add(upload);
        }

        public UploadDoc GetUpload(Guid Id)
        {
            return _db.UploadDocs.Where(u => u.Id == Id).FirstOrDefault();
        }

        public List<UploadDoc> GetUploadDocs()
        {
            return _db.UploadDocs.ToList();
        }

        public List<UploadImage> GetUploadFiles(string email, string transNumber)
        {
            return _db.UploadImages.Include(u => u.Upload).Where(e => e.Upload.Email == email && e.Upload.TransNumber == transNumber).ToList();
        }

        public void RemoveUploadDoc(Guid Id)
        {
            _db.UploadDocs.Remove(GetUpload(Id));
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (await _db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public void UpdateUploadDoc(UploadDoc upload)
        {
            throw new NotImplementedException();
        }
    }
}
