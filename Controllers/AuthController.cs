using GraflowBackend.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GraflowBackend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GraflowContext m_Context;
        private readonly SignInManager<User> m_SignInManager;
        private readonly UserManager<User> m_UserManager;
        
        public AuthController(GraflowContext context, SignInManager<User> signInManager, UserManager<User> userManager) {
            m_Context = context;
            m_SignInManager = signInManager;
            m_UserManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInfo info)
        {
            var result = await m_SignInManager.PasswordSignInAsync(info.Username, info.Password, false, false);
            if (result.Succeeded)
            {
                return Ok(APIReturnFlags.SUCCESS);
            }
            return Ok(APIReturnFlags.SIGN_IN_FAILED);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInfo info)
        {
            APIReturnFlags status = 0;
            if (m_Context.Users.Any(u => u.NormalizedUsername == info.Username.ToUpper()))
            {
                status |= APIReturnFlags.USERNAME_TAKEN;
            }
            if (m_Context.Users.Any(u => u.NormalizedEmail == info.Email.ToUpper()))
            {
                status |= APIReturnFlags.EMAIL_TAKEN;
            }
            if ((status & APIReturnFlags.USERNAME_TAKEN) == APIReturnFlags.USERNAME_TAKEN
                || (status & APIReturnFlags.EMAIL_TAKEN) == APIReturnFlags.EMAIL_TAKEN)
            {
                return Ok(status);
            }

            User newUser = new()
            {
                Username = info.Username,
                NormalizedUsername = info.Username.ToLower(),
                Email = info.Email,
                NormalizedEmail = info.Email.ToLower(),
                IsEmailConfirmed = false
            };
            var result = await m_UserManager.CreateAsync(newUser, info.Password);
            if (result.Succeeded == false)
            {
                return Ok(APIReturnFlags.BAD_REQUEST);
            }
            var signInResult = await m_SignInManager.PasswordSignInAsync(newUser, info.Password, false, false);
            if (!signInResult.Succeeded)
            {
                return Ok(APIReturnFlags.SIGN_IN_FAILED);
            }
            return Ok(APIReturnFlags.SUCCESS);
        }

        [HttpGet("isLogged")]
        public IActionResult IsLogged()
        {
            if (m_SignInManager.IsSignedIn(User))
            {
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await m_SignInManager.SignOutAsync();
            return Ok();
        }
    }
}
