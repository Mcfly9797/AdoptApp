using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appdoptanos.Api.Models;
using Appdoptanos.Api.DTOs;
using Newtonsoft.Json;

namespace appdoptanosV2.Controllers
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
        public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetAnimal()
        {
            var lstAnimalBd = await (from animal in _context.Animal
                                     select new
                                     {
                                         animal.IdAnimal,
                                         animal.Nombre,
                                         animal.Color,
                                         animal.FecNac,
                                         animal.Disponibilidad
                                     }).ToListAsync();

            var lstAnimalDTO = new List<AnimalDTO>();
            foreach (var animalBd in lstAnimalBd)
                lstAnimalDTO.Add(AnimalToDTO(animalBd));

            if (lstAnimalDTO.Count != 0)
                return Ok(lstAnimalDTO);
            return NotFound("No se encuentran datos");
        }




        //Traigo solo los animales que estan disponibles para adoptar
        // GET: api/AvailableAnimal
        [HttpGet("Disponibles")]
        public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetAvailableAnimal()
        {
            var lstAnimalBD = await (from animal in _context.Animal
                                     where animal.Disponibilidad == true
                                     select new
                                     {
                                         animal.IdAnimal,
                                         animal.Nombre,
                                         animal.Color,
                                         animal.FecNac,
                                         animal.Disponibilidad
                                     }).ToListAsync();

            var lstAnimalDTO = new List<AnimalDTO>();
            foreach (var animalBd in lstAnimalBD)
                lstAnimalDTO.Add(AnimalToDTO(animalBd));

            if (lstAnimalDTO.Count != 0)
                return Ok(lstAnimalDTO);
            else
                return NotFound("No hay animales disponibles para adoptar");
        }




        //Trae animal por id
        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalDTO>> GetAnimal(int id)
        {
            var animalModel = await (from animal in _context.Animal
                                     where animal.IdAnimal == id
                                     select new
                                     {
                                         animal.IdAnimal,
                                         animal.Nombre,
                                         animal.Color,
                                         animal.FecNac,
                                         animal.Disponibilidad
                                     }).FirstOrDefaultAsync();

            if (animalModel != null)
                //Transformo la consulta al DTO y lo devuelvo
                return Ok(AnimalToDTO(animalModel));
            return NotFound("No hay animales con ese id");
        }




        //Modifico animal por id
        // PUT: api/Animals/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, AnimalDTO animalDTO)
        {
            if (id != animalDTO.IdAnimal)
                return BadRequest("El id es diferente al del animal");

            ////Busco si el nombre de especie ingresado en el dto a ver si existe
            //var especieQuery =
            //   (from especie in _context.Especie
            //    where especie.NombreEspecie == animalDTO.NombreEspecie
            //    select especie).FirstOrDefaultAsync();

            //Creo el Model para llenarlo con los datos del DTO y posterior insertar


            Animal animalModel = new Animal
                {
                    IdAnimal = id,
                    Nombre = animalDTO.Nombre,
                    Color = animalDTO.Color,
                    FecNac = animalDTO.FecNac,
                    //Especie = new Especie { NombreEspecie = especieQuery.Result.NombreEspecie, IdEspecie = especieQuery.Result.IdEspecie },
                    Disponibilidad = true
                };
            


            //Guardo el AnimalModel
            _context.Entry(animalModel).State = EntityState.Modified;

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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AnimalDTO>> PostAnimal(dynamic animalJson)
        {
            ////Busco si el nombre de especie ingresado en el dto a ver si existe
            //var especieQuery = (await (from especie in _context.Especie
            //                           where especie.NombreEspecie == animalDTO.NombreEspecie
            //                           select especie)
            //                                .FirstOrDefaultAsync());

            Animal animalModel = new Animal
            {
                IdAnimal = animalJson.IdAnimal,
                Nombre = animalJson.Nombre,
                Color = animalJson.Color,
                FecNac = animalJson.FecNac,
                Disponibilidad = true
            };

            //Uso atach en lugar de Add porque: Add inserta en las tablas relacionada los valores que yo ingrese como si fueran nuevos, en cambio atach solo inserta en la tabla donde quiero insertar valores principalmente.
            _context.Animal.Attach(animalModel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
            return CreatedAtAction("GetAnimal", new { id = animalModel.IdAnimal }, new
            {
                IdAnimal = animalJson.IdAnimal,
                Nombre = animalJson.Nombre,
                Color = animalJson.Color,
                FecNac = animalJson.FecNac,
                NombreEspecie = animalJson.NombreEspecie,
                Disponibilidad = true
            });
        }




        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnimalDTO>> DeleteAnimal(int id)
        {
            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
                return NotFound("No se encontro el elemento a eliminar");

            //Creo el AnimalDTO para devolver al usuario el elemento eliminado como informacion.
            AnimalDTO animalDTO = new AnimalDTO
            {
                IdAnimal = animal.IdAnimal,
                Nombre = animal.Nombre,
                Color = animal.Color,
                FecNac = animal.FecNac
            };
            _context.Animal.Remove(animal);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(JsonConvert.SerializeObject(new
                {
                    Menssage = "Se elimino el siguiente elemento: ",
                    Elemento = new
                    {
                        IdAnimal = animalDTO.IdAnimal,
                        Nombre = animalDTO.Nombre,
                        Color = animalDTO.Color,
                        FecNac = animalDTO.FecNac,
                    }
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }



        //Comprueba que el elemento exista
        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.IdAnimal == id);
        }



        //Conversion de Model a DTO
        private static AnimalDTO AnimalToDTO(dynamic animalBd) =>
             new AnimalDTO
             {
                 IdAnimal = animalBd.IdAnimal,
                 Nombre = animalBd.Nombre,
                 Color = animalBd.Color,
                 FecNac = animalBd.FecNac,
                 Disponibilidad = animalBd.Disponibilidad
             };





        private async Task<ActionResult<dynamic>> BuscarAnimal(int id)
        {

            var animalModel = await(from animal in _context.Animal
                                    where animal.IdAnimal == id
                                    select new
                                    {
                                        animal.IdAnimal,
                                        animal.Nombre,
                                        animal.Color,
                                        animal.FecNac,
                                        animal.Disponibilidad
                                    }).FirstOrDefaultAsync();

            if (animalModel != null)
                //Transformo la consulta al DTO y lo devuelvo
                return Ok(AnimalToDTO(animalModel));
            return NotFound("No hay animales con ese id");
        }
    }
}
