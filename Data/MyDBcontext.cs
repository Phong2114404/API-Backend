using Microsoft.EntityFrameworkCore;

namespace IoTwithMysql.Data
{
    public class MyDBcontext : DbContext
    {
        public MyDBcontext(DbContextOptions<MyDBcontext> options) : base(options) {}

        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Measurement>()
                .HasOne(m => m.User)
                .WithMany(u => u.Measurements)
                .HasForeignKey(m => m.UserID)
                .OnDelete(DeleteBehavior.Cascade); // Kích hoạt xóa cascading

            modelBuilder.Entity<Measurement>()
                .HasOne(m => m.Sensor) // Liên kết với bảng Sensors
                .WithMany(s => s.Measurements)
                .HasForeignKey(m => m.Version)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Measurement nếu Sensor bị xóa

        }
    }
}
