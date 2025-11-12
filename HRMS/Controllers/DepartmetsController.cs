using HRMS.DbContext;
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

        //Dependency Injection
        private readonly HrmsContext _context;
        public DepartmetsController(HrmsContext context)
        {
            _context = context;
        }
        

        [HttpGet("GetByCraterya")]
        public IActionResult GetByCraterya([FromQuery] FilterDepartmentsDTO filterDepDto)
        {
            var result = from Department in _context.Departments
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
            var result = _context.Departments.Where(x => x.Id == id).Select(x => new DepartmentsDOT
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
            if (departmentDto == null || departmentDto.Name == null)
    {
        return BadRequest("Invalid department data.");
    }

    var newDepartment = new Department
    {
        Id = departmentDto.Id,
        Name = departmentDto.Name ?? string.Empty,
        Description = departmentDto.Description,
        FloorNumber = departmentDto.FloorNumber
        
    };
    _context.Departments.Add(newDepartment);

    return Ok(new { message = "Department added successfully", newDepartment });
        }
        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] DepartmentsDOT UpdatDepDto)
        {
            var existingDepartment = _context.Departments.FirstOrDefault(d => d.Id == UpdatDepDto.Id);
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
            var existingDepartment = _context.Departments.FirstOrDefault(d => d.Id == id);
            if (existingDepartment == null)
            {
                return NotFound($"Department with Id {id} not found.");
            }
            _context.Departments.Remove(existingDepartment);
            return Ok(new { message = "Department deleted successfully", existingDepartment });

        }




    }
}
