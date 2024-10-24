using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopNest.Services.User.Model;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ShopNest.Services.User.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetUserDetails")]
        public async Task<ActionResult<IEnumerable<UserData>>> GetUserDetails()
        {
            var userDetails = await _context.UserData.FromSqlRaw("EXEC GetUserDetails").ToListAsync();

            if (userDetails == null || !userDetails.Any())
            {
                return NotFound("No user details found.");
            }

            // Optional: Normalize the data by replacing nulls
            foreach (var user in userDetails)
            {
                user.FirstName = user.FirstName ?? "N/A"; // Replace null with "N/A"
                user.LastName = user.LastName ?? "N/A"; // Replace null with "N/A"
                user.EmailOrPhone = user.EmailOrPhone ?? "N/A"; // Replace null with "N/A"
                //user.PhoneNumber = user.PhoneNumber ?? "N/A"; // Replace null with "N/A"
                user.Password = user.Password ?? "N/A"; // Replace null with "N/A" (consider security implications)
            }

            return Ok(userDetails);
        }


        [HttpPost("SaveUserDetails")]
        public async Task<ActionResult<SaveResponse>> SaveUserDetails([FromBody] UserDetails userDetails)
        {
            try
            {
                if (userDetails == null)
                {
                    return BadRequest("User data is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if EmailOrPhone field is filled out
                if (string.IsNullOrEmpty(userDetails.EmailOrPhone))
                {
                    return BadRequest(new { message = "Email or Phone is required." });
                }

                // Validate EmailOrPhone
                //string contactType;
                //try
                //{
                //    contactType = userDetails.IdentifyContactType(); // Validate and identify contact type
                //}
                //catch (ValidationException ex)
                //{
                //    return BadRequest(new { message = ex.Message });
                //}

                SaveResponse response = new SaveResponse();

                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("SaveUserDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Adding parameters
                        command.Parameters.AddWithValue("@FirstName", userDetails.FirstName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@LastName", userDetails.LastName ?? (object)DBNull.Value);

                        // Save EmailOrPhone based on its type
                        command.Parameters.AddWithValue("@EmailOrPhone", userDetails.EmailOrPhone ?? (object)DBNull.Value);

                        command.Parameters.AddWithValue("@Password", userDetails.Password ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ModifiedUser", 1); // Use your logic for the ModifiedUser

                        // Output parameter
                        var responseParam = new SqlParameter("@ResponseMsg", SqlDbType.VarChar, 200)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(responseParam);

                        await command.ExecuteNonQueryAsync();

                        // Evaluate response message
                        string responseMsg = responseParam.Value.ToString();
                        response.Saved = responseMsg == "Y";

                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [Authorize]
        [HttpGet("protected")]
        public IActionResult GetProtectedData()
        {
            return Ok("This is a protected route.");
        }
    }
}
