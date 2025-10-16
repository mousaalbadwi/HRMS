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
        public static List<Employees> employees = new List<Employees>
        {
           new Employees {Id=1, FName = "Mousa ",LName="Alabdwi",Email="Mousa@gmail.com" ,Position="Developer",BirthDate=new DateTime(2003,8,18) },
           new Employees {Id=2, FName = "Raneem", LName="Asaad",Email="Raneem@gmail.com",Position = "Manager" ,BirthDate=new DateTime(2003,3,23) },
           new Employees { Id=3,FName = "Ahmad",LName="Masagdeh" ,Email="Ahmad@gmail.com", Position = "Salers",BirthDate=new DateTime(1995,6,19) },
           new Employees {Id=1, FName = "Rania ",LName="Hasan",Email="Rania@gmail.com" ,Position="Qualty Assurunce",BirthDate=new DateTime(2004,10,8) },
           new Employees {Id=2, FName = "Mohammed", LName="Saleem",Email="Mohammed@gmail.com",Position = "Technecal suport" ,BirthDate=new DateTime(2003,8,18) },
           new Employees { Id=3,FName = "Shahed",LName="Bdier" ,Email="Shahed@gmail.com", Position = "Bissnis Analysis",BirthDate=new DateTime(1999,7,11) }


        };

        [HttpGet("GetByCraterya")]
        public IActionResult GetByCraterya([FromQuery]SearchEmployeeDTO searchEmpDto)
        {
            var result = from Employees in employees
                         where (searchEmpDto == null || Employees.Position.ToUpper().Contains(searchEmpDto.Position.ToUpper()))&&
                         (searchEmpDto==null ||Employees.FName.ToUpper().Contains(searchEmpDto.Name.ToUpper())) 
                         orderby Employees.Id descending
                         select new EmployeeDTO
                         { Id = Employees.Id,
                             Name = Employees.FName + " " + Employees.LName,
                             Email = Employees.Email,
                             BirthDate = Employees.BirthDate,

                             Position = Employees.Position
                         };
            return Ok(result);

        }

        [HttpGet("GetById")]
        public IActionResult GetById(long id)
        {
            var result = employees.Select(x => new EmployeeDTO
            {
                Id = x.Id,
                Name = x.FName + " " + x.LName,
                Email = x.Email,
                BirthDate = x.BirthDate,
                Position = x.Position
            });


            return Ok(new { result });
        }
        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee([FromBody]SaveEmployeeDTO emp)
        {
            var newEmp = new Employees
            {
                Id = (employees.LastOrDefault()?.Id ?? 0) + 1,
                FName = emp.FName,
                LName = emp.LName,
                Email = emp.Email,
                BirthDate = emp.BirthDate,
                Position = emp.Position
            };
            employees.Add(newEmp);

            return Ok(new { Message = "Employee Added Successfully" });
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody]SaveEmployeeDTO emp)
        {
            var existingEmp = employees.FirstOrDefault(e => e.Id == emp.Id);
            if (existingEmp == null)
            {
                return NotFound(new { Message = "Employee Not Found" });
            }
            existingEmp.FName = emp.FName;
            existingEmp.LName = emp.LName;
            existingEmp.Email = emp.Email;
            existingEmp.BirthDate = emp.BirthDate;
            existingEmp.Position = emp.Position;
            return Ok(new { Message = "Employee Updated Successfully" });
        }

        [HttpDelete("Delete/{id}")]//Route Parameter
        public IActionResult Delete(long id)
        {
            var existingEmp = employees.FirstOrDefault(e => e.Id == id);
            if (existingEmp == null)
            {
                return NotFound(new { Message = "Employee Not Found" });//404 => Not Found
            }
            employees.Remove(existingEmp);
            return Ok(new { Message = "Employee Deleted Successfully" });
        }







    }

}

