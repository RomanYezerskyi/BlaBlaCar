using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.UserDTOs;

namespace BlaBlaCar.BL.DTOs.AdminDTOs
{
    public class UserRequestsDTO
    {
        public IEnumerable<UserDTO> Users { get; set; }
        public int TotalRequests { get; set; }
    }
}
