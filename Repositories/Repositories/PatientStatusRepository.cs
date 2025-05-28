using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhatsQ.Models;
using WhatsQ.Repositories;

namespace WhatsQ.Repositories
{
    public class PatientStatusRepository : IPatientStatusRepository
    {
        private readonly string _connectionString;

        public PatientStatusRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public Task<List<PatientStatusModel>> GetPatientStats()
        {
            throw new NotImplementedException();
        }

        public async Task<List<PatientStatusModel>> GetPatientStatsForToday()
        {
            List<PatientStatusModel> statusList = new List<PatientStatusModel>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Modify the query to filter for today's records
                    string query = @"
                        SELECT 
                            COUNT(*) AS total_patients,
                            SUM(CASE WHEN status = 'waiting' THEN 1 ELSE 0 END) AS waiting_count,
                            SUM(CASE WHEN status = 'in-consultancy' THEN 1 ELSE 0 END) AS in_consultation_count,
                            SUM(CASE WHEN status = 'completed' THEN 1 ELSE 0 END) AS completed_count,
                            SUM(CASE WHEN status = 'cancelled' THEN 1 ELSE 0 END) AS cancelled_count
                        FROM whatsQ.tokens
                        WHERE DATE(created_at) = CURDATE()";  // Filtering data for today (replace 'date_column' with your actual column name)

                    using (MySqlCommand mysqlCommand = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = (MySqlDataReader)await mysqlCommand.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var status = new PatientStatusModel
                                {
                                    total_patients = reader["total_patients"] is DBNull ? 0 : Convert.ToInt32(reader["total_patients"]),
                                    waiting_count = reader["waiting_count"] is DBNull ? 0 : Convert.ToInt32(reader["waiting_count"]),
                                    in_consultation_count = reader["in_consultation_count"] is DBNull ? 0 : Convert.ToInt32(reader["in_consultation_count"]),
                                    completed_count = reader["completed_count"] is DBNull ? 0 : Convert.ToInt32(reader["completed_count"]),
                                    cancelled_count = reader["cancelled_count"] is DBNull ? 0 : Convert.ToInt32(reader["cancelled_count"])
                                };

                                statusList.Add(status);
                            }
                        }
                    }
                }

                return statusList;
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                throw;
            }
        }
    }
}
