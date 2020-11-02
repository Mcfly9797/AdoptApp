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
    public class AdopcionsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AdopcionsController(MyDbContext context)
        {
            _context = context;
        }




        // GET: api/Adopcions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdopcionDTO>>> GetAdopcion()
        {

            //var lstAdopcionBd = await _context.Adopcion.ToListAsync();
            
            var lstAdopcionBd = await (from adopcion in _context.Adopcion
                    select new
                    {
                        IdAdopcion = adopcion.IdAdopcion,
                        IdAnimal = adopcion.AnimalId,
                        NombreAnimal = adopcion.Animal.Nombre,
                        RescatistaUserId = adopcion.AdoptanteUser.IdUser,
                        RescatistaUserNombre = adopcion.RescatistaUser.Nombre,
                        AdoptanteUserId = adopcion.AdoptanteUser.IdUser,
                        AdoptanteUserNombre = adopcion.AdoptanteUser.Nombre
                    }).ToListAsync();

            var lstAdopcionDTO = new List<AdopcionDTO>();
            foreach (var adopcionBd in lstAdopcionBd)
                lstAdopcionDTO.Add(AdopcionToDTO(adopcionBd));

            if (lstAdopcionDTO.Count != 0)
                return Ok(lstAdopcionDTO);
            else
                return NotFound("No se encuentran datos");
        }



        // GET: api/Adopcions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Adopcion>> GetAdopcion(int id)
        {
            var adopcion = await _context.Adopcion.FindAsync(id);

            if (adopcion == null)
            {
                return NotFound();
            }

            return adopcion;
        }




        // PUT: api/Adopcions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdopcion(int id, Adopcion adopcion)
        {
            if (id != adopcion.IdAdopcion)
            {
                return BadRequest();
            }

            _context.Entry(adopcion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdopcionExists(id))
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




        // POST: api/Adopcions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Adopcion>> PostAdopcion(Adopcion adopcion)
        {
            _context.Adopcion.Add(adopcion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdopcion", new { id = adopcion.IdAdopcion }, adopcion);
        }




        // DELETE: api/Adopcions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Adopcion>> DeleteAdopcion(int id)
        {
            var adopcion = await _context.Adopcion.FindAsync(id);
            if (adopcion == null)
            {
                return NotFound();
            }

            _context.Adopcion.Remove(adopcion);
            await _context.SaveChangesAsync();

            return adopcion;
        }




        private bool AdopcionExists(int id)
        {
            return _context.Adopcion.Any(e => e.IdAdopcion == id);
        }


        private static AdopcionDTO AdopcionToDTO(dynamic adopcionBd) =>
             new AdopcionDTO
             {
                IdAdopcion = adopcionBd.IdAdopcion,

                AnimalId = adopcionBd.IdAnimal,
                NombreAnimal = adopcionBd.NombreAnimal,

                RescatistaUserId = adopcionBd.RescatistaUserId,
                RescatistaUserNombre = adopcionBd.RescatistaUserNombre,

                AdoptanteUserId = adopcionBd.AdoptanteUserId,
                AdoptanteUserNombre = adopcionBd.AdoptanteUserNombre
             };
    }
}
