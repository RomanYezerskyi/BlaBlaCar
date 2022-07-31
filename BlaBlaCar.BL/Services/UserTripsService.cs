using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.DAL.Interfaces;

namespace BlaBlaCar.BL.Services
{
    //public class UserTripsService: IUserTripsService
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public UserTripsService(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }
    //    public async Task<UserTripModel> GetUserTripAsync(int id)
    //    {
    //        var trip = _mapper.Map<UserTrip, UserTripModel>
    //            (await _unitOfWork.UserTrips.GetAsync(null, x => x.Id == id));
    //        return trip;
    //    }

    //    public async Task<IEnumerable<UserTripModel>> GetUserTripsAsync()
    //    {
    //        var trip = _mapper.Map<IEnumerable<UserTrip>, IEnumerable<UserTripModel>>
    //            (await _unitOfWork.UserTrips.GetAsync(null, null, null));
    //        return trip;
    //    }

    //    public async Task<bool> AddUserTripAsync(UserTripModel tripModel)
    //    {
    //        if (tripModel != null)
    //        {
    //            var trip = _mapper.Map<UserTripModel, UserTrip>(tripModel);
    //            await _unitOfWork.UserTrips.InsertAsync(trip);
    //            return await _unitOfWork.SaveAsync();

    //        }
    //        return false;
    //    }

    //    public async Task<bool> UpdateUserTripAsync(UserTripModel tripModel)
    //    {
    //        if (tripModel != null)
    //        {
    //            var trip = _mapper.Map<UserTripModel, UserTrip>(tripModel);
    //            _unitOfWork.UserTrips.Update(trip);
    //            return await _unitOfWork.SaveAsync();

    //        }
    //        return false;
    //    }

    //    public async Task<bool> DeleteUserTripAsync(int id)
    //    {
    //        var trip = await _unitOfWork.UserTrips.GetAsync(includes: null, filter: x => x.Id == id);
    //        if (trip == null) throw new Exception("No information about the trip!");
    //        _unitOfWork.UserTrips.Delete(trip);
    //        return await _unitOfWork.SaveAsync();
    //    }
    //}
}
