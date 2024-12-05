using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GraflowBackend.Controllers
{
    [Route("api/engine")]
    [ApiController]
    public class EngineController : ControllerBase
    {
        private readonly SignInManager<User> m_SingInManager;

        public EngineController(SignInManager<User> singInManager)
        {
            m_SingInManager = singInManager;
        }
    }
}
