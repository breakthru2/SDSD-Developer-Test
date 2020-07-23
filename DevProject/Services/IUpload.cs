using DevProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevProject.Services
{
    public interface IUpload
    {
        UploadDoc GetUpload(Guid Id);

        List<UploadDoc> GetUploadDocs();

        void AddUploadDoc(UploadDoc upload);

        void RemoveUploadDoc(Guid Id);

        void UpdateUploadDoc(UploadDoc upload);

        Task<bool> SaveChangesAsync();

        void AddUploadFile(UploadImage upload);

        List<UploadImage> GetUploadFiles(string email, string transNumber);


    }
}
