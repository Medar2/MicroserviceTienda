using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TiendaServicios.Api.Libros.Aplicacion;
using TiendaServicios.Api.Libros.Modelo;

namespace TiendaServicios.Api.Libro.Test
{
    public class MappingTest : Profile
    {
        public MappingTest()
        {
              CreateMap<LibreriaMaterial, LibreriaMaterialDto>();  
        }
    }
}
