using GraflowBackend.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GraflowBackend.Controllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly GraflowContext m_Context;
        private readonly SignInManager<User> m_SignInManager;
        private readonly UserManager<User> m_UserManager;

        public ProjectController(GraflowContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            m_Context = context;
            m_SignInManager = signInManager;
            m_UserManager = userManager;
        }

        [HttpGet()]
        public IActionResult GetProjects([FromQuery] Guid? id)
        {
            if (!m_SignInManager.IsSignedIn(User))
            {
                return Unauthorized();
            }
            Guid userId;
            if (!Guid.TryParse(m_UserManager.GetUserId(User), out userId))
            {
                return Unauthorized();
            }
            if (id == null)
            {
                return Ok(m_Context.Projects.Where(p => p.Owner == userId).ToArray());
            }
            return Ok(m_Context.Projects.FirstOrDefault(p => p.Owner == userId && p.Id == id));
        }

        [HttpPost()]
        public IActionResult CreateProject([FromBody] ProjectCreateInfo info)
        {
            if (!m_SignInManager.IsSignedIn(User))
            {
                return Unauthorized();
            }

            Guid userId = Guid.Empty;
            if (!Guid.TryParse(m_UserManager.GetUserId(User), out userId))
            {
                return Unauthorized();
            }
            Project project = new()
            {
                Id = Guid.NewGuid(),
                Owner = userId,
                Name = info.Name,
                CreationDate = DateTime.UtcNow,
                LastAccessDate = DateTime.UtcNow,
            };

            var status = m_Context.Projects.Add(project);
            if (status.State != Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return Ok();
            }
            m_Context.SaveChanges();
            return Ok(project.Id);
        }

        [HttpDelete()]
        public IActionResult DeleteProject([FromBody] ProjectDeleteInfo info)
        {
            if (!m_SignInManager.IsSignedIn(User))
            {
                return Unauthorized();
            }
            Guid userId;
            if (!Guid.TryParse(m_UserManager.GetUserId(User), out userId))
            {
                return Unauthorized();
            }
            Project? project = m_Context.Projects.FirstOrDefault(p => p.Id == info.Id && p.Owner == userId);
            if (project == null)
            {
                return Ok(APIReturnFlags.RESOURCE_NOT_FOUNT);
            }
            var status = m_Context.Projects.Remove(project);
            if (status.State != Microsoft.EntityFrameworkCore.EntityState.Deleted)
            {
                return Ok(APIReturnFlags.BAD_REQUEST);
            }
            m_Context.SaveChanges();
            return Ok(APIReturnFlags.SUCCESS);
        }
    }
}
