using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using TiendaServicios.Api.Libros.Persistencia;
using System.Linq;
using TiendaServicios.Api.Libros.Modelo;
using Microsoft.EntityFrameworkCore;

namespace TiendaServicios.Api.Libros.Aplicacion
{
    public class ConsultaFiltro
    {
        public class LibroUnico : IRequest<LibreriaMaterialDto>
        {
            public Guid? LibroId { get; set; }
        }

        public class Manejador : IRequestHandler<LibroUnico, LibreriaMaterialDto>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                this._contexto = contexto;
                this._mapper = mapper;
            }
            public async Task<LibreriaMaterialDto> Handle(LibroUnico request, CancellationToken cancellationToken)
            {
                var libro = await _contexto.LibreriaMaterial.Where(x => x.LibreriaMaterialId == request.LibroId).FirstOrDefaultAsync();
                if (libro == null)
                {
                    throw new Exception("No se encontro el libro");
                }
                var libroDto = _mapper.Map<LibreriaMaterial, LibreriaMaterialDto>(libro);
                return libroDto;
            }
        }
    }
}
