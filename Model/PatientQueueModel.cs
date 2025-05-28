using System;

namespace WhatsQ.Model
{
    public class PatientQueueModel
    {

        public int id { get; set; }
        public required string patientname { get; set; }
        public int tokennumber { get; set; }
        public required string estimatedtime { get; set; }
        public required string status { get; set; } 
        public DateTime visitdate { get; set; }

        public string whatsappnumber { get; set; }

    }
}
