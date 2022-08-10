using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlaBlaCar.DAL.Entities
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public Status UserStatus { get; set; }
        public ICollection<UserDocuments> UserDocuments { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Trip> Trips { get; set; }
    }
}
