using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IFileService
    {
        Task<List<string>> GetFilesDbPathAsync(IEnumerable<IFormFile> collection);
        Task<string> GetFileDbPathAsync(IFormFile file);
        void DeleteFileFormApi(IEnumerable<string> files);
    }
}
