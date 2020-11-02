using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appdoptanos.Api.DTOs
{
    public class UserDTO
    {
        public int IdUser { get; set; }

        public string NombreUser { get; set; }

        public string Clave { get; set; }

        public string? Nombre { get; set; }
        
        public string? Apellido { get; set; }

        public string Email { get; set; }

        public int? Dni { get; set; }

        public int? Phone { get; set; }

        //IMPORTANTE PARA VALIDAR DOMICILIO PEDIR ACCESO A UBICACION Y FIJARSE SI COINCIDE LA UBICACION CON LA QUE PUSO
        public string Domicilio { get; set; }

        public string Localidad { get; set; }

        public string Provincia { get; set; }

        public int? Reputacion { get; set; }
        
        public bool PerfilVerificado { get; set; }

    }
}
