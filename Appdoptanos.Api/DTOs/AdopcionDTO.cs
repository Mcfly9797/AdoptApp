using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appdoptanos.Api.DTOs
{

    public class AdopcionDTO
    {
        public int IdAdopcion { get; set; }

        public int AnimalId { get; set; }
        public string NombreAnimal { get; set; }

        public int RescatistaUserId { get; set; }
        public string RescatistaUserNombre { get; set; }

        public int AdoptanteUserId { get; set; }
        public string AdoptanteUserNombre { get; set; }


        //public AdopcionDTO(int idAdopcion, int animalId, int rescatistaUserId, int adoptanteUserId)
        //{
        //    IdAdopcion = idAdopcion;
        //    AnimalId = animalId;
        //    RescatistaUserId = rescatistaUserId;
        //    AdoptanteUserId = adoptanteUserId;
        //}
    }


}
