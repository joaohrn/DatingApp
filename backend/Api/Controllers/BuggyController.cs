using Api.Data;
using Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{    public class BuggyController(DataContext context) : BaseApiController
    {
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> getAuth(){
            return "secret text";
        }
        [HttpGet("not-found")]
        public ActionResult<AppUser> getNotFound(){
            var thing = context.Users.Find(-1);
            if(thing == null) return NotFound();
            return thing;
        }
        [HttpGet("server-error")]
        public ActionResult<AppUser> getServerError(){
            var thing = context.Users.Find(-1) ?? throw new Exception("A bad thing has happened");
            return thing;
        }
        [HttpGet("bad-request")]
        public ActionResult<string> getBadRequest(){
            return BadRequest("This was not a good request.");
        }
    }
}
