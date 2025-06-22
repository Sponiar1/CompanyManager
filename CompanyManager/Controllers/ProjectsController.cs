using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyManager.Data;
using CompanyManager.Models;
using CompanyManager.Services;
using CompanyManager.Mappers;
using CompanyManager.Mappers.Validator;

namespace CompanyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            if (projects == null || !projects.Any())
            {
                return NotFound("No projects found.");
            }
            return Ok(projects);
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }
            return Ok(project);
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, [FromBody] ProjectDTO updatedProject)
        {
            if (updatedProject == null)
            {
                return BadRequest("Project data is required");
            }

            Project project = mapProject(updatedProject, id);
            if (!DataValidator.Validate(project, out var results))
            {
                foreach (var error in results)
                {
                    foreach (var memberName in error.MemberNames)
                    {
                        ModelState.AddModelError(memberName, error.ErrorMessage);
                    }
                    if (!error.MemberNames.Any())
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
                return BadRequest(ModelState);
            }
            try
            {
                var updated = await _projectService.UpdateProjectAsync(id, project);
                if (updated == null)
                {
                    return NotFound($"Project with ID {id} not found.");
                }
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject([FromBody] ProjectDTO newProject)
        {
            if (newProject == null)
            {
                return BadRequest("Project data is required");
            }
            Project project = mapProject(newProject);
            if (!DataValidator.Validate(project, out var results))
            {
                foreach (var error in results)
                {
                    foreach (var memberName in error.MemberNames)
                    {
                        ModelState.AddModelError(memberName, error.ErrorMessage);
                    }
                    if (!error.MemberNames.Any())
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
                return BadRequest(ModelState);
            }
            try
            {
                var createdProject = await _projectService.AddProjectAsync(project);
                return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id_Project }, createdProject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var deleted = await _projectService.DeleteProjectAsync(id);
            if (!deleted)
            {
                return NotFound($"Project with ID {id} not found.");
            }
            return Ok(deleted);
        }

        private Project mapProject(ProjectDTO projectDTO, int? id = null)
        {
            return new Project
            {
                Id_Project = id ?? 0,
                Pro_Name = projectDTO.Com_Name,
                Code = projectDTO.Code,
                Id_Division = projectDTO.Id_Division,
                Id_Boss = projectDTO.Id_Boss
            };
        }
    }
}
