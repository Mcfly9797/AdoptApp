using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appdoptanos.Api.Models;
using Appdoptanos.Api.DTOs;

namespace Appdoptanos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {

        //Esta clase  representa la bd
        private readonly MyDbContext _context;

        public AnimalsController(MyDbContext context)
        {
            _context = context;
        }


        //Trae todos los animales
        // GET: api/Animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimal()
        {
            ObjectResult objectResult;

            var query =
            from animal in _context.Animal
            select new
            {
                IdAnimal = animal.IdAnimal,
                Nombre = animal.Nombre,
                Color = animal.Color,
                FecNac = animal.FecNac,
                NombreEspecie = animal.Especie.NombreEspecie,
                Disponibilidad = animal.Disponibilidad
            };

            var lstAnimalBD = await query.ToListAsync();
            var lstAnimalDTO = new List<AnimalDTO>();

            foreach (var animalbd in lstAnimalBD)
            {
                lstAnimalDTO.Add(new AnimalDTO
                {
                    //IdAnimal = animalbd.IdAnimal,
                    Nombre = animalbd.Nombre,
                    Color = animalbd.Color,
                    FecNac = animalbd.FecNac,
                    Especie = animalbd.NombreEspecie,
                    Disponibilidad = animalbd.Disponibilidad
                });
            }
            if (lstAnimalDTO.Count != 0)
                objectResult = Ok(lstAnimalDTO);
            else
                objectResult = NotFound("No se encuentran datos");
            return objectResult;
        }


        //Traigo solo los animales que estan disponibles para adoptar
        // GET: api/AvailableAnimal
        [HttpGet("Disponibles")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAvailableAnimal()
        {
            ObjectResult objectResult;

            var query =
            from animal in _context.Animal
            where animal.Disponibilidad == true
            select new
            {
                IdAnimal = animal.IdAnimal,
                Nombre = animal.Nombre,
                Color = animal.Color,
                FecNac = animal.FecNac,
                NombreEspecie = animal.Especie.NombreEspecie,
                Disponibilidad = animal.Disponibilidad
            };

            var lstAnimalBD = await query.ToListAsync();
            var lstAnimalDTO = new List<AnimalDTO>();

            foreach (var animalbd in lstAnimalBD)
            {
                lstAnimalDTO.Add(new AnimalDTO
                {
                    //IdAnimal = animalbd.IdAnimal,
                    Nombre = animalbd.Nombre,
                    Color = animalbd.Color,
                    FecNac = animalbd.FecNac,
                    Especie = animalbd.NombreEspecie,
                    Disponibilidad = animalbd.Disponibilidad
                });
            }
            if (lstAnimalDTO.Count != 0)
                objectResult = Ok(lstAnimalDTO);
            else
                objectResult = NotFound("No hay animales disponibles para adoptar");
            return objectResult;
        }

        //Trae animal por id
        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var query =
              from animal in _context.Animal
              where animal.IdAnimal == id
              select new
              {
                  animal.IdAnimal,
                  animal.Nombre,
                  animal.Color,
                  animal.FecNac,
                  animal.Especie.NombreEspecie,
                  animal.Disponibilidad
              };

            var animalBd = await query.FirstOrDefaultAsync();

            if (animalBd != null)
            {
                //Transformo la consulta al DTO
                AnimalDTO animalDTO = new AnimalDTO
                {
                    //IdAnimal = animalBd.IdAnimal,
                    Nombre = animalBd.Nombre,
                    Color = animalBd.Color,
                    FecNac = animalBd.FecNac,
                    Especie = animalBd.NombreEspecie,
                    Disponibilidad = animalBd.Disponibilidad
                };
                return Ok(animalDTO);
            }
            return NotFound("No hay animales con ese id"); ;
        }

        //Modifico animal por id
        // PUT: api/Animals/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, AnimalDTO animalDTO)
        {
            //if (id != animalDTO.IdAnimal)
            //{
            //    return BadRequest();
            //}

            //Busco la especie del animal ingresado para inicializar luego en el EspecieModel
            var especieQuery =
             (from especie in _context.Especie
              where especie.NombreEspecie == animalDTO.Nombre
              select new
              {
                  NombreEspecie = especie.NombreEspecie,
                  IdEspecie = especie.IdEspecie
              }).FirstOrDefault();



            //Tengo que convertir el animalDTO en AnimalModel
            Animal animalBd = new Animal {
                //IdAnimal = animalDTO.IdAnimal,
                Nombre = animalDTO.Nombre,
                Color = animalDTO.Color,
                FecNac = animalDTO.FecNac,
                Especie = new Especie{NombreEspecie = especieQuery.NombreEspecie,IdEspecie = especieQuery.IdEspecie},
                Disponibilidad = animalDTO.Disponibilidad
            };

            //Guardo el AnimalModel
            _context.Entry(animalBd).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id) && especieQuery != null)
                    return NotFound("No se encontro el animal ingresado");
                if (especieQuery == null)
                    return NotFound("No se encontro la especie ingresada");
                else
                    throw;
            }
            return NoContent();
        }

        // POST: api/Animals
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(AnimalDTO animalDTO)
        {
            //Esta linea de abajo no va a funcionar porque yo cuando mando el DTO desde el front envio solo el nombre de la especia, asi que en el back deberia buscar el id de la especia
            //Especie especie = await _context.Especie.FindAsync(animalDTO.Especie);

            //Busco si el nombre de especie ingresado en el dto a ver si existe
            var especieQuery =
               (from especie in _context.Especie
                where especie.NombreEspecie == animalDTO.Especie
                select new
                {
                    NombreEspecie = especie.NombreEspecie,
                    IdEspecie = especie.IdEspecie
                }).FirstOrDefault();
            //Creo el Model para llenarlo con los datos del DTO y posterior insertar
            Animal animalModel = new Animal();

            if (especieQuery != null)
            {
                animalModel = new Animal
                {
                    //IdAnimal = animalDTO.IdAnimal,
                    Nombre = animalDTO.Nombre,
                    Color = animalDTO.Color,
                    FecNac = animalDTO.FecNac,
                    Especie = new Especie { NombreEspecie = especieQuery.NombreEspecie, IdEspecie = especieQuery.IdEspecie },
                    Disponibilidad = true
                };
            }
            else
            {
                return NotFound("No se encontro la especie ingresada");

            }
           
            //Uso atach en lugar de Add porque: Add inserta en las tablas relacionada los valores que yo ingrese como si fueran nuevos, en cambio atach solo inserta en la tabla donde quiero insertar valores principalmente.
            _context.Animal.Attach(animalModel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                    throw;
            }
            return CreatedAtAction("GetAnimal", new { id = animalModel.IdAnimal }, new AnimalDTO { Nombre = animalModel.Nombre,
                                                                                                    Color = animalModel.Color,
                                                                                                    FecNac = animalModel.FecNac,
                                                                                                    Especie = animalModel.Nombre,
                                                                                                    //Disponibilidad = animalModel.Disponibilidad
                                                                                                    });

        }

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Animal>> DeleteAnimal(int id)
        {
            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            //Revisar pruebas a ver si elimina tambien la especie al eliminar el animal
            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();

            return animal;
        }

        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.IdAnimal == id);
        }
    }
}
