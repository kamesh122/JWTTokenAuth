using AutoMapper;
using JWTAuth.WebApi.DTOs;
using JWTAuth.WebApi.Interface;
using JWTAuth.WebApi.Models;
using JWTAuth.WebApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JWTAuth.WebApi.Controllers
{
    [Authorize]
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployees _IEmployee;
        private readonly IIdempotencyService _idempotencyService;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployees IEmployee, IIdempotencyService idempotencyService, IMapper mapper)
        {
            _IEmployee = IEmployee;
            _idempotencyService = idempotencyService;
            _mapper = mapper;
        }

        // GET: api/employee>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> Get()
        {
            string idempotencyKey = Request.Headers["Idempotency-Key"];


            // Process your logic here
            var result = _IEmployee.GetEmployeeDetails();
            IEnumerable<EmployeeDTO> enumerable = result.Select(emp => _mapper.Map<EmployeeDTO>(emp));
            
            // Store the response
            string responseBody = JsonConvert.SerializeObject(enumerable);
            await _idempotencyService.StoreResponseForKeyAsync(idempotencyKey, "Get Empolyee", responseBody, 200);

            return await Task.FromResult(enumerable.ToList());
        }

        

        // GET api/employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            var employees = await Task.FromResult(_IEmployee.GetEmployeeDetails(id));
            if (employees == null)
            {
                return NotFound();
            }
            return employees;
        }

        // POST api/employee
        [HttpPost]
        public async Task<ActionResult<Employee>> Post(Employee employee)
        {
            _IEmployee.AddEmployee(employee);
            //return await Task.FromResult(CreatedAtAction("GetEmployees", new { id = employee.EmployeeID }, employee));
            return await Task.FromResult(employee);
        }

        // PUT api/employee/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> Put(int id, Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }
            try
            {
                _IEmployee.UpdateEmployee(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return await Task.FromResult(employee);
        }

        // DELETE api/employee/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> Delete(int id)
        {
            var employee = _IEmployee.DeleteEmployee(id);
            return await Task.FromResult(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _IEmployee.CheckEmployee(id);
        }
    }
}
