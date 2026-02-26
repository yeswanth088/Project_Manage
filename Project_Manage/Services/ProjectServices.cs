namespace Project_Manage.Models
{
    public interface ProjectServices
    {
        void AddEmployee(Employee emp);
        void AddProject(Project project);
        void AddTask(Task task);
        void AssignProjectToEmployee(int projectId, int employeeId);
        List<Task> GetTasks(int empId, int projectId);
        void DeleteEmployee(int id);
    }
}
