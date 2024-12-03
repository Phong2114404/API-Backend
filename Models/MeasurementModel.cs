using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IoTwithMysql.Models
{
    public class MeasurementModel
    {
        [Required] 
        public decimal SpO2 { get; set; }

        [Required] 
        public decimal Temp { get; set; }

        [Required] 
        public decimal HeartRate { get; set; }

        [Required]  
        public decimal Weight { get; set; }

        [Required]
        public int Version { get; set; }
    }
}
