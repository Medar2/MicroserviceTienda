using Microsoft.EntityFrameworkCore;
using TiendaService.Api.Autor.Modelo;

namespace TiendaService.Api.Autor.Persistencia
{
    public class ContextoAutor: DbContext
    {
        public ContextoAutor(DbContextOptions<ContextoAutor> options) : base(options) { }

        public DbSet<AutorLibro> AutorLibro  { get; set; }
        public DbSet<GradoAcademico> GradoAcademico  { get; set; }
    }
}
