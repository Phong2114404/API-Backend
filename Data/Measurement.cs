using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTwithMysql.Data
{
    [Table("Measurement")]
    public class Measurement
    {
        [Key] // Khóa chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeasurementID { get; set; }

        [Required]
        public Guid UserID { get; set; } // Khóa ngoại tham chiếu đến User.UserID

        [ForeignKey(nameof(UserID))] // Ràng buộc khóa ngoại
        public User User { get; set; } // Navigation property để liên kết với User

        [Required]
        public DateTime MeasurementTime { get; set; } // Thời điểm đo mặc định là hiện tại

        // SpO2, Temp, Heart Rate, Weight.
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

        [ForeignKey(nameof(Version))] // Ràng buộc khóa ngoại
        public Sensor Sensor { get; set; } // Navigation property để liên kết với User
    }
}
