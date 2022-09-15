using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlaBlaCar.DAL.Interfaces;

namespace BlaBlaCar.BL.Services
{
    
    public class FeedBackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FeedBackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
