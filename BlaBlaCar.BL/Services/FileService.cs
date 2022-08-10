﻿using System;
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
        private readonly IUnitOfWork _unitOfWork;
        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            if (filePath == null) throw new Exception("File path cannot be null");
            byte[] result;

            await using FileStream SourceStream = File.Open(filePath, FileMode.Open);
            result = new byte[SourceStream.Length];
            await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
            return result;
        }

        public async Task<List<string>> FilesDbPathListAsync(IEnumerable<IFormFile> collection)
        {
            var folderName = Path.Combine("DriverDocuments", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            List<string> files = new List<string>();
            foreach (var file in collection)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').Split(".");
                var newFileName = new string(Guid.NewGuid() + "." + fileName.Last());
                var fullPath = Path.Combine(pathToSave, newFileName);
                var dbPath = Path.Combine(folderName, newFileName);
                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                files.Add(dbPath);
            }
            return files;
        }
    }
}
