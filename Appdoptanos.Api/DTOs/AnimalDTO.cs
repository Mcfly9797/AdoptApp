using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appdoptanos.Api.DTOs
{
    public class AnimalDTO
    {
        public int IdAnimal { get; set; }
        public string Nombre { get; set; }
        public string Color { get; set; }
        public DateTime FecNac { get; set; }
        public string NombreEspecie { get; set; }
        public bool Disponibilidad { get; set; }
    }
}
