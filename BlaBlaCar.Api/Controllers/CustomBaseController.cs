using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlaBlaCar.BL.DTOs.UserDTOs;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace BlaBlaCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomBaseController : ControllerBase
    {
        protected Guid UserId => !User.Identity.IsAuthenticated
            ? Guid.Empty
            : Guid.Parse(User.FindFirst(x=>x.Type == JwtClaimTypes.Id).Value);
        protected string UserName => !User.Identity.IsAuthenticated
            ? string.Empty 
            : User.FindFirst(x => x.Type == ClaimTypes.GivenName).Value;

        protected UserDTO GetUserInformation()
        {
            var user = new UserDTO()
            {
                Id = UserId,
                Email = UserName,
                FirstName = User.FindFirst(x => x.Type == ClaimTypes.GivenName).Value,
                PhoneNumber = User.FindFirst(x => x.Type == JwtClaimTypes.PhoneNumber).Value,
                UserStatus = UserStatusDTO.None
            };
            return user;
        }
    }
}
