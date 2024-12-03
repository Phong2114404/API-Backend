using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IoTwithMysql.Data
{
    [Table("User")]
    [Index(nameof(UserName), IsUnique = true)]
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserPassword { get; set; }

        [Required]
        [MaxLength(100)]
        public string Fullname { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        // Navigation property: Một người dùng có nhiều lần đo
        public ICollection<Measurement> Measurements { get; set; }
    }
}
