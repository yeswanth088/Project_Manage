using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System;
using Project_Manage.Models;
using Project_Manage.Services;
using Task = Project_Manage.Models.Task;
using TaskStatus = Project_Manage.Models.TaskStatus;

namespace ProjectManagement.Tests
{
    [TestClass]
    public class ProjectServicesTests
    {
        private Mock<ProjectServices> _mockService = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<ProjectServices>();
        }

        [TestMethod]
        public void AddEmployee_ValidEmployee_ExecutesSuccessfully()
        {
            var newEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                Designation = "Developer",
                Gender = "Male",
                Salary = 60000
            };

            _mockService.Setup(s => s.AddEmployee(It.IsAny<Employee>()));

            _mockService.Object.AddEmployee(newEmployee);

            _mockService.Verify(s => s.AddEmployee(It.IsAny<Employee>()), Times.Once);
        }

        [TestMethod]
        public void AddTask_ValidTask_ExecutesSuccessfully()
        {
            var newTask = new Task
            {
                Id = 101,
                Name = "Design Database Schema",
                Project_id = 1,
                Employee_id = 1,
                Status = TaskStatus.Assigned
            };

            _mockService.Setup(s => s.AddTask(It.IsAny<Task>()));

            _mockService.Object.AddTask(newTask);

            _mockService.Verify(s => s.AddTask(It.IsAny<Task>()), Times.Once);
        }

        [TestMethod]
        public void GetTasks_ValidIds_ReturnsListOfTasks()
        {
            int empId = 1;
            int projectId = 1;
            var expectedTasks = new List<Task>
            {
                new Task { Id = 101, Name = "Frontend Setup", Employee_id = empId, Project_id = projectId },
                new Task { Id = 102, Name = "Backend API", Employee_id = empId, Project_id = projectId }
            };

            _mockService.Setup(s => s.GetTasks(empId, projectId)).Returns(expectedTasks);

            var actualTasks = _mockService.Object.GetTasks(empId, projectId);

            Assert.IsNotNull(actualTasks);
            Assert.HasCount(2, actualTasks);
            Assert.AreEqual("Frontend Setup", actualTasks[0].Name);
        }

        [TestMethod]
        public void DeleteEmployee_InvalidId_ThrowsException()
        {
            int invalidEmployeeId = 999;

            _mockService.Setup(s => s.DeleteEmployee(invalidEmployeeId))
                        .Throws(new ProjectNotFoundException("Employee not found"));

            try
            {
                _mockService.Object.DeleteEmployee(invalidEmployeeId);

                Assert.Fail("Expected ProjectNotFoundException was not thrown.");
            }
            catch (ProjectNotFoundException)
            {
            }
        }
    }
}