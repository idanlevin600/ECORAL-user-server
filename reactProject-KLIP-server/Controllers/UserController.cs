using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reactProject_KLIP_server.Models;

namespace reactProject_KLIP_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> GetAllUsers(){
            try
            {
                return Ok(Models.User.getAllUsers()); // to fill, i think it should be a call to the dbservices
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        [HttpGet("email")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> GetUser(string email)
        {
            try
            {
                User user = Models.User.getUserByEmail(email);
                return Ok(user); 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("byId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> GetUser(int id)
        {
            try
            {
                User user = Models.User.getUserById(id);
                return Ok(user); 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostRegisteration([FromBody] User user){
            try
            {
                if (user == null)
                    return BadRequest(user);
                
                if (user.Registration())
                    return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
                else
                    return BadRequest(user);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> PostLogin(string email, [FromBody] string password)
        {
            try
            {
                if (email == null || password == null)
                    return BadRequest();

                User u = Models.User.logIn(email, password);
                if (u != null)
                    return Ok(u);
                else
                    return BadRequest(u);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("{email}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(string email)
        {
            try
            {
                if(email == null)
                {
                    return BadRequest();
                }
                User user = Models.User.getUserByEmail(email);
                if (user == null)
                {
                    return NotFound($"student with email={email} was not found to delete!");
                }
                
                Models.User.deleteUser(user);
                return NoContent();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{email}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string email, [FromBody] string password)
        {
            try
            {
                if (email == null)
                {
                    return BadRequest();
                }
                User user = Models.User.getUserByEmail(email);
                if (user == null)
                {
                    return NotFound($"student with email={email} was not found to delete!");
                }

                Models.User.updateUserPassword(email, password);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
