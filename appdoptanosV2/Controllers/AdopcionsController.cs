using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appdoptanos.Api.Models;
using Appdoptanos.Api.DTOs;

namespace appdoptanosV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdopcionsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AdopcionsController(MyDbContext context)
        {
            _context = context;
        }

        delegate Task<Adopcion> ToModel(dynamic obj);
        // GET: api/Adopcions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdopcionDTO>>> GetAdopcion()
        {
            var lstAdopcionDTO = await (from adopcion in _context.Adopcion
                                       select new AdopcionDTO
                                       {
                                           IdAdopcion = adopcion.IdAdopcion,
                                           AnimalId = adopcion.AnimalId,
                                           NombreAnimal = adopcion.Animal.Nombre,
                                           RescatistaUserId = adopcion.AdoptanteUser.IdUser,
                                           RescatistaUserNombre = adopcion.RescatistaUser.Nombre,
                                           AdoptanteUserId = adopcion.AdoptanteUser.IdUser,
                                           AdoptanteUserNombre = adopcion.AdoptanteUser.Nombre
                                       }).ToListAsync();

            if (lstAdopcionDTO.Count != 0)
                return Ok(lstAdopcionDTO);
            else
                return NotFound("No se encuentran datos");
        }

        // GET: api/Adopcions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdopcionDTO>> GetAdopcion(int id)
        {
            var adopcionDTO = await (from adopcion in _context.Adopcion
                                     where adopcion.IdAdopcion == id
                                     select new AdopcionDTO
                                     {
                                         IdAdopcion = adopcion.IdAdopcion,
                                         AnimalId = adopcion.AnimalId,
                                         NombreAnimal = adopcion.Animal.Nombre,
                                         RescatistaUserId = adopcion.AdoptanteUser.IdUser,
                                         RescatistaUserNombre = adopcion.RescatistaUser.Nombre,
                                         AdoptanteUserId = adopcion.AdoptanteUser.IdUser,
                                         AdoptanteUserNombre = adopcion.AdoptanteUser.Nombre
                                     }).FirstOrDefaultAsync();
            if (adopcionDTO == null)
                return NotFound();
            return Ok(adopcionDTO);
        }

        // PUT: api/Adopcions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdopcion(int id, AdopcionDTO adopcionDTO)
        {
            if (id != adopcionDTO.IdAdopcion)
                return BadRequest();

            _context.Entry(new Adopcion
                                  {
                                      IdAdopcion = adopcionDTO.IdAdopcion,
                                      AnimalId = adopcionDTO.AnimalId,
                                      Animal = await _context.Animal.FindAsync(adopcionDTO.AnimalId),
                                      RescatistaUserId = adopcionDTO.RescatistaUserId,
                                      RescatistaUser = await _context.User.FindAsync(adopcionDTO.AdoptanteUserId),
                                      AdoptanteUserId = adopcionDTO.AdoptanteUserId,
                                      AdoptanteUser = await _context.User.FindAsync(adopcionDTO.AdoptanteUserId)
                                  }).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdopcionExists(id))
                    return NotFound();
                else
                    throw;
            }
            return Ok("Elemento modificado correctamente");
        }

        // POST: api/Adopcions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Adopcion>> PostAdopcion(AdopcionDTO adopcionDTO)
        {
            _context.Adopcion.Add((new Adopcion
                                    {
                                        IdAdopcion = adopcionDTO.IdAdopcion,
                                        AnimalId = adopcionDTO.AnimalId,
                                        Animal = await _context.Animal.FindAsync(adopcionDTO.AnimalId),
                                        RescatistaUserId = adopcionDTO.RescatistaUserId,
                                        RescatistaUser = await _context.User.FindAsync(adopcionDTO.AdoptanteUserId),
                                        AdoptanteUserId = adopcionDTO.AdoptanteUserId,
                                        AdoptanteUser = await _context.User.FindAsync(adopcionDTO.AdoptanteUserId)
                                    }));
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAdopcion", new { id = adopcionDTO.IdAdopcion }, adopcionDTO);
        }

        // DELETE: api/Adopcions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdopcion(int id)
        {
            Adopcion adopcion = await (from adopcionBd in _context.Adopcion
                                  where adopcionBd.IdAdopcion == id
                                  select new Adopcion
                                  {
                                      IdAdopcion = adopcionBd.IdAdopcion,
                                      AnimalId = adopcionBd.AnimalId,
                                      Animal = adopcionBd.Animal,
                                      AdoptanteUserId = adopcionBd.AdoptanteUserId,
                                      AdoptanteUser = adopcionBd.AdoptanteUser,
                                      RescatistaUserId = adopcionBd.RescatistaUserId,
                                      RescatistaUser = adopcionBd.RescatistaUser
                                  }).FirstOrDefaultAsync();
            if (adopcion == null)
                return NotFound("No se encontro adopcion");

            _context.Adopcion.Remove(adopcion);
            await _context.SaveChangesAsync();
            return Ok("Elemento eliminado correctamente");
        }

        private bool AdopcionExists(int id)
        {
            return _context.Adopcion.Any(e => e.IdAdopcion == id);
        }
    }
}
