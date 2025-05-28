using System.Threading.Tasks;
using WhatsQ.Models;

namespace WhatsQ.Repositories
{
    public interface IPatientStatusRepository
    {
        // Task<PatientStatusModel> GetPatientStats();
        Task<List<PatientStatusModel>> GetPatientStats();
        Task<List<PatientStatusModel>> GetPatientStatsForToday();
    }
}
