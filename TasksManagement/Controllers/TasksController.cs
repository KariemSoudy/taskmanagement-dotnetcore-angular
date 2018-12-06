using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksManagement.Data;
using TasksManagement.Data.Interfaces;
using TasksManagement.Models;

namespace TasksManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUnitOfWork _tasksUnitOfWork;
        readonly UserManager<Data.Entities.User> _userManager;

        public TasksController(IUnitOfWork tasksUnitOfWork, UserManager<Data.Entities.User> userManager)
        {
            _tasksUnitOfWork = tasksUnitOfWork;
            _userManager = userManager;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<IEnumerable<TaskModel>> GetTasks()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);

            string role = roles.FirstOrDefault();

            return from task in _tasksUnitOfWork.GetTaksRepository().GetAll().Where(p => p.OwnerUserID == user.Id || p.AssignedToUserID == user.Id || role.ToLower() == "admin")
                   .IncludeMultiple(t => t.OwnerUser, t => t.AssignedToUser)
                   select new TaskModel
                   {
                       ID = task.ID,
                       Title = task.Title,
                       Description = task.Description,
                       Created = task.Created,
                       OwnerUser = new UserModel()
                       {
                           ID = task.OwnerUser.Id,
                           Username = task.OwnerUser.UserName,
                           Email = task.OwnerUser.Email
                       },
                       AssignedToUser = task.AssignedToUser != null ? new UserModel()
                       {
                           ID = task.AssignedToUser.Id,
                           Username = task.AssignedToUser.UserName,
                           Email = task.AssignedToUser.Email
                       } : null,
                       Completed = task.Completed
                   };
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tasks = await _tasksUnitOfWork.GetTaksRepository().GetAsync(p => p.ID == id);

            if (tasks == null)
            {
                return NotFound();
            }

            var task = tasks.FirstOrDefault();

            return Ok(new TaskModel
            {
                ID = task.ID,
                Title = task.Title,
                Description = task.Description,
                Created = task.Created,
                OwnerUser = new UserModel()
                {
                    ID = task.OwnerUser.Id,
                    Username = task.OwnerUser.UserName,
                    Email = task.OwnerUser.Email
                },
                AssignedToUser = task.AssignedToUser != null ? new UserModel()
                {
                    ID = task.AssignedToUser.Id,
                    Username = task.AssignedToUser.UserName,
                    Email = task.AssignedToUser.Email
                } : null,
                Completed = task.Completed
            });
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask([FromRoute] int id, [FromBody] TaskModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var tasks = _tasksUnitOfWork.GetTaksRepository().GetAll().Where(p => p.ID == id).IncludeMultiple(t => t.OwnerUser, t => t.AssignedToUser);

            if (tasks.Count() == 0)
                return NotFound();

            var task = tasks.FirstOrDefault();
            if (task == null)
                return NotFound();

            if (model.AssignedToUser == null)
            {
                if (model.Completed)
                {
                    if (task.AssignedToUserID == user.Id)
                    {
                        task.Completed = true;
                        task.AssignedToUserID = null;
                    }
                    else
                        return BadRequest(new { message = "This task is not assigned to you" });
                }
            }
            else
            {
                if (task.OwnerUserID == user.Id)
                {
                    if (model.AssignedToUser.ID == user.Id)
                        return BadRequest(new { message = "You can't assign the task to yourself" });
                    else
                    {
                        if (model.AssignedToUser.ID == "0")
                            task.AssignedToUserID = null;
                        else
                        {
                            task.AssignedToUserID = model.AssignedToUser.ID;
                            task.Completed = false;
                        }
                    }
                }
                else
                    return BadRequest(new { message = "You are not owner for this task" });
            }

            _tasksUnitOfWork.GetTaksRepository().Edit(task);

            try
            {
                await _tasksUnitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_tasksUnitOfWork.GetTaksRepository().Get(e => e.ID == id).Any())
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            task = _tasksUnitOfWork.GetTaksRepository().GetAll().Where(p => p.ID == id).IncludeMultiple(t => t.OwnerUser, t => t.AssignedToUser).FirstOrDefault();

            return Ok(new TaskModel
            {
                ID = task.ID,
                Title = task.Title,
                Description = task.Description,
                Created = task.Created,
                OwnerUser = new UserModel()
                {
                    ID = task.OwnerUser.Id,
                    Username = task.OwnerUser.UserName,
                    Email = task.OwnerUser.Email
                },
                AssignedToUser = task.AssignedToUser != null ? new UserModel()
                {
                    ID = task.AssignedToUser.Id,
                    Username = task.AssignedToUser.UserName,
                    Email = task.AssignedToUser.Email
                } : null,
                Completed = task.Completed
            });
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody] TaskModel taskModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var task = new Data.Entities.Task()
            {
                Title = taskModel.Title,
                Description = taskModel.Description,
                OwnerUserID = user.Id,
                Created = DateTime.Now
            };

            _tasksUnitOfWork.GetTaksRepository().Add(task);

            await _tasksUnitOfWork.SaveChangesAsync();

            task = _tasksUnitOfWork.GetTaksRepository().GetAll().Where(p => p.ID == task.ID).IncludeMultiple(t => t.OwnerUser, t => t.AssignedToUser).FirstOrDefault();

            return Ok(new TaskModel
            {
                ID = task.ID,
                Title = task.Title,
                Description = task.Description,
                Created = task.Created,
                OwnerUser = new UserModel()
                {
                    ID = task.OwnerUser.Id,
                    Username = task.OwnerUser.UserName,
                    Email = task.OwnerUser.Email
                },
                AssignedToUser = task.AssignedToUser != null ? new UserModel()
                {
                    ID = task.AssignedToUser.Id,
                    Username = task.AssignedToUser.UserName,
                    Email = task.AssignedToUser.Email
                } : null,
                Completed = task.Completed
            });
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);

            string role = roles.FirstOrDefault();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tasks = await _tasksUnitOfWork.GetTaksRepository().GetAsync(p => p.ID == id);
            if (tasks.Count == 0)
                return NotFound();

            var task = tasks.FirstOrDefault();
            if (task == null)
                return NotFound();

            if ((role.ToLower() == "admin") || (task.OwnerUserID == user.Id))
            {
                _tasksUnitOfWork.GetTaksRepository().Delete(p => p.ID == id);

                await _tasksUnitOfWork.SaveChangesAsync();

                return Ok(task);
            }

            return BadRequest(new { message = "You are not owner or admin to delete this task" });
        }
    }
}