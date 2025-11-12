using HRMS.DbContext;
using HRMS.DTOs.Auth;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //Dependency Injection
        private readonly HrmsContext _context;
        public AuthController(HrmsContext context)
        {
            _context = context;
        }
        [HttpPost("Login")]
       public IActionResult Login([FromBody]LoginDTO LoginDto)
        {
            try
            {

                var user = _context.Users.FirstOrDefault(u => u.Username.ToUpper()== LoginDto.Username.ToUpper());//جلب المستخدم بناء على اسم المستخدم
                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid username or password." });
                }

                if (!BCrypt.Net.BCrypt.Verify(LoginDto.Password,user.HashedPassword)) //التحقق من كلمة المرور
                {
                    return Unauthorized(new { Message = "Invalid username or password." });
                }

                var Token = GeneratJWToken(user);//انشاء التوكن 

                return Ok(new { Token = Token });//ارجاع التوكن للعميل



            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during login.", Details = ex.Message });//500 Internal Server Error
            }
        }

        private string GeneratJWToken(Models.User user)//انشاء التوكن
        {
            var Claims = new List<Claim>();
            Claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));//ادعاء بمعرف المستخدم
            Claims.Add(new Claim(ClaimTypes.Name, user.Username));//ادعاء باسم المستخدم
            if (user.IsAdmin)//اذا كان الادمن
            {
                Claims.Add(new Claim(ClaimTypes.Role,"Admin"));//الادعاء اللي بيميز الادمن
            }
            else
            {
                var employee = _context.Employees.Include(x=>x.Lookup).FirstOrDefault(e => e.UserId == user.Id);//جلب بيانات الموظف المرتبط بالمستخدم

                Claims.Add(new Claim(ClaimTypes.Role, employee.Lookup.Name));//الادعاء اللي بيميز الموظف حسب وظيفته
            }


            //Secret Key = WHAFWEI#!@S!!112312WQEQW@RWQEQW432

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432"));//المفتاح السري اللي بنستخدمه عشان نوقع التوكن
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//التوقيع الرقمي-> حتى تحمي التوكن تبعك 
            var tokenSettings = new JwtSecurityToken(
                claims: Claims,//الادعاءات اللي بدنا نحطها بالتوكن
                expires: DateTime.Now.AddDays(1),//مدة صلاحية هذه التوكن
                signingCredentials: creds//التوقيع الرقمي 
                );
            var tokenHandler = new JwtSecurityTokenHandler();//معالج التوكن
            var Token = tokenHandler.WriteToken(tokenSettings);//كتابة التوكن على شكل سترينج

            return Token;//ارجاع التوكن




        }

    }
}
