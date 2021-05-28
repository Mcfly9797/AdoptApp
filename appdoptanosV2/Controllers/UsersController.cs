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
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UsersController(MyDbContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
        {
            List<UserDTO> lstUserDTO = await (from user in _context.User
                                              select new UserDTO
                                              {
                                                 IdUser=user.IdUser,
                                                  NombreUser= user.NombreUser,
                                                  Email = user.Email,
                                                  PerfilVerificado = user.PerfilVerificado,
                                                  Reputacion = user.Reputacion
                                              }).ToListAsync();
            if (lstUserDTO.Count != 0)
                return Ok(lstUserDTO);
            return NotFound("No se encuentran datos");
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            UserDTO userDTO = await (from user in _context.User
                                  where user.IdUser == id
                                  select new UserDTO
                                  {
                                      IdUser = user.IdUser,
                                      NombreUser = user.NombreUser,
                                      Email = user.Email,
                                      PerfilVerificado = user.PerfilVerificado,
                                      Reputacion = user.Reputacion
                                  }).FirstOrDefaultAsync();


            // _context.User.FindAsync(id);  
            if (userDTO == null)
                return NotFound("Ese usuario no existe");
            return Ok(userDTO);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User userInput)
        {
            if (id != userInput.IdUser)
                return BadRequest();


            _context.Entry(new User
            {
                IdUser      = userInput.IdUser,
                NombreUser = userInput.NombreUser,
                Clave = userInput.Clave,
                Nombre = userInput.Nombre,
                Email = userInput.Email,
                Dni = userInput.Dni,
                Phone = userInput.Phone,
                Domicilio = userInput.Domicilio,
                Localidad = userInput.Localidad,
                Provincia = userInput.Provincia,
            
            }).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
                    //Crear archivo de logs para saber porque fallo
            }

            return Ok("El elemento fue modificado correctamente");
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User userInput)
        {
            _context.User.Add(userInput);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { id = userInput.IdUser }, userInput);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return BadRequest("El id es diferente al del usuario");

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("Elemento modificado satisfactoriamente");
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.IdUser == id);
        }
    }
}
