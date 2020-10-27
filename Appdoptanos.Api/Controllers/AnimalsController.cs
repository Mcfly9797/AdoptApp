using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appdoptanos.Api.Models;
using Appdoptanos.Api.DTOs;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Dynamic;

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
            var lstAnimalBD = await (from animal in _context.Animal
                                      select new
                                      {
                                          animal.IdAnimal,
                                          animal.Nombre,
                                          animal.Color,
                                          animal.FecNac,
                                          animal.Especie.NombreEspecie,
                                          animal.Disponibilidad
                                      }).ToListAsync();

            var lstAnimalDTO = new List<AnimalDTO>();
            foreach (var animalBd in lstAnimalBD)
                lstAnimalDTO.Add(AnimalToDTO(animalBd));
            
            if (lstAnimalDTO.Count != 0)
                return Ok(lstAnimalDTO);
            else
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
                                         animal.Especie.NombreEspecie,
                                         animal.Disponibilidad
                                     }).ToListAsync();

            var lstAnimalDTO = new List<AnimalDTO>();
            foreach (var animalBd in lstAnimalBD)
                 lstAnimalDTO.Add(AnimalToDTO(animalBd));

            if (lstAnimalDTO.Count != 0)
                return  Ok(lstAnimalDTO);
            else
                return NotFound("No hay animales disponibles para adoptar");
        }




        //Trae animal por id
        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalDTO>> GetAnimal(int id)
        {
            var animalBd = await (from animal in _context.Animal
                                  where animal.IdAnimal == id
                                  select new
                                 {
                                     animal.IdAnimal,
                                     animal.Nombre,
                                     animal.Color,
                                     animal.FecNac,
                                     animal.Especie.NombreEspecie,
                                     animal.Disponibilidad
                                 }).FirstOrDefaultAsync();

            if (animalBd != null)
                //Transformo la consulta al DTO y lo devuelvo
                return Ok(AnimalToDTO(animalBd));
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
                return BadRequest();

            //Busco si el nombre de especie ingresado en el dto a ver si existe
            var especieQuery =
               (from especie in _context.Especie
                where especie.NombreEspecie == animalDTO.NombreEspecie
                select especie).FirstOrDefaultAsync();
            
            //Creo el Model para llenarlo con los datos del DTO y posterior insertar
            Animal animalModel = new Animal();

            if (especieQuery != null)
            {
                animalModel = new Animal
                {
                    IdAnimal = id,
                    Nombre = animalDTO.Nombre,
                    Color = animalDTO.Color,
                    FecNac = animalDTO.FecNac,
                    EspecieId = especieQuery.Result.IdEspecie,
                    //Especie = new Especie { NombreEspecie = especieQuery.Result.NombreEspecie, IdEspecie = especieQuery.Result.IdEspecie },
                    Disponibilidad = true
                };
            }
            else
            {
                return NotFound("No se encontro la especie ingresada");

            }


            //Guardo el AnimalModel
            _context.Entry(animalModel).State = EntityState.Modified;

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
            return Ok("Elemento modificado satisfactoriamente");
        }




        // POST: api/Animals
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AnimalDTO>> PostAnimal(AnimalDTO animalDTO)
        {
            //Busco si el nombre de especie ingresado en el dto a ver si existe
            var especieQuery = (await    (from especie in _context.Especie
                                            where especie.NombreEspecie == animalDTO.NombreEspecie
                                            select especie)
                                            .FirstOrDefaultAsync());

            //Creo el Model para llenarlo con los datos del DTO y posterior insertar
            Animal animalModel = new Animal();

            //Verifico que haya encontrado la especie que ingreso el usuario
            if (especieQuery != null)
            {
                animalModel = new Animal
                                {
                                    //IdAnimal = animalDTO.IdAnimal,
                                    Nombre = animalDTO.Nombre,
                                    Color = animalDTO.Color,
                                    FecNac = animalDTO.FecNac,
                                    EspecieId = especieQuery.IdEspecie,
                                    Disponibilidad = true
                                };
            }
            else
                return NotFound("No se encontro la especie ingresada");

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
        return CreatedAtAction("GetAnimal", new { id = animalModel.IdAnimal },  new {
                                                                                        //IdAnimal = animalDTO.IdAnimal,
                                                                                        Nombre = animalDTO.Nombre,
                                                                                        Color = animalDTO.Color,
                                                                                        FecNac = animalDTO.FecNac,
                                                                                        NombreEspecie = animalDTO.NombreEspecie,
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
                                        FecNac = animal.FecNac,
                                        NombreEspecie = await (from especie in _context.Especie
                                                              where especie.IdEspecie == animal.EspecieId
                                                              select especie.NombreEspecie).FirstOrDefaultAsync()
            };
            _context.Animal.Remove(animal);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(JsonConvert.SerializeObject( new { 
                                                            Menssage = "Se elimino el siguiente elemento: ",
                                                            Elemento = new{
                                                                            IdAnimal = animalDTO.IdAnimal,
                                                                            Nombre = animalDTO.Nombre,
                                                                            Color = animalDTO.Color,
                                                                            FecNac = animalDTO.FecNac,
                                                                            NombreEspecie = animalDTO.NombreEspecie
                                                                           }
                                                            }));
            }
            catch (Exception)
            {
                throw;
            }
        }





        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.IdAnimal == id);
        }

        private static AnimalDTO AnimalToDTO(dynamic animalBd) =>
             new AnimalDTO
            {
                IdAnimal = animalBd.IdAnimal,
                Nombre = animalBd.Nombre,
                Color = animalBd.Color,
                FecNac = animalBd.FecNac,
                NombreEspecie = animalBd.NombreEspecie,
                Disponibilidad = animalBd.Disponibilidad
            };
    }
}
