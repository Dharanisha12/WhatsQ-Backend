using System.Collections.Generic;
using System.Threading.Tasks;
using WhatsQ.Models;

namespace WhatsQ.Providers
{
    public interface IPatientStatusProvider
    {
        Task<List<PatientStatusModel>> GetPatientStatus(); 
      
        Task<List<PatientStatusModel>> GetPatientStatusForToday(); 
    }
}
