using System.Collections.Generic;
using System.Threading.Tasks;
using WhatsQ.Models;
using WhatsQ.Repositories;

namespace WhatsQ.Providers
{
    public class PatientStatusProvider : IPatientStatusProvider
    {
        private readonly IPatientStatusRepository _repository;

        public PatientStatusProvider(IPatientStatusRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PatientStatusModel>> GetPatientStatus() // 🔹 Return List<PatientStatusModel>
        {
            var result = await _repository.GetPatientStats();
            return result ?? new List<PatientStatusModel>(); // ✅ Prevent null reference
        }
        public async Task<List<PatientStatusModel>> GetPatientStatusForToday()
        {
            return await _repository.GetPatientStatsForToday(); // 🔴 add this method
        }
    }
}
