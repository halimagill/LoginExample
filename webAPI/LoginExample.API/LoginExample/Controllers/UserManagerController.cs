using System.Net.Mime;
using LoginExample.BSN.Interfaces;
using LoginExample.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static FastExpressionCompiler.ExpressionCompiler;

// TODO: SETUP the Logger, Login, Logout, install cert on in store

namespace LoginExample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private IAuthService _authService;
        private IUserManagerService _userManagerService;
        public UserManagerController(IAuthService authService, IUserManagerService usrMgr)
        {
            _authService = authService;
            _userManagerService = usrMgr;
        }

        // GET: api/<UserManagerController>
        [HttpGet]
        [Route("Users")]
        public async Task<IEnumerable<User>?> Get()
        {
            return await _userManagerService.GetUsers();
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

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var token = await _authService.Login(loginRequest.email, loginRequest.password);

                if (token != null)
                {
                    return Ok(new { token });
                }

                return Problem(
                    type: "Bad Request",
                    title: "Login was unsuccessful",
                    detail: token,
                    statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(
                    type: "Bad Request",
                    title: "Identity failure",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status400BadRequest);
            }            
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult> Logout(User user)
        {
            //TODO: Build out further and add session handling
            //Remove the token
            _authService.LogOut();

            return Ok();
        }

        // PUT api/<UserManagerController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(Guid id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            
            var updatedUser = await _userManagerService.UpdateUser(user);

            if (updatedUser is null) 
            { 
                return NotFound();
            }

            return Ok(updatedUser);
        }

        // DELETE api/<UserManagerController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
