using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Services.Data.Interfaces;
using HumanCapitalManagement.Web.ViewModels.Employee;
using HumanCapitalManagement.Web.ViewModels.Project;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Services.Data
{
    public class ProjectService : IProjectService
    {
        private readonly HumanCapitalManagementContext context;

        public ProjectService(HumanCapitalManagementContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ProjectViewModel>> GetAllAsync()
        {
            IEnumerable<ProjectViewModel> allProjects = await this.context
                .Projects
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProjectViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    ImgUrl = p.ImgUrl,
                    Salary = p.Salary,
                    EndDate = p.EndDate.ToString("f")
                })
                .ToListAsync();

            return allProjects;
        }

        public async Task<ProjectDetailsViewModel> GetByIdAsync(int id)
        {
            ProjectDetailsViewModel? projectDetails = await this.context
                .Projects
                .Where(p => p.IsDeleted == false && p.Id == id)
                .Select(p => new ProjectDetailsViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ImgUrl = p.ImgUrl,
                    Salary = p.Salary,
                    StartDate = p.StartDate.ToString("f"),
                    EndDate = p.EndDate.ToString("f"),
                    Employees = p.Employees.Select(e => new EmployeeViewModel()
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        MiddleName = e.MiddleName,
                        LastName = e.LastName,
                        Username = e.Username
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return projectDetails!;
        }

        public async Task AddAsync(AddEditProjectViewModel projectModel)
        {
            Project newProject = new Project()
            {
                Title = projectModel.Title,
                Description = projectModel.Description,
                ImgUrl = projectModel.ImgUrl,
                Salary = projectModel.Salary,
                StartDate = projectModel.StartDate,
                EndDate = projectModel.EndDate
            };

            await this.context.Projects.AddAsync(newProject);
            await this.context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, AddEditProjectViewModel projectModel)
        {
            Project? project = await this.context.Projects
                .FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);

            if (project != null)
            {
                project.Title = projectModel.Title;
                project.Description = projectModel.Description;
                project.ImgUrl = projectModel.ImgUrl;
                project.Salary = projectModel.Salary;
                project.StartDate = projectModel.StartDate;
                project.EndDate = projectModel.EndDate;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Project? projectToDelete = await this.context
                .Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (projectToDelete == null)
            {
                return false;
            }

            projectToDelete.IsDeleted = true;

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignAsync(int projectId, int employeeId)
        {
            Employee? employee = await this.context
                .Employees
                .Include(e => e.Projects)
                .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsEmployed);

            if (employee == null)
            {
                return false;
            }

            Project? project = await this.context
                .Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.IsDeleted == false);

            if (project == null)
            {
                return false;
            }

            if (!employee.Projects.Any(p => p.Id == projectId))
            {
                employee.Projects.Add(project);

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> LeaveAsync(int projectId, int employeeId)
        {
            Employee? employee = await this.context
                .Employees
                .Include(e => e.Projects)
                .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsEmployed);

            if (employee == null)
            {
                return false;
            }

            Project? project = await this.context
                .Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.IsDeleted == false);

            if (project == null)
            {
                return false;
            }

            if (employee.Projects.Any(p => p.Id == projectId))
            {
                employee.Projects.Remove(project);

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<ProjectViewModel>> GetAllAssignedAsync(int employeeId)
        {
            Employee? employee = await this.context
               .Employees
               .Include(e => e.Projects)
               .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsEmployed);

            IEnumerable<ProjectViewModel> allAssignedProjects = new List<ProjectViewModel>();

            if (employee != null)
            {
                allAssignedProjects = employee
                    .Projects
                    .Select(p => new ProjectViewModel()
                    {
                        Id = p.Id,
                        Title = p.Title,
                        ImgUrl = p.ImgUrl,
                        Salary = p.Salary,
                        EndDate = p.EndDate.ToString("f")
                    })
                    .ToList();

                return allAssignedProjects;
            }

            return allAssignedProjects;
        }
    }
}
