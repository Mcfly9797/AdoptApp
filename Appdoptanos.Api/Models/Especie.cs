using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appdoptanos.Api.Models
{
    public class Especie
    {
        [Key]
        public int IdEspecie { get; set; }

        [Required(ErrorMessage="Es necesario el nombre de la especie")]
        [MaxLength(30, ErrorMessage = "Este campo acepta hasta 30 caracteres")]
        public string NombreEspecie { get; set; }

    }
}
