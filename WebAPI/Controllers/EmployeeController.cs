using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Services;
using System.Xml.Linq;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Services;
using WebAPI.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EmployeeController>/5
        [HttpGet("list")]
        public ActionResult<SearchResultViewModel> List([FromQuery]int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchString = "")
        {
            var result = _employeeService.GetEmployeeList(pageNumber, pageSize, searchString);

            return result;
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public EmployeeViewModel Get([FromRoute]Guid id)
        {
            var result = _employeeService.GetEmployee(id);

            return result;
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public ActionResult Post([FromBody] List<Dictionary<string,string>> data)
        {
            try
            {
               var result = _employeeService.AddEmployee(data);

                return Ok(result);
            }
            catch(Exception e)
            {
                var errMessage = "Err:" + e.Message;
                if (e.InnerException != null) 
                    errMessage += "\n" + "InnerException:" + e.InnerException.Message;

                return BadRequest(errMessage);
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public ActionResult Put([FromRoute]Guid id, [FromBody] List<Dictionary<string, string>> data)
        {
            try
            {
                var result = _employeeService.UpdateEmployee(id, data);

                return Ok(result);
            }
            catch (Exception e)
            {
                var errMessage = "Err:" + e.Message;
                if (e.InnerException != null)
                    errMessage += "\n" + "InnerException:" + e.InnerException.Message;

                return BadRequest(errMessage);
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {

            try
            {
                var result = _employeeService.DeleteEmployee(id);

                return Ok(result);
            }
            catch (Exception e)
            {
                var errMessage = "Err:" + e.Message;
                if (e.InnerException != null)
                    errMessage += "\n" + "InnerException:" + e.InnerException.Message;

                return BadRequest(errMessage);
            }
        }
    }
}
