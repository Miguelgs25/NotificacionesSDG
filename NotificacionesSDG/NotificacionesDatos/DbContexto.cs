using Microsoft.EntityFrameworkCore;
using NotificacionesDatos.Entitades;

namespace NotificacionesDatos
{
    public class DbContexto :DbContext
    {
        public DbContexto(DbContextOptions<DbContexto> options) : base(options)
        {
        }

        public DbSet<Lector> Lectores { get; set; }
    }
}