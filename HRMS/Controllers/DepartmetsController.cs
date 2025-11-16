using HRMS.DbContext;
using HRMS.DTOs;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    [Authorize]
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
            try
            {
                var result = from Department in _context.Departments
                             from Type in _context.Lookup.Where(x => x.Id == Department.TypeId).DefaultIfEmpty() // Left Join
                             where (filterDepDto == null || Department.Name.ToUpper().Contains(filterDepDto.Name.ToUpper())) &&
                             (filterDepDto == null || Department.FloorNumber.ToString().Contains(filterDepDto.FloorNumber.ToString()))
                             orderby Department.Id descending
                             select new DepartmentsDOT
                             {
                                 Id = Department.Id,
                                 Name = Department.Name,
                                 Description = Department.Description,
                                 FloorNumber = Department.FloorNumber,
                                 TypeId = Department.TypeId,
                                 TypeName = Type.Name,

                             };


                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }
        [HttpGet("GetById/{id}")]//api/Departmets/GetById/1
        public IActionResult GetById(long id)
        {
            var result = _context.Departments.Where(x => x.Id == id).Select(x => new DepartmentsDOT
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                FloorNumber = x.FloorNumber,
                TypeId = x.TypeId,
                TypeName=x.Type.Name
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
        [Authorize(Roles ="HR,Admin")]
        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] SaveDepartmentDto departmentDto)
        {


     if (departmentDto == null || departmentDto.Name == null)
    {
        return BadRequest("Invalid department data.");
    }

    var newDepartment = new Department
    {
        Id = 0,
        Name = departmentDto.Name ?? string.Empty,
        Description = departmentDto.Description,
        FloorNumber = departmentDto.FloorNumber,
        TypeId = departmentDto.TypeId,


    };
      _context.Departments.Add(newDepartment);
      _context.SaveChanges();

            return Ok(new { message = "Department added successfully", newDepartment });
        }

        [Authorize(Roles = "HR,Admin")]
        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] SaveDepartmentDto UpdatDepDto)
        {
            var existingDepartment = _context.Departments.FirstOrDefault(d => d.Id == UpdatDepDto.Id);
            if (existingDepartment == null)
            {
                return NotFound($"Department with Id {UpdatDepDto.Id} not found.");
            }
            
            existingDepartment.Name = UpdatDepDto.Name;
            existingDepartment.Description = UpdatDepDto.Description;
            existingDepartment.FloorNumber = UpdatDepDto.FloorNumber;
            existingDepartment.TypeId = UpdatDepDto.TypeId;
            _context.SaveChanges();

            if (existingDepartment == null)
            {
                return BadRequest("Invalid department data.");
            }
            else
            {
                return Ok(new { message = "Department updated successfully", existingDepartment });
            }

        }

        [Authorize(Roles = "HR,Admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(long id)//api/Departmets/Delete/1
        {
            try//محاولة تنفيذ الكود
            {
                var department = _context.Departments.FirstOrDefault(x => x.Id == id);//جلب القسم بناء على المعرف
                if (department == null)//التحقق اذا القسم موجود
                {
                    return NotFound("Department Does Not Exist");//اذا القسم مش موجود
                }

                var isEmployee = _context.Employees.Any(x => x.DepartmentId == id);//التحقق اذا في موظفين مرتبطين بالقسم
                if (isEmployee)
                {
                    return BadRequest("Department with assigned employees cannot be deleted");//اذا في موظفين مرتبطين بالقسم
                }

                _context.Departments.Remove(department);//حذف القسم
                _context.SaveChanges();//حفظ التغييرات
                return Ok();//ارجاع استجابة ناجحة
            }
            catch (Exception ex)//التقاط اي استثناءات
            {
                return BadRequest(ex.Message);//ارجاع رسالة الخطأ
            }

        }




    }
}
