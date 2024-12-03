using IoTwithMysql.Data;
using IoTwithMysql.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTwithMysql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly MyDBcontext _context;

        public MeasurementController(MyDBcontext context)
        {
            _context = context;
        }

        // GET: api/measurement
        [HttpGet("GetAllMeasurements")]
        public async Task<IActionResult> GetAll()
        {
            // Lấy tất cả bản ghi Measurement
            var measurements = await _context.Measurements.ToListAsync();
            return Ok(measurements);
        }

        // GET: api/measurement/{id}
        [HttpGet("GetMeasurementBy{MeasurementID:int}")]
        public async Task<IActionResult> GetById(int MeasurementID)
        {
            try
            {
                // Lấy Measurement theo ID
                var measurement = await _context.Measurements
                    .Include(m => m.User) // Bao gồm thông tin User liên kết
                    .Select(m => new
                    {
                        m.MeasurementID,
                        m.MeasurementTime,
                        m.SpO2,
                        m.Temp,
                        m.HeartRate,
                        m.Weight,
                        m.Version
                    })
                    .SingleOrDefaultAsync(m => m.MeasurementID == MeasurementID);

                if (measurement == null)
                {
                    return NotFound(new { Message = "Measurement not found." });
                }

                return Ok(measurement);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Invalid request or internal error occurred.", Details = ex.Message });
            }
        }


        [HttpGet("GetMeasurementsOf{UserID:guid}")]
        public async Task<IActionResult> GetByUserId(Guid UserID)
        {
            try
            {
                // Truy vấn theo UserID và chỉ lấy 10 lần đo gần nhất
                var measurements = await _context.Measurements
                    .Where(m => m.UserID == UserID) // Lọc theo UserID
                    .OrderByDescending(m => m.MeasurementTime) // Sắp xếp theo thời gian giảm dần
                    .Take(10) // Lấy 10 bản ghi đầu tiên
                    .ToListAsync();

                // Kiểm tra nếu không có bản ghi nào
                if (measurements == null || !measurements.Any())
                {
                    return NotFound(new { Message = "No measurements found for the specified UserID." });
                }

                // Trả về danh sách Measurement
                return Ok(measurements);
            }
            catch
            {
                return BadRequest(new { Message = "Invalid request or internal error occurred." });
            }
        }

        // POST: api/measurement
        [HttpPost("CreateMeasurementof{UserID:guid}")]
        public async Task<IActionResult> Create(Guid UserID, [FromBody] MeasurementModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Tạo đối tượng Measurement mới
                var measurement = new Measurement
                {
                    UserID = UserID,         // Gán UserID từ request
                    SpO2 = request.SpO2,            // Gán giá trị SpO2
                    Temp = request.Temp,            // Gán giá trị nhiệt độ
                    HeartRate = request.HeartRate,  // Gán giá trị nhịp tim
                    Weight = request.Weight,        // Gán giá trị cân nặng
                    MeasurementTime = DateTime.UtcNow,  // Thời gian hiện tại
                    Version = request.Version    
                };

                // Thêm bản ghi vào database
                _context.Measurements.Add(measurement);
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công với Measurement đã lưu
                return Ok(measurement);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while creating the measurement.", Details = ex.Message });
            }
        }

        [HttpPut("Edit{MeasurementID:int}")]
        public async Task<IActionResult> Update(int MeasurementID, [FromBody] MeasurementModel measurementUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Lấy Measurement cần cập nhật
                var measurement = await _context.Measurements
                    .Include(m => m.User) // Bao gồm User để kiểm tra UserID
                    .SingleOrDefaultAsync(m => m.MeasurementID == MeasurementID);

                if (measurement == null)
                {
                    return NotFound(new { Message = "Measurement not found." });
                }

                // Cập nhật giá trị
                measurement.SpO2 = measurementUpdate.SpO2;
                measurement.Temp = measurementUpdate.Temp;
                measurement.HeartRate = measurementUpdate.HeartRate;
                measurement.Weight = measurementUpdate.Weight;
                measurement.Version = measurementUpdate.Version;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.Measurements.Update(measurement);
                await _context.SaveChangesAsync();

                return Ok(measurement);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while updating the measurement.", Details = ex.Message });
            }
        }

        // DELETE: api/measurement/{id}
        [HttpDelete("Delete{MeasurementID:int}")]
        public async Task<IActionResult> Delete(int MeasurementID)
        {
            try
            {
                // Lấy Measurement cần xóa
                var measurement = await _context.Measurements.SingleOrDefaultAsync(m => m.MeasurementID == MeasurementID);

                if (measurement == null)
                {
                    return NotFound(new { Message = "Measurement not found." });
                }

                // Xóa Measurement
                _context.Measurements.Remove(measurement);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Measurement deleted successfully." });
            }
            catch
            {
                return BadRequest(new { Message = "Error while deleting the measurement." });
            }
        }

        [HttpDelete("DeleteMeasurementsOf{UserID:guid}")]
        public async Task<IActionResult> DeleteMeasurementsByUserId(Guid UserID)
        {
            try
            {
                // Lấy tất cả measurements thuộc UserID
                var measurements = await _context.Measurements
                    .Where(m => m.UserID == UserID)
                    .ToListAsync();

                if (measurements.Count == 0)
                {
                    return NotFound(new { Message = "No measurements found for the specified UserID." });
                }

                // Xóa tất cả measurements
                _context.Measurements.RemoveRange(measurements);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "All measurements for the user have been deleted." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while deleting measurements.", Details = ex.Message });
            }
        }


    }
}

