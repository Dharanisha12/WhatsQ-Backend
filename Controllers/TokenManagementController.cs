using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

[Route("api/tokenmanagement")]
[ApiController]
public class TokenManagementController : ControllerBase
{
    private readonly string _connectionString;

    public TokenManagementController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // GET: api/tokenmanagement/bydate?date=2025-04-26
    [HttpGet("bydate")]
    public async Task<IActionResult> GetTokensByDate([FromQuery] DateTime date)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = "CALL GetTokensByDate(@input_date, @input_schema);";
            var cmd = new MySqlCommand(query, connection);

            cmd.Parameters.Add(new MySqlParameter("@input_date", date.Date));
            cmd.Parameters.Add(new MySqlParameter("@input_schema", "whatsq"));
            
            var reader = await cmd.ExecuteReaderAsync();

            var tokens = new List<Token>();
            while (await reader.ReadAsync())
            {
                tokens.Add(new Token
                {
                    Id = reader.GetInt32("id"),
                    FullName = reader.GetString("full_name"),
                    Phone = reader.GetString("phone"),
                    TokenNumber = reader.GetInt32("token_number"),
                    Status = reader.GetString("status"),
                    CreatedAt = reader.GetDateTime("created_at")
                });
            }

            return Ok(tokens);
        }
    }



    // PUT: api/tokenmanagement/updatestatus
    [HttpPut("updatestatus")]
    public async Task<IActionResult> UpdateTokenStatus([FromBody] TokenStatusUpdate model)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Call stored procedure
            var query = "CALL UpdateTokenStatus(@input_schema, @token_number, @input_date, @new_status);";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@input_schema", "whatsq"); // your schema name
                cmd.Parameters.AddWithValue("@token_number", model.TokenNumber);
                cmd.Parameters.AddWithValue("@input_date", model.Date.ToString("yyyy-MM-dd")); // Date formatted as 'yyyy-MM-dd'
                cmd.Parameters.AddWithValue("@new_status", model.NewStatus);

                await cmd.ExecuteNonQueryAsync();
            }

            return Ok(new { Message = "Token status updated successfully!" });
        }
    }

}

public class TokenStatusUpdate
{
    public int TokenNumber { get; set; }
    public DateTime Date { get; set; }
    public string NewStatus { get; set; }
}

public class Token
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public int TokenNumber { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
