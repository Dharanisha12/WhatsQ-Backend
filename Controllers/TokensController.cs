using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace WhatsQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokensController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Tokens/{tokenNumber}/{date}
        [HttpGet("{tokenNumber}/{date}")]
        public IActionResult GetTokenDetails(int tokenNumber, string date)
        {
            string query = @"SELECT full_name, phone, token_number, status, description, created_at 
                             FROM tokens 
                             WHERE token_number = @tokenNumber AND DATE(created_at) = @date";

            using MySqlConnection con = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@tokenNumber", tokenNumber);
            cmd.Parameters.AddWithValue("@date", date); 

            con.Open();
            MySqlDataReader mySqlDataReader = cmd.ExecuteReader();
            using MySqlDataReader reader = mySqlDataReader;
            if (reader.Read())
            {
                var token = new
                {
                    full_name = reader["full_name"].ToString(),
                    phone = reader["phone"].ToString(),
                    token_number = Convert.ToInt32(reader["token_number"]),
                    status = reader["status"].ToString(),
                    description = reader["description"]?.ToString(),
                    created_at = reader["created_at"].ToString()
                };
                return Ok(token);
            }

            return NotFound("Token not found for the given date");
        }

        // POST: api/Tokens/UpdateDescription
        [HttpPost("UpdateDescription")]
        public IActionResult UpdateDescription([FromBody] TokenDescriptionUpdateModel model)
        {
            string query = @"UPDATE tokens 
                             SET description = @description 
                             WHERE token_number = @tokenNumber AND DATE(created_at) = @date";

            using MySqlConnection con = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using MySqlCommand cmd = new MySqlCommand(query, con);

            cmd.Parameters.AddWithValue("@description", model.description);
            cmd.Parameters.AddWithValue("@tokenNumber", model.token_number);
            cmd.Parameters.AddWithValue("@date", model.date); 

            con.Open();
            int result = cmd.ExecuteNonQuery();

            if (result > 0)
                return Ok("Description updated successfully for the given date");
            else
                return NotFound("Token not found for the given date");
        }

        public class TokenDescriptionUpdateModel
        {
            public int token_number { get; set; }
            public string description { get; set; }
            public string date { get; set; } 
        }
    }
}
