using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlaBlaCar.BL.Interfaces
{
    public interface ISaveFileService
    {
        Task<List<string>> GetFilesDbPathListAsync(IEnumerable<IFormFile> collection);
        Task<string> GetFilesDbPathListAsync(IFormFile file);
        void DeleteFileFormApi(IEnumerable<string> files);
    }
}
