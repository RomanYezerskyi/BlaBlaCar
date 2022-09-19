using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BlaBlaCar.BL.Services
{
    public class FileService: IFileService
    {
        public async Task<List<string>> GetFilesDbPathAsync(IEnumerable<IFormFile> collection)
        {
            List<string> files = new List<string>();
            foreach (var file in collection)
            {
                files.Add(await SaveFileToApi(file));
            }
            return files;
        }
        public async Task<string> GetFileDbPathAsync(IFormFile file)
        {
            return await SaveFileToApi(file);
        }

        private async Task<string> SaveFileToApi(IFormFile file)
        {
            var folderName = Path.Combine("DriverDocuments", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').Split(".");
            var newFileName = new string(Guid.NewGuid() + "." + fileName.Last());
            var fullPath = Path.Combine(pathToSave, newFileName);
            var dbPath = Path.Combine(folderName, newFileName);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return dbPath;
        }

        public void DeleteFileFormApi(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}
