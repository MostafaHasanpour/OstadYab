using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OstadYab.Models;

namespace OstadYab.Controllers
{
    [Route("api/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly OstadyabContext _ostadyabContext;

        public DefaultController(OstadyabContext ostadyabContext)
        {
            _ostadyabContext = ostadyabContext;
        }

        [HttpGet("Users")]
        public ActionResult<object> GetUsers()
        {
            try
            {
                var users = _ostadyabContext.Users.Select(x => new
                {
                    ID = x.Id,
                    x.Name,
                    x.Fname,
                    x.Username,
                    Age = 0,
                    x.PhoneNumber,
                    Field = "",
                    ExpID = x.ExpId,
                    Photo = x.Photo,
                    Resume = x.Details
                }).ToList();
                return Ok(users);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpGet("login")]
        public ActionResult<object> LoginUser(LoginDto login)
        {
            try
            {
                var user = _ostadyabContext.Login.FirstOrDefault(x => x.UserName.ToLower() == login.Username.ToLower() && x.Password == x.Password);
                return Ok(new { user.Id, user.UserName });
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpGet("experiences")]
        public ActionResult<object> GetExperience()
        {
            try
            {
                var exps = _ostadyabContext.Exp.ToList();
                return Ok(exps);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpGet("experience/{userId}")]
        public ActionResult<object> GetExperience(int userId)
        {
            try
            {
                var exp = _ostadyabContext.Exp.Find(_ostadyabContext.Users.FirstOrDefault(x => x.Id == userId).ExpId);
                return Ok(exp);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpPost("profile")]
        public ActionResult<object> Profile(Users user)
        {
            try
            {
                if (user.Id == 0 || user.Id == null)
                    _ostadyabContext.Users.Add(user);
                else
                    _ostadyabContext.Users.Update(user);
                _ostadyabContext.SaveChanges();
                return Ok(new { result = true });
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}