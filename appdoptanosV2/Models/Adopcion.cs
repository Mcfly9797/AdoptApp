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
        
        //FK a nivel bd
        public int AnimalId { get; set; }
        //Relacion a nivel objetos (reference navigation property)
        public virtual Animal Animal { get; set; }

        public int AdoptanteUserId { get; set; }
        [ForeignKey("AdoptanteUserId")]
        public virtual User AdoptanteUser { get; set; }

        public int RescatistaUserId { get; set; }
        [ForeignKey("RescatistaUserId")]
        public virtual User RescatistaUser { get; set; }
    }
}
