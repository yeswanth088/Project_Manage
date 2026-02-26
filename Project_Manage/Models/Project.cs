using System.ComponentModel.DataAnnotations;


namespace Project_Manage.Models
{
    public enum ProjectStatus
    {
        Started,
        Dev,
        Build,
        Test,
        Deployed
    }
    public class Project
    {
        [Key]
        public int Id {  get; set; }
        public string Name {  get; set; }
        public string Description { get; set; } 
        public string Start_date { get; set; }
        
        public ProjectStatus Status { get; set; }
    }
}
