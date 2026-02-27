using System;

namespace Project_Manage.Services
{
    [Serializable]
    public class ProjectNotFoundException : Exception 
    {
        public ProjectNotFoundException()
        {
        }

        public ProjectNotFoundException(string? message) : base(message)
        {
        }

        public ProjectNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}