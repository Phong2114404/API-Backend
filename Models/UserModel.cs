using System.ComponentModel.DataAnnotations;

namespace IoTwithMysql.Models
{
    public class UserRequest
    {
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
    }

}
