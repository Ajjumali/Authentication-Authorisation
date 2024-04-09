using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Contract;
using SharedClassLibrary.DTOs;
using SharedClassLibrary.UserDTO;

namespace IdentityManagerServiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("register")]
        public async  Task<ActionResult> Register(UserDTOs userDTOs)
        {
            if (userDTOs == null)  return BadRequest();
            var responce = await userAccount.CreateAccount(userDTOs);
            return Ok(responce);
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null) return BadRequest();
            var responce = await userAccount.LoginAccount(loginDTO);
            return Ok(responce);
        }
    }
}
