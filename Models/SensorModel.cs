using System.ComponentModel.DataAnnotations;

namespace IoTwithMysql.Models
{
    public class SensorModel
    {
        // SpO2, Temp, Heart Rate, Weight.
        [Required]
        public string SpO2 { get; set; }

        [Required]
        public string Temp { get; set; }

        [Required]
        public string HeartRate { get; set; }

        [Required]
        public string Weight { get; set; }





    }
}
