using HRMS.DTOs;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmetsController : ControllerBase
    {
        public static List<Department> department = new List<Department>
        {
           new Department {Id=1, Name = "IT",Description="Information Technology",FloorNumber=5 },
           new Department {Id=2, Name = "HR",Description="Human Resource",FloorNumber=3 },
           new Department {Id=3, Name = "Finance",Description="Finance Department",FloorNumber=4 },

        };


        [HttpGet("GetByCraterya")]
        public IActionResult GetByCraterya([FromQuery] FilterDepartmentsDTO filterDepDto)
        {
            var result = from Department in department
                         where (filterDepDto == null || Department.Name.ToUpper().Contains(filterDepDto.Name.ToUpper())) &&
                         (filterDepDto == null || Department.FloorNumber.ToString().Contains(filterDepDto.FloorNumber.ToString()))
                         orderby Department.Id descending
                         select new DepartmentsDOT
                         {
                             Id = Department.Id,
                             Name = Department.Name,
                             Description = Department.Description,
                             FloorNumber = Department.FloorNumber
                         };


            return Ok(new { result });
        }

        [HttpGet("GetById/{id}")]//api/Departmets/GetById/1
        public IActionResult GetById(long id)
        {
            var result = department.Where(x => x.Id == id).Select(x => new DepartmentsDOT
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                FloorNumber = x.FloorNumber
            });
            if (!result.Any())
            {
                return NotFound($"Department with Id {id} not found.");
            }
            else
            {
                return Ok(new { result });
            }
        }

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] DepartmentsDOT departmentDto)
        {
            var newDepartment = new Department
            {
                Id = departmentDto.Id,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                FloorNumber = departmentDto.FloorNumber
            };
            department.Add(newDepartment);

            if (newDepartment == null)
            {
                return BadRequest("Invalid department data.");
            }
            else
            {
                return Ok(new { message = "Department added successfully", newDepartment });

            }

        }
        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] DepartmentsDOT UpdatDepDto)
        {
            var existingDepartment = department.FirstOrDefault(d => d.Id == UpdatDepDto.Id);
            if (existingDepartment == null)
            {
                return NotFound($"Department with Id {UpdatDepDto.Id} not found.");
            }
            
            existingDepartment.Name = UpdatDepDto.Name;
            existingDepartment.Description = UpdatDepDto.Description;
            existingDepartment.FloorNumber = UpdatDepDto.FloorNumber;

            if (existingDepartment == null)
            {
                return BadRequest("Invalid department data.");
            }
            else
            {
                return Ok(new { message = "Department updated successfully", existingDepartment });
            }

        }
        [HttpDelete("Delete/{id}")]
       public IActionResult Delete(long id)
        {
            var existingDepartment = department.FirstOrDefault(d => d.Id == id);
            if (existingDepartment == null)
            {
                return NotFound($"Department with Id {id} not found.");
            }
            department.Remove(existingDepartment);
            return Ok(new { message = "Department deleted successfully", existingDepartment });

        }




    }
}
