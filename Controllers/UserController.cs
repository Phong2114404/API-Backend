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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Lấy danh sách tất cả người dùng từ database
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while fetching users.", Details = ex.Message });
            }
        }

        // GET: api/user/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                // Lấy người dùng theo ID
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserID == id);

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

        // POST: api/user
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = new User
                {
                    UserID = Guid.NewGuid(),
                    UserName = userRequest.UserName,
                    UserPassword = userRequest.UserPassword,
                    Fullname = userRequest.Fullname,
                    DateOfBirth = userRequest.DateOfBirth
                };

                // Thêm người dùng mới vào database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { Success = true, Data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while creating the user.", Details = ex.Message });
            }
        }


        // PUT: api/user/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Tìm người dùng cần chỉnh sửa
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserID == id);

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

                return Ok(new { Success = true, Data = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while updating the user.", Details = ex.Message });
            }
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try
            {
                // Tìm người dùng cần xóa
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserID == id);

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
