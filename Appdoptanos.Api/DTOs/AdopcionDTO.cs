using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appdoptanos.Api.DTOs
{

    public class AdopcionDTO
    {
        public int IdAdopcion;

        public AnimalDTO Animal;

        public UserDTO RescatistaUser;

        public UserDTO AdoptanteUser;

        public AdopcionDTO(int idAdopcion, AnimalDTO animal, UserDTO rescatistaUser, UserDTO adoptanteUser)
        {
            IdAdopcion = idAdopcion;
            Animal = animal;
            RescatistaUser = rescatistaUser;
            AdoptanteUser = adoptanteUser;
        }
    }


}
