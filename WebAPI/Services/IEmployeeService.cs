using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Services
{
    public interface IEmployeeService
    {
        public SearchResultViewModel GetEmployeeList(int pageIndex, int pageSize, string searchString);
        public bool AddEmployee(List<Dictionary<string, string>> data);
        public bool UpdateEmployee(Guid id, List<Dictionary<string, string>> data);
        public EmployeeViewModel GetEmployee(Guid id);
        public bool DeleteEmployee(Guid id);

    }
}
