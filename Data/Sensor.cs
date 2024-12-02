using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IoTwithMysql.Data
{
    [Table("Sensor")]
    public class Sensor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Khóa chính tự động tăng
        public int Version { get; set; } // Phiên bản của bộ sensor

        [Required]
        public string SpO2 { get; set; } // Phiên bản của sensor SpO2

        [Required]
        public string Temp { get; set; } // Phiên bản của sensor nhiệt độ

        [Required]
        public string HeartRate { get; set; } // Phiên bản của sensor nhịp tim

        [Required]
        public string Weight { get; set; } // Phiên bản của sensor cân

        [Required]
        public DateTime Time { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<Measurement> Measurements { get; set; }
    }
}
