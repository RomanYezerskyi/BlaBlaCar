using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.Services
{
    public class UpdateUserDocuments
    {
        public IEnumerable<IFormFile>? DocumentsFile { get; set; }
        public IEnumerable<string>? DeletedDocuments { get; set; }
    }
}
