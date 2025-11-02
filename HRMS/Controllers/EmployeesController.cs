using HRMS.DbContext;
using HRMS.DTOs;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
      

        //Dependency Injection
       private readonly HrmsContext _context;
        public EmployeesController(HrmsContext context)
        {
            _context = context;
        }


        [HttpGet("GetByCraterya")]
        public IActionResult GetByCraterya([FromQuery]SearchEmployeeDTO searchEmpDto)
        {
            var result = from Employees in _context.Employees
                         from Departments in _context.Departments.Where(x => x.Id == Employees.DepartmentId).DefaultIfEmpty() // Left Join
                         from Managers in _context.Employees.Where(x => x.Id == Employees.ManagerId).DefaultIfEmpty() // Left Join
                         where (searchEmpDto == null || Employees.Position.ToUpper().Contains(searchEmpDto.Position.ToUpper()))&&
                         (searchEmpDto==null ||Employees.FName.ToUpper().Contains(searchEmpDto.Name.ToUpper())) 
                         orderby Employees.Id descending
                         select new EmployeeDTO
                         { Id = Employees.Id,
                             Name = Employees.FName + " " + Employees.LName,
                             Email = Employees.Email,
                             BirthDate = Employees.BirthDate,
                             Position = Employees.Position,
                             Salary= Employees.Salary,
                             DepartmentId= Employees.DepartmentId,
                             DepartmentName=Departments.Name,
                             ManagerId= Employees.ManagerId,
                             ManagerName= Managers.FName ,


                         };
            return Ok(result);

        }

        [HttpGet("GetById")]
        public IActionResult GetById(long id)
        {
            var result = _context.Employees.Select(x => new EmployeeDTO
            {
                Id = x.Id,
                Name = x.FName + " " + x.LName,
                Email = x.Email,
                BirthDate = x.BirthDate,
                Position = x.Position,
                Salary= x.Salary,
                DepartmentId= x.DepartmentId,
               // DepartmentName="", // To be implemented
                ManagerId = x.ManagerId,
                //ManagerName = "" // To be implemented

            }).FirstOrDefault(e => e.Id == id);

            if (result == null)
            {
                return NotFound(new { Message = "Employee Not Found" });
            }
            return Ok(new { result });
        }


        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee([FromBody]SaveEmployeeDTO emp)
        {
            var newEmp = new Employees
            {
                Id = (_context.Employees.LastOrDefault()?.Id ?? 0) + 1,
                FName = emp.FName,
                LName = emp.LName,
                Email = emp.Email,
                BirthDate = emp.BirthDate,
                Position = emp.Position,
                Salary = emp.Salary,
                DepartmentId = emp.DepartmentId,
                ManagerId = emp.ManagerId,
            };
            _context.Employees.Add(newEmp);
            _context.SaveChanges();// Commit changes to the database// //  عشان لما يعمل سيف في الداتابيز بس يروح مرة مش مرتين,فبتخفف اللود على النظام//

            return Ok(new { Message = "Employee Added Successfully" });
        }

        
        [HttpPut("Update")]
        public IActionResult Update([FromBody]SaveEmployeeDTO emp)
        {
            var existingEmp = _context.Employees.FirstOrDefault(e => e.Id == emp.Id);
            if (existingEmp == null)
            {
                return NotFound(new { Message = "Employee Not Found" });
            }
            existingEmp.FName = emp.FName;
            existingEmp.LName = emp.LName;
            existingEmp.Email = emp.Email;
            existingEmp.BirthDate = emp.BirthDate;
            existingEmp.Position = emp.Position;
            existingEmp.Salary = emp.Salary;
            existingEmp.DepartmentId = emp.DepartmentId;
            existingEmp.ManagerId = emp.ManagerId;
            _context.SaveChanges();
            return Ok(new { Message = "Employee Updated Successfully" });

        }




        [HttpDelete("Delete/{id}")]//Route Parameter
        public IActionResult Delete(long id)
        {
            var existingEmp = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (existingEmp == null)
            {
                return NotFound(new { Message = "Employee Not Found" });//404 => Not Found
            }
            _context.Employees.Remove(existingEmp);
            _context.SaveChanges();
            return Ok(new { Message = "Employee Deleted Successfully" });
        }







    }

}

