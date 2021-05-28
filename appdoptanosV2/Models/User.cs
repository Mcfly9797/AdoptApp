using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appdoptanos.Api.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string NombreUser { get; set; }

        [Required(ErrorMessage = "La clave es requerida")]
        public string Clave { get; set; }

        //[Required(ErrorMessage = "El campo es requerido")]
        [StringLength(25)]
        public string Nombre { get; set; }

        ////[Required(ErrorMessage = "El campo es requerido")]
        //[StringLength(50)]
        //public string? Apellido { get; set; }

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(10, MinimumLength = 1)]
        public int? Dni { get; set; }

        //IMPORTANTE VALIDO EL NUMERO DE TELEFONO
        [Phone]
        public int? Phone { get; set; }

        //IMPORTANTE PARA VALIDAR DOMICILIO PEDIR ACCESO A UBICACION Y FIJARSE SI COINCIDE LA UBICACION CON LA QUE PUSO
        public string? Domicilio { get; set; }

        public string? Localidad { get; set; }

        public string? Provincia { get; set; }

        public bool PerfilVerificado { get; set; }

        [Range(0, 5)]
        public int? Reputacion { get; set; }


        [InverseProperty("AdoptanteUser")]
        public virtual ICollection<Adopcion> Adoptados { get; set; }
        [InverseProperty("RescatistaUser")]
        public virtual ICollection<Adopcion> Rescatados { get; set; }

    }
}
