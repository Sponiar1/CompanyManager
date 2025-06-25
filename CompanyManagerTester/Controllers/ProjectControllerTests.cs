using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyManager.Controllers;
using CompanyManager.Mappers;
using CompanyManager.Models;
using CompanyManager.Services.Templates;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CompanyManagerTester.Controllers
{
    public class ProjectControllerTests
    {
        private readonly ProjectsController _controller;
        private readonly Mock<IProjectService> _projectServiceMock;

        public ProjectControllerTests()
        {
            _projectServiceMock = new Mock<IProjectService>();
            _controller = new ProjectsController(_projectServiceMock.Object);
        }

        [Fact]
        public async void GetProjectsSuccess()
        {
            var projects = new List<Project>
            {
                new Project { Id_Project = 1, Pro_Name = "Test Project 1", Code = "P01", Id_Division = 1, Id_Boss = 1 },
                new Project { Id_Project = 2, Pro_Name = "Test Project 2", Code = "P02", Id_Division = 2, Id_Boss = 2 }
            };
            _projectServiceMock.Setup(s => s.GetAllProjectsAsync()).ReturnsAsync(projects);
            var result = await _controller.GetProjects();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Project>>(okResult.Value);
            Assert.NotNull(result);
            Assert.Contains(returnValue, p => p.Pro_Name == "Test Project 1" && p.Code == "P01");
            Assert.Contains(returnValue, p => p.Pro_Name == "Test Project 2" && p.Code == "P02");
        }
        [Fact]
        public async void GetProjectsNotFound()
        {
            _projectServiceMock.Setup(s => s.GetAllProjectsAsync()).ReturnsAsync(new List<Project>());
            var result = await _controller.GetProjects();
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No projects found.", notFoundResult.Value);
        }
        [Fact]
        public async void GetProjectsException()
        {
            _projectServiceMock.Setup(s => s.GetAllProjectsAsync()).Throws(new Exception("Database error"));
            var result = await _controller.GetProjects();
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Database error", statusCodeResult.Value);
        }
        [Fact]
        public async void GetProjectByIdSuccess()
        {
            var project = CreateProject();
            _projectServiceMock.Setup(s => s.GetProjectByIdAsync(1)).ReturnsAsync(project);
            var result = await _controller.GetProject(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Project>(okResult.Value);
            Assert.Equal("Test Project", returnValue.Pro_Name);
        }
        [Fact]
        public async void GetProjectByIdNotFound()
        {
            _projectServiceMock.Setup(s => s.GetProjectByIdAsync(1)).ReturnsAsync((Project)null);
            var result = await _controller.GetProject(1);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Project was not found.", notFoundResult.Value);
        }
        [Fact]
        public async void GetProjectByIdException()
        {
            _projectServiceMock.Setup(s => s.GetProjectByIdAsync(1)).Throws(new Exception("Database error"));
            var result = await _controller.GetProject(1);
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Database error", statusCodeResult.Value);
        }
        [Fact]
        public async void CreateProjectSuccess()
        {
            var projectDTO = CreateProjectDTO();
            var project = CreateProject();
            _projectServiceMock.Setup(s => s.AddProjectAsync(It.IsAny<Project>())).ReturnsAsync(project);
            var result = await _controller.PostProject(projectDTO);
            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Project>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal("Test Project", returnValue.Pro_Name);
        }
        [Fact]
        public async void CreateProjectBadRequest()
        {
            var projectDTO = CreateProjectDTO();
            _projectServiceMock.Setup(s => s.AddProjectAsync(It.IsAny<Project>())).Throws(new ArgumentException("Database update failed: Division does not exist."));
            var result = await _controller.PostProject(projectDTO);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Database update failed: Division does not exist.", badRequestResult.Value);
        }
        [Fact]
        public async void UpdateProjectSuccess()
        {
            var projectDTO = CreateProjectDTO();
            var project = CreateProject();
            _projectServiceMock.Setup(s => s.UpdateProjectAsync(1, It.IsAny<Project>())).ReturnsAsync(project);
            var result = await _controller.PutProject(1, projectDTO);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Project>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal("Test Project", returnValue.Pro_Name);
        }
        [Fact]
        public async void UpdateProjectNotFound()
        {
            var projectDTO = CreateProjectDTO();
            _projectServiceMock.Setup(s => s.UpdateProjectAsync(1, It.IsAny<Project>())).ReturnsAsync((Project)null);
            var result = await _controller.PutProject(1, projectDTO);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Project not found.", notFoundResult.Value);
        }
        [Fact]
        public async void DeleteProjectSuccess()
        {
            _projectServiceMock.Setup(s => s.DeleteProjectAsync(1)).ReturnsAsync(true);
            var result = await _controller.DeleteProject(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, okResult.Value);
        }
        [Fact]
        public async void DeleteProjectNotFound()
        {
            _projectServiceMock.Setup(s => s.DeleteProjectAsync(1)).ReturnsAsync(false);
            var result = await _controller.DeleteProject(1);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Project was not found.", notFoundResult.Value);
        }
        private Project CreateProject()
        {
            return new Project
            {
                Id_Project = 1,
                Pro_Name = "Test Project",
                Code = "P01",
                Id_Division = 1,
                Id_Boss = 1,
            };
        }
        private ProjectDTO CreateProjectDTO()
        {
            return new ProjectDTO
            {
                Pro_Name = "Test Project",
                Code = "P01",
                Id_Division = 1,
                Id_Boss = 1,
            };
        }
    }
}
