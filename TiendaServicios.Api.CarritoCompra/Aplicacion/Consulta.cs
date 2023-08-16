using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecutar : IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecutar, CarritoDto>
        {
            private readonly CarritoContexto carritoContexto;
            private readonly ILibroService libroService;

            public Manejador(CarritoContexto carritoContexto, ILibroService libroService)
            {
                this.carritoContexto = carritoContexto;
                this.libroService = libroService;
            }

            public async Task<CarritoDto> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var carritoSesion = await carritoContexto.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle = await carritoContexto.CarritoSesionDetalle.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                var listaCarritoDto = new List<CarritoDetalleDto>();

                foreach (var libro in carritoSesionDetalle)
                {
                    var response = await libroService.Getlibros(new Guid(libro.ProductoSeleccionado));
                    if (response.resultado)
                    {
                        var objectoLibro = response.Libro;
                        var carritoDetalle = new CarritoDetalleDto
                        {
                            TituloLibro = objectoLibro.Titulo,
                            FechaPublicacion = objectoLibro.FechaPublicacion,
                            LibroId = objectoLibro.LibreriaMaterialId,
                        };
                        listaCarritoDto.Add(carritoDetalle);

                    }
                }

                var carritoSesionDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = listaCarritoDto
                };

                return carritoSesionDto;

            }
        }
    }
}
