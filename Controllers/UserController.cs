using System;
using Ecomm.API.DataAccess;
using Ecomm.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

namespace Ecomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService dataAccess;
        private readonly string DateFormat;
        public UserController(IUserService dataAccess, IConfiguration configuration)
        {
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }

        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(User user)
        {
            user.CreatedAt = DateTime.Now.ToString(DateFormat);
            user.ModifiedAt = DateTime.Now.ToString(DateFormat);

            var result = dataAccess.InsertUser(user);
            return Ok(result? "inserted" : "insert fail");
        }

        [HttpPost("LoginUser")]
        public IActionResult LoginUser(User user)
        {
            var token = dataAccess.IsUserPresent(user.Email, user.Password);
            if (token == "") token = "invalid";
            return Ok(token);
        }
        [HttpGet("GetUser/{id}")]
        public IActionResult GetUser(int id)
        {
            var result = dataAccess.GetUser(id);
            return Ok(result);
        }
        [HttpGet("GetUsers")]
        public IActionResult GetAllUser()
        {
            var result = dataAccess.GetAllUser();
            return Ok(result);
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = dataAccess.Delete(id);
            return Ok(result?"deleted" : "delete fail");
        }

        [HttpPut("Update")]
        public IActionResult Update(User user)
        {
            var result = dataAccess.Update(user);
            return Ok(result ? "updated" : "update fail");
        }
    }
}

