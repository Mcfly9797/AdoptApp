using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appdoptanos.Api.Models
{


    //La adopcion solamente se guarda como un historial de una adopcion ya concretada
    public class Adopcion
    {
        [Key]
        public int IdAdopcion { get; set; }
        
        //FK Una adopcion tiene un 
        public int AnimalId { get; set; }
        //Relacion a nivel objetos
        public virtual Animal Animal { get; set; }

        [ForeignKey("RescatistaID")]
        [InverseProperty("RescatistaUser")]
        public virtual User RescatistaUser { get; set; }

        [ForeignKey("AdoptanteID")]
        [InverseProperty("AdoptanteUser")]
        public virtual User AdoptanteUser { get; set; }

    }
}
