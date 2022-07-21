using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaBlaCar.BL.Models;

namespace BlaBlaCar.BL.Interfaces
{
    public interface IUserTripsService
    {
        Task<UserTripModel> GetUserTripAsync(int id);
        Task<IEnumerable<UserTripModel>> GetUserTripsAsync();
        Task<bool> AddUserTripAsync(UserTripModel tripModel);
        Task<bool> UpdateUserTripAsync(UserTripModel tripModel);
        Task<bool> DeleteUserTripAsync(int id);
    }
}
