using System;

namespace WhatsQ.Models
{
    public class PatientStatusModel
    {
        public int total_patients { get; set; }
        public int waiting_count { get; set; }
        public int in_consultation_count { get; set; }
        public int completed_count { get; set; }
        
        public int cancelled_count { get; set; }
    }
}
