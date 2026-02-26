using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Project_Manage.Models
{

    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public int Salary { get; set; }
        
        public int? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? project { get; set; }
       
       
    }
}

