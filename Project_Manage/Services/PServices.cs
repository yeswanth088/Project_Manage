using Project_Manage.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Project_Manage.Services
{
    public class PServices : ProjectServices
    {
        private readonly ProjectContext _context;

        public PServices(ProjectContext context)
        {
            _context = context;
        }
        public void AddEmployee(Employee emp)
        {
            _context.employees.Add(emp);
            _context.SaveChanges();
        }

        public void AddProject(Project project)
        {
            _context.projects.Add(project);
            _context.SaveChanges();
        }

        public void AddTask(Models.Task task)
        {
            _context.tasks.Add(task);
            _context.SaveChanges();
        }

        public void AssignProjectToEmployee(int projectId, int employeeId)
        {
            var emp = _context.employees.Find(employeeId);
            if (emp == null)
                throw new EmployeeNotFoundException("Employee not found");

            var project = _context.projects.Find(projectId);
            if (project == null)
                throw new ProjectNotFoundException("Project not found");

            emp.ProjectId = projectId;
            _context.SaveChanges();
        }

        public List<Models.Task> GetTasks(int empId, int projectId)
        {
            return _context.tasks
                .Where(t => t.Employee_id == empId && t.Project_id == projectId)
                .ToList();
        }

        public void DeleteEmployee(int id)
        {
            var emp = _context.employees.Find(id);
            if (emp == null)
                throw new EmployeeNotFoundException("Employee not found");

            _context.employees.Remove(emp);
            _context.SaveChanges();
        }
    }
}

namespace Project_Manage
{
    [Serializable]
    class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException()
        {
        }

        public EmployeeNotFoundException(string? message) : base(message)
        {
        }

        public EmployeeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}