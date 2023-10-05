using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.ViewModels;

namespace WebAPI.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository) {
            _employeeRepository = employeeRepository;
        }

        public SearchResultViewModel GetEmployeeList(int pageNumber, int pageSize, string searchString)
        {
            return _employeeRepository.GetEmployeeList(pageNumber, pageSize, searchString);
        }

        public bool AddEmployee(List<Dictionary<string, string>> data)
        {
            var dict = new Dictionary<string, object>();

            foreach(var prop in data)
            {
                Console.WriteLine(prop);

                var name = prop["name"];
                var value = prop["value"].ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(name))
                    dict.Add(name, value);

            }

            var jsonString = JsonConvert.SerializeObject(dict);
            var viewModel = JsonConvert.DeserializeObject<EmployeeViewModel>(jsonString);

            return _employeeRepository.AddEmployee(viewModel);

        }

        public bool UpdateEmployee(Guid id, List<Dictionary<string, string>> data)
        {
            var dict = new Dictionary<string, object>();

            foreach (var prop in data)
            {
                var name = prop["name"];
                var value = prop["value"].ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(name))
                    dict.Add(name, value);

            }

            var jsonString = JsonConvert.SerializeObject(dict);
            var viewModel = JsonConvert.DeserializeObject<EmployeeViewModel>(jsonString);

            return _employeeRepository.UpdateEmployee(id, viewModel);

        }


        public bool DeleteEmployee(Guid id)
        {
  
            return _employeeRepository.DeleteEmployee(id);

        }
        public EmployeeViewModel GetEmployee(Guid id)
        {
            return _employeeRepository.GetEmployee(id);
        }
    }
}
