using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksManagement.Data;
using TasksManagement.Data.Interfaces;
using TasksManagement.Models;

namespace TasksManagement.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUnitOfWork _tasksUnitOfWork;

        public TasksController(IUnitOfWork tasksUnitOfWork)
        {
            _tasksUnitOfWork = tasksUnitOfWork;
        }

        // GET: api/Tasks
        [HttpGet]
        public IEnumerable<TaskModel> GetTasks()
        {
            return from task in _tasksUnitOfWork.GetTaksRepository().GetAll().IncludeMultiple(t => t.OwnerUser, t => t.AssignedToUser)
                   select new TaskModel
                   {
                       ID = task.ID,
                       Title = task.Title,
                       Description = task.Description,
                       Created = task.Created,
                       OwnerUser = new UserModel()
                       {
                           UserID = task.OwnerUser.Id,
                           Username = task.OwnerUser.UserName,
                           Email = task.OwnerUser.Email
                       },
                       AssignedToUser = task.AssignedToUser != null ? new UserModel()
                       {
                           UserID = task.AssignedToUser.Id,
                           Username = task.AssignedToUser.UserName,
                           Email = task.AssignedToUser.Email
                       } : null,
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

            var task = await _tasksUnitOfWork.GetTaksRepository().GetAsync(p => p.ID == id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask([FromRoute] int id, [FromBody] Data.Entities.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.ID)
            {
                return BadRequest();
            }

            _tasksUnitOfWork.GetTaksRepository().Edit(task);

            try
            {
                await _tasksUnitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<IActionResult> PostTask([FromBody] Data.Entities.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _tasksUnitOfWork.GetTaksRepository().Add(task);

            await _tasksUnitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.ID }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _tasksUnitOfWork.GetTaksRepository().GetAsync(p => p.ID == id);
            if (task == null)
            {
                return NotFound();
            }

            _tasksUnitOfWork.GetTaksRepository().Delete(p => p.ID == id);

            await _tasksUnitOfWork.SaveChangesAsync();

            return Ok(task);
        }

        private bool TaskExists(int id)
        {
            return _tasksUnitOfWork.GetTaksRepository().Get(e => e.ID == id).Any();
        }
    }
}