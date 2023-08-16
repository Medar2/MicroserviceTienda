using System;
using System.Collections.Generic;

namespace TiendaService.Api.Autor.Modelo
{
    public class AutorLibro
    {
        public int AutorLibroId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento{ get; set; }

        //Relacion con Grados Academico
        public ICollection<GradoAcademico> LibroGradoAcademico { get;}

        //Clave primaria
        public string AutorLibroGuid { get; set; }
    }
}
