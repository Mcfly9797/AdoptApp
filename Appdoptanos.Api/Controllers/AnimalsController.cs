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
                    IdAnimal = animalbd.IdAnimal,
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
                    IdAnimal = animalbd.IdAnimal,
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

        //Trae animales por id
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
                    IdAnimal = animalBd.IdAnimal,
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
            if (id != animalDTO.IdAnimal)
            {
                return BadRequest();
            }

            _context.Entry(animalDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Animals
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(AnimalDTO animalDTO)
        {

            Especie especie = await _context.Especie.FindAsync(animalDTO.Especie);

            Animal animalModel = new Animal
            {
                IdAnimal = animalDTO.IdAnimal,
                Nombre = animalDTO.Nombre,
                Color = animalDTO.Color,
                FecNac = animalDTO.FecNac,
                Especie = especie,
                Disponibilidad = animalDTO.Disponibilidad
            };

            _context.Animal.Add(animalModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnimal", new { id = animalModel.IdAnimal }, animalModel);
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
