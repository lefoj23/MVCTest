using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Repositories
{
    public interface IEmployeeRepository
    {
        public SearchResultViewModel GetEmployeeList(int pageNumber, int pageSize, string searchString);
        public bool AddEmployee(EmployeeViewModel viewModel);
        public bool UpdateEmployee(Guid id, EmployeeViewModel viewModel);
        public bool DeleteEmployee(Guid id);
        public EmployeeViewModel GetEmployee(Guid id);
    }
}
