using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlaBlaCar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly AuthorizationService _authService;
        //private readonly HttpContext _httpContext;

        //public UserController(AuthorizationService authService, IHttpContextAccessor httpContextAccessor)
        //{
        //    _authService = authService;
        //    _httpContext = httpContextAccessor.HttpContext;
        //}

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] UserRegisterModel user)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest("No data was provided");
        //    }

        //    try
        //    {
        //        await _authService.CreateRoleAsync("USER");
        //        var result = await _authService.RegisterAsync(user);
        //        if (result)
        //            return new JsonResult("You are registered");
        //        return BadRequest("Check your data");
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[AllowAnonymous]
        //[HttpPut("login")]
        //public async Task<IActionResult> LogIn([FromBody] UserLoginModel user)
        //{
        //    try
        //    {
        //        var response = await _authService.LogInAsync(user);
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpPost("logout")]
        //public async Task<IActionResult> LogOut()
        //{
        //    try
        //    {
        //        await _authService.LogOutAsync();
        //        var checkAuth = _httpContext.User.Identity.IsAuthenticated;
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpGet]
        //public async Task<IEnumerable<AuthSettings>> GetAllUsers()
        //{
        //    try
        //    {
        //        return await _authService.GetAllUsersAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpGet("{id}")]
        //public async Task<User> GetUserById(string id)
        //{
        //    try
        //    {
        //        return await _authService.GetUserByIdAsync(id);
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpGet("by-email/{email}")]
        //public async Task<User> GetUserByEmail(string email)
        //{
        //    try
        //    {
        //        return await _authService.GetUserByEmailAsync(email);
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateUser([FromBody] User user)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        var response = await _authService.UpdateUserAsync(user);
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserById(string id)
        //{
        //    try
        //    {
        //        await _authService.DeleteUserByIdAsync(id);
        //        return new JsonResult("Deleted Successfully");
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpPost("createRole/{roleName}")]
        //public async Task<IActionResult> CreateRole(string roleName)
        //{
        //    try
        //    {
        //        await _authService.CreateRoleAsync(roleName);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpPost("addUserToRole/{roleName}/{userEmail}")]
        //public async Task<IActionResult> AddUserToRole(string userEmail, string roleName)
        //{
        //    try
        //    {
        //        await _authService.AddUserToRoleAsync(userEmail, roleName);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpGet("userRoles")]
        //public async Task<IActionResult> GetUserRole([FromBody] string email)
        //{
        //    try
        //    {
        //        await _authService.GetUserRolesAsync(email);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
    }
}
