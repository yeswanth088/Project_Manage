using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Manage.Models
{
    public enum TaskStatus
    {
        Assigned,
        Started,
        Complete
    }
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int Project_id { get; set; }
        public int Employee_id { get; set; }
      
        public  TaskStatus Status{  get; set; }
        [ForeignKey("Project_id")]
        public Project? Pro { get; set; }
        [ForeignKey("Employee_id")]
        public Employee? Emp { get; set; }
    }
}
