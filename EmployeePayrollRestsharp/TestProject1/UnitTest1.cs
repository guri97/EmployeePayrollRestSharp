using EmployeePayrollRestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Nodes;

namespace EmployeePayrollTest
{
    public class RestsharpTest
    {
        RestClient client = new RestClient("http://localhost:4000");

        [SetUp]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        /// <summary>
        /// Gets the employee list.
        /// </summary>
        /// <returns></returns>
        //class to get list of employees
        public RestResponse GetEmployeeList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            //passing rest request for employees list api using get method
            RestRequest request = new RestRequest("/employees", Method.Get);
            //Act
            //Execute the request
            RestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// UC 1 : Retrieve all employee details in the json file
        /// </summary>
        [Test]
        public void OnCallingGetAPI_ReturnEmployeeList()
        {
            //calling the method
            RestResponse response = GetEmployeeList();
            //checks if the status code of response equals the employee code for the method requested
            //and checks response as okay or not
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //convert the response object to list of employees
            //get 
            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            //checking whether list is equal to count
            Assert.AreEqual(4, employeeList.Count);

            foreach (Employee emp in employeeList)
            {
                Console.WriteLine("Id: " + emp.Id + "\t" + "Name: " + emp.Name + "\t" + "Salary: " + emp.Salary);
            }
        }
        /// <summary>
        /// UC 2 : Add new employee to the json file in JSON server and return the same
        /// </summary>
        [Test]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            //Arrange
            //Initialize the request for POST to add new employee
            RestRequest request = new RestRequest("/employees", Method.Post);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("name", "Mounesh");
            jsonObj.Add("salary", "78400");

            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            //Act
            RestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Mounesh", employee.Name);
            Assert.AreEqual("78400", employee.Salary);
            Console.WriteLine(response.Content);
        }
        /// <summary>
        /// UC 3 : Adds multiple employees to the json file in JSON server and returns the same
        /// </summary>
        [Test]
        public void OnCallingPostAPIForAEmployeeListWithMultipleEMployees_ReturnEmployeeObject()
        {
            // Arrange
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { Name = "Kylie ", Salary = "885040" });
            employeeList.Add(new Employee { Name = "Kendall ", Salary = "125030" });
            employeeList.Add(new Employee { Name = "Kim ", Salary = "125040" });
            //Iterate the loop for each employee
            foreach (var v in employeeList)
            {
                //Initialize the request for POST to add new employee
                RestRequest request = new RestRequest("/employees", Method.Post);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("Name", v.Name);
                jsonObj.Add("Salary", v.Salary);
                // jsonObj.Add("Id", v.Id);
                //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                RestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(v.Name, employee.Name);
                Assert.AreEqual(v.Salary, employee.Salary);
                Console.WriteLine(response.Content);
            }
        }
    }
}



