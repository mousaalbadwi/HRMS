using HRMS.DbContext;
using HRMS.DTOs;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace HRMS.Controllers
{
    [Authorize]
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
        public IActionResult GetByCraterya([FromQuery ]SearchEmployeeDTO searchEmpDto)
        {
            try
            {
                var role= User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                var UserId= User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;


                var result = from Employees in _context.Employees
                             from Departments in _context.Departments.Where(x => x.Id == Employees.DepartmentId).DefaultIfEmpty() // Left Join
                             from Managers in _context.Employees.Where(x => x.Id == Employees.ManagerId).DefaultIfEmpty() // Left Join
                             from Positions in _context.Lookup.Where(x => x.Id == Employees.PositionId)
                             where (searchEmpDto == null || Employees.PositionId == searchEmpDto.PositionId) &&
                             (searchEmpDto == null || Employees.FName.ToUpper().Contains(searchEmpDto.Name.ToUpper()))
                             orderby Employees.Id descending
                             select new EmployeeDTO
                             {
                                 Id = Employees.Id,
                                 Name = Employees.FName + " " + Employees.LName,
                                 Email = Employees.Email,
                                 BirthDate = Employees.BirthDate,
                                 PositionId = Employees.PositionId,

                                 Salary = Employees.Salary,
                                 DepartmentId = Employees.DepartmentId,
                                 DepartmentName = Departments.Name,
                                 ManagerId = Employees.ManagerId,
                                 ManagerName = Managers.FName,
                                 UserId= Employees.UserId,


                             };
                if(role?.ToUpper()!="ADMIN" && role?.ToUpper() !="HR")
                    {
                    result= result.Where(e => e.UserId ==long.Parse(UserId));
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving employees.", Details = ex.Message });
            }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(long id)
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;//جلب دور المستخدم الحالي
            var UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;// جلب معرف المستخدم الحالي

            try
            {
                var result = _context.Employees.Select(x => new EmployeeDTO
                {
                    Id = x.Id,
                    Name = x.FName + " " + x.LName,
                    Email = x.Email,
                    BirthDate = x.BirthDate,
                    PositionId = x.PositionId,
                    PositionName = x.Lookup.Name,
                    Salary = x.Salary,
                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.Department.Name,
                    ManagerId = x.ManagerId,
                    ManagerName = x.Manager.FName,
                    UserId = x.UserId,


                }).FirstOrDefault(e => e.Id == id);

                if (role?.ToUpper() != "ADMIN" && role?.ToUpper() != "HR")//اذا المستخدم مش ادمن ولا اتش ار
                {
                    if (result.UserId != long.Parse(UserId))//اذا المستخدم اللي طالب البيانات مش هو نفسو المستخدم اللي مربوط بالموظف
                    {
                        return Forbid();//403
                    }
                }


                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the employee.", Details = ex.Message });
            }
        }

        [Authorize(Roles = "HR,Admin")]
        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee([FromBody] SaveEmployeeDTO emp)
        {
            try
            {
                var user = new User//انشاء مستخدم جديد
                {
                    Id = 0,
                    Username =$"{emp.FName}_{emp.LName}_Hrms",//انشاء اسم مستخدم تلقائي
                    HashedPassword =BCrypt.Net.BCrypt.HashPassword($"{emp.FName}@123"),//تشفير كلمة المرور
                    IsAdmin = false//ليس ادمن
                };
                var IsUserExist = _context.Users.Any(u => u.Username.ToUpper() == user.Username.ToUpper());
                //Any اذا بدك تتاكد من ان الاسم الرح يدخل المستخدم مش موجود  في قعدة البيانات منستخدم 
                //لانه انا بس بدي يرجع يا صح يا خطا(يا موجود او لا)يعني بوليان
                if (IsUserExist)//اذا الاسم موجود
                {
                    return Conflict(new { Message = "Username Already Exists" });//409
                }
                _context.Users.Add(user);//اضف المستخدم الجديد لقعدة البيانات

                var newEmp = new Employees
                {
                    Id = (_context.Employees.LastOrDefault()?.Id ?? 0) + 1,
                    FName = emp.FName,
                    LName = emp.LName,
                    Email = emp.Email,
                    BirthDate = emp.BirthDate,
                    PositionId = emp.PositionId,
                    Salary = emp.Salary,
                    DepartmentId = emp.DepartmentId,
                    ManagerId = emp.ManagerId,
                   // UserId= user.Id,// ربط الموظف بالمستخدم الذي تم إنشاؤه
                    User = user // ربطناه عن طريق النافيجايشن بروبرتي 
                    //لما بضيف كائن مستخدم جديد للموظف,EF Core رح يتعامل مع العلاقة بين الكائنين ويعين معرف المستخدم تلقائياً بعد حفظ التغييرات
                };
                _context.Employees.Add(newEmp);
                _context.SaveChanges();// Commit changes to the database// //  عشان لما يعمل سيف في الداتابيز بس يروح مرة مش مرتين,فبتخفف اللود على النظام//

                return Ok(new { Message = "Employee Added Successfully" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding the employee.", Details = ex.Message });
            }
        }
        [Authorize(Roles = "HR,Admin")]
        [HttpPut("Update")]
        public IActionResult Update([FromBody] SaveEmployeeDTO emp)
        {

            try
            {
                var existingEmp = _context.Employees.FirstOrDefault(e => e.Id == emp.Id);

           
                if (existingEmp == null)
                {
                    return NotFound(new { Message = "Employee Not Found" });
                }
                else
                {
                    existingEmp.FName = emp.FName;
                    existingEmp.LName = emp.LName;
                    existingEmp.Email = emp.Email;
                    existingEmp.BirthDate = emp.BirthDate;
                    existingEmp.PositionId = emp.PositionId;
                    existingEmp.Salary = emp.Salary;
                    existingEmp.DepartmentId = emp.DepartmentId;
                    existingEmp.ManagerId = emp.ManagerId;
                    _context.SaveChanges();
                    return Ok(new { Message = "Employee Updated Successfully" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the employee.", Details = ex.Message });

            }

        }

        [Authorize(Roles = "HR,Admin")]
        [HttpDelete("Delete/{id}")]//Route Parameter
        public IActionResult Delete(long id)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the employee.", Details = ex.Message });

            }

        }





    }

}

