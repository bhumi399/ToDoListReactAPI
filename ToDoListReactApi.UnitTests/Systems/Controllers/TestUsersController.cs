using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListReactApi.API.Controllers;
using ToDoListReactApi.API.Models;
using ToDoListReactApi.API.Services;
using Xunit;


namespace ToDoListReactApi.UnitTests.Systems.Controllers
{
    public class TestUsersController
    {
        private readonly Mock<IUsersService> _mockService;
        private readonly TodoController _controller;

        public TestUsersController()
        {
            _mockService = new Mock<IUsersService>();
            _controller = new TodoController(_mockService.Object);
        }

        //returns a list of users
        [Fact]
        public async Task GetAllUsers_ShouldReturnOkWithListOfUsers()
        {
            // Arrange
            var users = new List<Users>
            {
                new Users { userId = 1, name = "John Doe" },
                new Users { userId = 2, name = "Jane Smith" }
            };
            _mockService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(users);

            _mockService.Verify(service => service.GetAllUsersAsync(), Times.Once);
        }

        //returns an empty list of users
        [Fact]
        public async Task GetAllUsers_WhenNoUsersExist_ShouldReturnOkWithEmptyList()
        {
            // Arrange
            var emptyList = new List<Users>();
            _mockService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(emptyList);

            _mockService.Verify(service => service.GetAllUsersAsync(), Times.Once);
        }

        //service throws an exception
        [Fact]
        public async Task GetAllUsers_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllUsersAsync()).
                ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var statusCodeResult = result as ObjectResult;
            statusCodeResult.Should().NotBeNull();
            statusCodeResult.StatusCode.Should().Be(500);
            statusCodeResult.Value.Should().Be("An error occurred while fetching users.");

            _mockService.Verify(service => service.GetAllUsersAsync(), Times.Once);
        }

        //when user has tasks
        [Fact]
        public async Task GetTasksByUserId_WhenTasksExist_ReturnsOkWithTasks()
        {
            // Arrange
            int userId = 1;
            var mockTasks = new List<ToDoTasks>
            {
                new ToDoTasks { taskId = 1, userId = userId, Title = "Task 1", status = "Pending" },
                new ToDoTasks { taskId = 2, userId = userId, Title = "Task 2", status = "Completed" }
            };

            _mockService.Setup(service => service.GetAllTaskByUserId(userId))
                        .ReturnsAsync(mockTasks);

            // Act
            var result = await _controller.GetTasksByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(mockTasks);
        }

        //when user has no data
        [Fact]
        public async Task GetTasksByUserId_WhenNoTasksExist_ReturnsOkWithMessage()
        {
            // Arrange
            int userId = 2;

            _mockService.Setup(service => service.GetAllTaskByUserId(userId))
                        .ReturnsAsync(new List<ToDoTasks>());

            // Act
            var result = await _controller.GetTasksByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(new { message = $"No tasks found for this user id {userId}." });
        }

        //when exception is thrown
        [Fact]
        public async Task GetTasksByUserId_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            int userId = 3;

            _mockService.Setup(service => service.GetAllTaskByUserId(userId))
                        .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetTasksByUserId(userId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            objectResult.StatusCode.Should().Be(500);
            objectResult.Value.Should().Be("An error occurred while processing your request.");
        }

        //status is updated successfully
        [Fact]
        public async Task UpdateTaskStatus_WhenTaskExists_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            int taskId = 1;
            var updateDto = new TaskUpdateDto { Status = "Completed" };

            _mockService.Setup(service => service.UpdateTaskStatus(taskId, updateDto.Status))
                        .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateTaskStatus(taskId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(new { message = "Status updated successfully." });
        }

        //Task not found - 404
        [Fact]
        public async Task UpdateTaskStatus_WhenTaskNotFound_ReturnsNotFoundWithMessage()
        {
            // Arrange
            int taskId = 2;
            var updateDto = new TaskUpdateDto { Status = "Pending" };

            _mockService.Setup(service => service.UpdateTaskStatus(taskId, updateDto.Status))
                        .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateTaskStatus(taskId, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().BeEquivalentTo(new { message = "Task not found." });
        }

        //Invalid input data
        [Fact]
        public async Task UpdateTaskStatus_WhenStatusIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            int taskId = 3;
            var updateDto = new TaskUpdateDto { Status = string.Empty }; // Invalid empty status

            // Act
            var result = await _controller.UpdateTaskStatus(taskId, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("Invalid status value.");
        }



    }
}