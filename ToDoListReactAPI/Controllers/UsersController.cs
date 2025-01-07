using Microsoft.AspNetCore.Mvc;
using ToDoListReactApi.API.Models;
using ToDoListReactApi.API.Services;

namespace ToDoListReactApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public TodoController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _usersService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching users.");
            }
        }

        [HttpGet("GetAllTasksByUserId/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            try
            {
                var tasks = await _usersService.GetAllTaskByUserId(userId);

                if (tasks == null || !tasks.Any())
                {
                    return Ok(new { message = $"No tasks found for this user id {userId}." });
                }
                return Ok(tasks);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("UpdateTaskStatus/{taskId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] TaskUpdateDto updateDto)
        {
            if(string.IsNullOrWhiteSpace(updateDto.Status))
            {
                return BadRequest("Invalid status value.");
            }

            var success = await _usersService.UpdateTaskStatus(taskId, updateDto.Status);

            if (!success)
                return NotFound(new { message = "Task not found." });

            return Ok(new { message = "Status updated successfully." });
        }

    }
}
