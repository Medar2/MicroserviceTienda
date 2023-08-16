using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TiendaService.Api.CarritoCompra.Models;

namespace TiendaService.Api.CarritoCompra.Persistencia
{
    public class DbContextCarritoCompra :DbContext
    {
        private readonly IConfiguration configuration;

        public DbContextCarritoCompra(DbContextOptions<DbContextCarritoCompra> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<CarritoSesion> CarritoSesion { get; set; }
        public DbSet<CarritoSesionDetalle> CarritoSesionDetalle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(configuration.GetConnectionString("MYSQL_DATABASE");
        }
    }
}
