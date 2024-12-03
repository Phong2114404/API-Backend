using System.ComponentModel.DataAnnotations;

namespace IoTwithMysql.Models
{
    public class ChangePasswordRequestModel
    {
        [Required]
        public string OldPassword { get; set; } // Mật khẩu cũ

        [Required]
        public string NewPassword { get; set; } // Mật khẩu mới
    }
}
