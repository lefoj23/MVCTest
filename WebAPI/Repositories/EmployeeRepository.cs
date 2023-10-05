using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Repositories
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public EmployeeRepository(IConfiguration configuration) {
            _configuration = configuration;
            _connectionString = _configuration.GetValue<string>("ConnectionString") ?? "";
        }

        public SearchResultViewModel GetEmployeeList(int pageNumber,int pageSize,string searchString)
        {
            var result = new SearchResultViewModel();
            var employeeList = new List<Employee>();

            result.PageNumber = pageNumber;
            result.TotalCount = 0;
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string storedProcedureName = "FETCH_EMPLOYEELIST";

                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PAGEINDEX", pageNumber - 1);
                        command.Parameters.AddWithValue("@PAGESIZE", pageSize);
                        command.Parameters.AddWithValue("@SEARCHSTRING", searchString);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {          
                                var employee = new Employee()
                                {
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    EmployeeNumber = reader["EmployeeNumber"].ToString() ?? string.Empty
                                };

                                var id = reader["Id"].ToString();
                                var createdDate = reader["createdDate"].ToString();
                                var modifiedDate = reader["modifiedDate"].ToString();

                                employee.Id = !string.IsNullOrEmpty(id) ? Guid.Parse(id) : Guid.Empty;
                                employee.CreatedDate = !string.IsNullOrEmpty(createdDate) ? DateTimeOffset.Parse(createdDate) : new DateTimeOffset();
                                employee.ModifiedDate = !string.IsNullOrEmpty(modifiedDate) ? DateTimeOffset.Parse(modifiedDate) : new DateTimeOffset();

                                employeeList.Add(employee);
                            }

                            result.DataList = employeeList;
                            reader.NextResult();
                            if (reader.Read())
                            {
                                result.TotalCount = reader.GetInt32(0);
                            }
                        }

                    }
                }


                return result;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool AddEmployee(EmployeeViewModel viewModel)
        {
            if (viewModel == null) return false;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string storedProcedureName = "INSERT_EMPLOYEE";

                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", viewModel.Name);
                        command.Parameters.AddWithValue("@Email", viewModel.Email);
                        command.Parameters.AddWithValue("@EmployeeNumber", viewModel.EmployeeNumber);   
                        command.Parameters.AddWithValue("@Position", viewModel.Name ?? string.Empty);
                        command.Parameters.AddWithValue("@Department", viewModel.Department ?? string.Empty);
                        command.Parameters.AddWithValue("@DateHired", viewModel.DateHired ?? null);
                        command.Parameters.AddWithValue("@Gender", viewModel.Gender ?? string.Empty);
                        command.Parameters.AddWithValue("@Birthdate", viewModel.Birthdate ?? null);
                        command.Parameters.AddWithValue("@Address", viewModel.Address ?? string.Empty);
                        command.Parameters.AddWithValue("@ContactNumber", viewModel.ContactNumber ?? string.Empty);
                        command.Parameters.AddWithValue("@MaritalStatus", viewModel.MaritalStatus ?? string.Empty);
                        command.Parameters.AddWithValue("@GovernmentId", viewModel.GovernmentId ?? string.Empty);
                        command.Parameters.AddWithValue("@EmergencyContact", viewModel.EmergencyContact ?? string.Empty);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {                       
                            if (reader.Read())
                            {
                                return reader.GetInt32(0) >= 1 ;
                            }
                        }

                    }
                }


                return true;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }


        public bool UpdateEmployee(Guid id, EmployeeViewModel viewModel)
        {
            if (viewModel == null) return false;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string storedProcedureName = "UPDATE_EMPLOYEE";

                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Name", viewModel.Name);
                        command.Parameters.AddWithValue("@Email", viewModel.Email);
                        command.Parameters.AddWithValue("@EmployeeNumber", viewModel.EmployeeNumber);
                        command.Parameters.AddWithValue("@Position", viewModel.Name ?? string.Empty);
                        command.Parameters.AddWithValue("@Department", viewModel.Department ?? string.Empty);
                        command.Parameters.AddWithValue("@DateHired", viewModel.DateHired ?? null);
                        command.Parameters.AddWithValue("@Gender", viewModel.Gender ?? string.Empty);
                        command.Parameters.AddWithValue("@Birthdate", viewModel.Birthdate ?? null);
                        command.Parameters.AddWithValue("@Address", viewModel.Address ?? string.Empty);
                        command.Parameters.AddWithValue("@ContactNumber", viewModel.ContactNumber ?? string.Empty);
                        command.Parameters.AddWithValue("@MaritalStatus", viewModel.MaritalStatus ?? string.Empty);
                        command.Parameters.AddWithValue("@GovernmentId", viewModel.GovernmentId ?? string.Empty);
                        command.Parameters.AddWithValue("@EmergencyContact", viewModel.EmergencyContact ?? string.Empty);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(0) >= 1;
                            }
                        }

                    }
                }


                return true;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool DeleteEmployee(Guid id)
        {

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string storedProcedureName = "DELETE_EMPLOYEE";

                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
       

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(0) >= 1;
                            }
                        }

                    }
                }


                return true;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }
        public EmployeeViewModel GetEmployee(Guid id)
        {
            var result = new EmployeeViewModel();
  
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string storedProcedureName = "FETCH_EMPLOYEE";

                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                   

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                              
                                var employeeId = reader["EmployeeId"].ToString();
                                var dateHired = reader["DateHired"].ToString();
                                var birthdate = reader["Birthdate"].ToString();
             

                                var employee = new EmployeeViewModel()
                                {
                                    Name = reader["Name"].ToString() ?? string.Empty,
                                    Email = reader["Email"].ToString() ?? string.Empty,
                                    EmployeeNumber = reader["EmployeeNumber"].ToString() ?? string.Empty,
                                    EmployeeId = !string.IsNullOrEmpty(employeeId) ? Guid.Parse(employeeId) : Guid.Empty,
                                    Position = reader["Position"].ToString() ?? string.Empty,
                                    Department = reader["Department"].ToString() ?? string.Empty,
                                    DateHired = !string.IsNullOrEmpty(dateHired) ? DateTime.Parse(dateHired) : new DateTime(),
                                    Gender = reader["Gender"].ToString() ?? string.Empty,
                                    Address = reader["Address"].ToString() ?? string.Empty,
                                    ContactNumber = reader["ContactNumber"].ToString() ?? string.Empty,
                                    MaritalStatus = reader["MaritalStatus"].ToString() ?? string.Empty,
                                    GovernmentId = reader["GovernmentId"].ToString() ?? string.Empty,
                                    EmergencyContact = reader["EmergencyContact"].ToString() ?? string.Empty,
                                };

                            
                                result = employee;
                            }

                  
                       
                        }

                    }
                }


                return result;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
