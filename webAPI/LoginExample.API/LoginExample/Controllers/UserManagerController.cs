using System.Net.Mime;
using LoginExample.BSN.Interfaces;
using LoginExample.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// TODO: SETUP the Logger, Login, Logout, install cert on in store

namespace LoginExample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private IUserManagerService _userManagerService;
        public UserManagerController(IUserManagerService usrMgr)
        {
            _userManagerService = usrMgr;
        }

        // GET: api/<UserManagerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserManagerController>/5
        [HttpGet("{email}")]
        public async Task<AppUser?> GetUserByEmail(string email)
        {
            return await _userManagerService.FindUserByEmail(email);
        }

        // POST api/<UserManagerController>
        [HttpPost]
        [Route("CreateUser")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(User user)
        {            
            IdentityResult result = await _userManagerService.CreateUser(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(CreateUser), result.Succeeded);
        }

        // PUT api/<UserManagerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserManagerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
