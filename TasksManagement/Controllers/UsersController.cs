using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksManagement.Models;

namespace TasksManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly UserManager<Data.Entities.User> _userManager;

        public UsersController(UserManager<Data.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetTasks()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var usersList = await _userManager.GetUsersInRoleAsync("support");

            var users = from user in usersList
                        where user.Id != currentUser.Id
                        select new UserModel()
                        {
                            ID = user.Id,
                            Username = user.UserName,
                            Email = user.Email
                        };

            return users.OrderBy(p => p.Username);
        }
    }
}