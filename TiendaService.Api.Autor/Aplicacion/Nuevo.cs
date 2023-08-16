using FluentValidation;
using MediatR;
using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TiendaService.Api.Autor.Modelo;
using TiendaService.Api.Autor.Persistencia;

namespace TiendaService.Api.Autor.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {

            public int AutorLibroId { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
        }
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
            }
        }

        public class Manjador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoAutor _contextoAutor;

            public Manjador(ContextoAutor contextoAutor)
            {
                this._contextoAutor = contextoAutor;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var autrolibro = new AutorLibro
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FechaNacimiento = request.FechaNacimiento,
                    AutorLibroGuid = Guid.NewGuid().ToString(),
                };
                
                _contextoAutor.AutorLibro.Add(autrolibro);
               var valor = await _contextoAutor.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el Autor del Libro");

            }
        }

    }

}
