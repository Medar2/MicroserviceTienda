using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TiendaServicios.Api.Libros.Aplicacion;
using TiendaServicios.Api.Libros.Modelo;
using TiendaServicios.Api.Libros.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Test
{
    public class LibrosServiceTest
    {
        private IEnumerable<LibreriaMaterial> ObtenerDataPruena()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
                
        }

        private Mock<ContextoLibreria> CrearContexto()
        {
            var dataPrueba = ObtenerDataPruena().AsQueryable();

            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider)
                .Returns(new AsyncQueryprovider<LibreriaMaterial>(dataPrueba.Provider));


            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
        }

        public async void GetLibroPorId()
        {
            var mockContexto = CrearContexto();
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            ConsultaFiltro.LibroUnico request = new ConsultaFiltro.LibroUnico();
            //var request = new ConsultaFiltro.LibroUnico();
            request.LibroId = Guid.Empty;

            //var manejador = new ConsultaFiltro.Manejador(mockContexto.Object,mapper);
            ConsultaFiltro.Manejador manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);
            

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);

        }
        [Fact]
        public async void GetLibros()
        {
            //que texto dentro de mi microservice libro se esta encargando 
            //de realizar la consulta de libros de la base de datos??

            // 1. Emular a la instancia de entity framekork core - contexto Libreria
            // para emular la acciones y eventos de un objeto en un ambiente de unit test
            // utilzando objetos de tipo mock.


            //            var mockContexto = new Mock<ContextoLibreria>();
            var mockContexto = CrearContexto();

            // 2.  >Emular al mappiing Imapper

            //var mockMapper = new Mock<IMapper>();
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();
            //3. Instanciar a la clase manjejador y pasarle como parametros los mock que hemos crado

            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await  manejador.Handle(request,new System.Threading.CancellationToken());

            Assert.True(lista.Any());
        }

        [Fact]
        public async void Guardarlibro()
        {
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatoslibro")
                .Options;

            var contexto = new ContextoLibreria(options);

            var request = new Nuevo.Ejecuta();
            request.Titulo = "Libro de Microservice";
            request.AutorLibro = Guid.Empty;
            request.FechaPublicacion = DateTime.Now;

            var manjeador = new Nuevo.Manejador(contexto);

            var libro = await manjeador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(libro != null);






        }


    }
}
