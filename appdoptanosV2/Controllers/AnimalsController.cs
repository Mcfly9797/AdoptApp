using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appdoptanos.Api.Models;
using Appdoptanos.Api.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace appdoptanosV2.Controllers
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

        // GET: api/Animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetAnimal()
        {
            var lstAnimalDTO = await (from animal in _context.Animal
                                      select new AnimalDTO
                                      {
                                          IdAnimal = animal.IdAnimal,
                                          Nombre = animal.Nombre,
                                          Color = animal.Color,
                                          FecNac = animal.FecNac,
                                          Disponibilidad = animal.Disponibilidad
                                      }).ToListAsync();
            if (lstAnimalDTO.Count != 0)
                return Ok(lstAnimalDTO);
            return NotFound("No hay animales disponibles para adoptar");
        }

        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalDTO>> GetAnimal(int id)
        {
            var animal = await (from animalBd in _context.Animal
                                where animalBd.IdAnimal == id
                                select new AnimalDTO
                                {
                                    IdAnimal = animalBd.IdAnimal,
                                    Nombre = animalBd.Nombre,
                                    Color = animalBd.Color,
                                    Disponibilidad = animalBd.Disponibilidad,
                                    FecNac = animalBd.FecNac
                                }).FirstOrDefaultAsync();
            if (animal == null)
                return NotFound();
            return animal;
        }

        // PUT: api/Animals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, Animal animal)
        {
            if (id != animal.IdAnimal)
                return BadRequest("El id es diferente al del animal");

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
                    return NotFound("No se encontro el animal ingresado");
                else
                    throw;
            }
            return Ok("Elemento modificado satisfactoriamente");
        }

        // POST: api/Animals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(Animal animal)
        {
            _context.Animal.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnimal", new { id = animal.IdAnimal }, animal);
        }

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
                return NotFound();

            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.IdAnimal == id);
        }
    }
}
