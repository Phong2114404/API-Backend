using IoTwithMysql.Data;
using IoTwithMysql.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTwithMysql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDBcontext _context;

        public UserController(MyDBcontext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet("GetAllUsers")]  // Lấy List UserName của người dùng để kiểm tra khi đăng nhập hoặc đăng kí
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Lấy danh sách UserName từ database
                var userNames = await _context.Users
                    .Select(u => u.UserName) // Chỉ chọn thuộc tính UserName
                    .ToListAsync();

                return Ok(userNames); // Trả về danh sách UserName
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while fetching user names.", Details = ex.Message });
            }
        }


        // GET: api/user/{id}
        [HttpGet("GetInforBy{UserID:guid}")]  // Lấy thông tin người dùng dựa trên ID
        public async Task<IActionResult> GetById(Guid UserID)
        {
            try
            {
                // Lấy người dùng theo ID
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserID == UserID);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while fetching the user.", Details = ex.Message });
            }
        }

        // GET: api/user/{username} // Lấy ID của người dùng dựa trên UserName
        [HttpGet("GetIDby{UserName}")]
        public async Task<IActionResult> GetUserIdByUserName(string UserName)
        {
            try
            {
                // Tìm người dùng có UserName khớp với username được cung cấp
                var userId = await _context.Users
                    .Where(u => u.UserName == UserName) // Điều kiện tìm kiếm
                    .Select(u => u.UserID)              // Chỉ lấy UserID
                    .SingleOrDefaultAsync();

                if (userId == Guid.Empty) // Guid.Empty là giá trị mặc định nếu không tìm thấy
                {
                    return NotFound(new { Message = "User not found." });
                }

                return Ok(userId); // Trả về UserID
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while fetching user ID.", Details = ex.Message });
            }
        }


        // POST: api/user
        [HttpPost("CreateNewUser")]
        public async Task<IActionResult> Create([FromBody] UserRequest UserRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Kiểm tra UserName đã tồn tại
                var existingUser = await _context.Users.AnyAsync(u => u.UserName == UserRequest.UserName);

                if (existingUser)
                {
                    return BadRequest(new { Message = "UserName already exists." });
                }

                // Thêm người dùng mới
                var newUser = new User
                {
                    UserName = UserRequest.UserName,
                    UserPassword = UserRequest.UserPassword,
                    Fullname = UserRequest.Fullname,
                    DateOfBirth = UserRequest.DateOfBirth
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "User created successfully.", UserID = newUser.UserID });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while creating user.", Details = ex.Message });
            }
        }


        // PUT: api/user/{id}   
        [HttpPut("Edit{UserID:guid}")]  // Thay đổi thông tin người dùng
        public async Task<IActionResult> Edit(Guid UserID, [FromBody] UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Tìm người dùng cần chỉnh sửa
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserID == UserID);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found." });
                }

                // Cập nhật thông tin người dùng
                user.UserName = userRequest.UserName;
                user.UserPassword = userRequest.UserPassword;
                user.Fullname = userRequest.Fullname;
                user.DateOfBirth = userRequest.DateOfBirth;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while updating the user.", Details = ex.Message });
            }
        }

        [HttpPut("ChangePassword{UserName}")] 
        public async Task<IActionResult> ChangePassword(string UserName, [FromBody] ChangePasswordRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Tìm người dùng theo UserName
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == UserName);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found." });
                }

                // Kiểm tra mật khẩu cũ
                if (user.UserPassword != request.OldPassword)
                {
                    return BadRequest(new { Message = "Old password is incorrect." });
                }

                // Thay đổi mật khẩu
                user.UserPassword = request.NewPassword;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Password updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while updating the password.", Details = ex.Message });
            }
        }

        [HttpDelete("Delete{UserID:guid}")]
        public async Task<IActionResult> Remove(Guid UserID)
        {
            try
            {
                // Tìm người dùng cần xóa
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserID == UserID);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found." });
                }

                // Xóa người dùng khỏi database
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new { Success = true, Message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while deleting the user.", Details = ex.Message });
            }
        }
    }
}
