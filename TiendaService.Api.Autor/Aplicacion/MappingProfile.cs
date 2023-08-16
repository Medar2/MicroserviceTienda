using AutoMapper;
using TiendaService.Api.Autor.Modelo;

namespace TiendaService.Api.Autor.Aplicacion
{
    public class MappingProfile: Profile
    {
        public MappingProfile() { 
        
            CreateMap<AutorLibro,AutorDto>();
        
        }
    }
}
