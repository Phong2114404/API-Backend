using IoTwithMysql.Data;
using IoTwithMysql.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTwithMysql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly MyDBcontext _context;

        public SensorController(MyDBcontext context)
        {
            _context = context;
        }

        // Lấy tất cả sensor
        [HttpGet]
        public async Task<IActionResult> GetAllSensors()
        {
            try
            {
                var sensors = await _context.Sensors.ToListAsync();
                return Ok(sensors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while fetching sensors.", Details = ex.Message });
            }
        }

        // Lấy sensor theo version
        [HttpGet("{version:int}")]
        public async Task<IActionResult> GetSensorByVersion(int version)
        {
            try
            {
                var sensor = await _context.Sensors.SingleOrDefaultAsync(s => s.Version == version);

                if (sensor == null)
                {
                    return NotFound(new { Message = "Sensor not found." });
                }

                return Ok(sensor);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while fetching the sensor.", Details = ex.Message });
            }
        }

        // Tạo version mới cho bộ sensor
        [HttpPost]
        public async Task<IActionResult> CreateSensor([FromBody] SensorModel sensorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Tạo sensor mới từ client model
                var sensor = new Sensor
                {
                    SpO2 = sensorModel.SpO2,
                    Temp = sensorModel.Temp,
                    HeartRate = sensorModel.HeartRate,
                    Weight = sensorModel.Weight
                };

                _context.Sensors.Add(sensor);
                await _context.SaveChangesAsync();

                //return CreatedAtAction(nameof(GetSensorByVersion), new { version = sensor.Version }, sensor);
                return Ok(sensor);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while creating the sensor.", Details = ex.Message });
            }
        }

        // Cập nhật phiên bản sensor (Không nên dùng) => dùng POST để tạo version mới
        [HttpPut("{version:int}")]
        public async Task<IActionResult> UpdateSensor(int version, [FromBody] SensorModel sensorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var sensor = await _context.Sensors.SingleOrDefaultAsync(s => s.Version == version);

                if (sensor == null)
                {
                    return NotFound(new { Message = "Sensor not found." });
                }

                // Cập nhật giá trị từ client model
                sensor.SpO2 = sensorModel.SpO2;
                sensor.Temp = sensorModel.Temp;
                sensor.HeartRate = sensorModel.HeartRate;
                sensor.Weight = sensorModel.Weight;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.Sensors.Update(sensor);
                await _context.SaveChangesAsync();

                return Ok(sensor);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while updating the sensor.", Details = ex.Message });
            }
        }

        // Xóa phiên bản sensor
        [HttpDelete("{version:int}")]
        public async Task<IActionResult> DeleteSensor(int version)
        {
            try
            {
                var sensor = await _context.Sensors.SingleOrDefaultAsync(s => s.Version == version);

                if (sensor == null)
                {
                    return NotFound(new { Message = "Sensor not found." });
                }

                _context.Sensors.Remove(sensor);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Sensor deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while deleting the sensor.", Details = ex.Message });
            }
        }







    }
}
