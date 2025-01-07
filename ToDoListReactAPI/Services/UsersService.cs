using Microsoft.EntityFrameworkCore;
using System;
using ToDoListReactApi.API.Data;
using ToDoListReactApi.API.Models;


namespace ToDoListReactApi.API.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<Users>> GetAllUsersAsync();

        Task<IEnumerable<ToDoTasks>> GetAllTaskByUserId(int id);

        Task<bool> UpdateTaskStatus(int taskId, string newStatus);
    }

    public class UsersService : IUsersService
    {
        private readonly ToDoContext _context;

        public UsersService(ToDoContext context) 
        {
            _context = context;
        }

        //fetch all users form database
        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync(); 
        }

        //fetch tasks as per user id
        public async Task<IEnumerable<ToDoTasks>> GetAllTaskByUserId(int id)
        {
            return await _context.ToDoTasks
                .Where(task => task.userId == id)
                .ToListAsync();
        }

        public async Task<bool> UpdateTaskStatus(int taskId, string? newStatus)
        {
            newStatus = newStatus?.Trim().ToLower() switch
            {
                "completed" => "Completed",
                "pending" => "Pending",
                _ => null
            };

            if (newStatus == null) return false;

            var task = await _context.ToDoTasks.FindAsync(taskId);
            if (task == null) return false;

            task.status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
