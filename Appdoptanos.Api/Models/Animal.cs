using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appdoptanos.Api.Models
{
    public class Animal
    {

        [Key]

        public int IdAnimal { get; set; }
        
        [Required(ErrorMessage = "Es necesario el nombre del animal")]
        [MaxLength(25, ErrorMessage = "Este campo acepta hasta 20 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Es necesario el color del animal")]
        [MaxLength(25, ErrorMessage = "Este campo acepta hasta 20 caracteres")]
        public string Color { get; set; }


        [Required(ErrorMessage = "Es necesaria la fecha estimada de nacimiento del animal")]
        public DateTime FecNac { get; set; }

        [Required(ErrorMessage = "La disponibilidad es obligatoria")]
        public bool Disponibilidad { get; set; }

        //FK
        public int EspecieId { get; set; }
        //Relacion a nivel objetos
        public virtual Especie Especie { get; set; }

        public Adopcion Adopcion { get; set; }
    }
}
