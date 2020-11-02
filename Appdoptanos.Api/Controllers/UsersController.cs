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
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UsersController(MyDbContext context)
        {
            _context = context;
        }




        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
        {


            var lstUserModel = await (from user in _context.User
                                     select new
                                     {
                                         user.IdUser,
                                         user.NombreUser,
                                         user.Nombre,
                                         user.Apellido,
                                         user.Email,
                                         user.Reputacion,
                                         user.PerfilVerificado
                                     }).ToListAsync();

            var lstUserDto = new List<UserDTO>();
            foreach (var userModel in lstUserModel)
                lstUserDto.Add(UserToDTO(userModel));

            if (lstUserModel.Count != 0)
                return Ok(lstUserModel); 
            return NotFound("No se encuentran datos");


        }




        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var userModel = await (from user in _context.User
                                   where user.IdUser == id
                                   select new
                                   {
                                       user.IdUser,
                                       user.NombreUser,
                                       user.Nombre,
                                       user.Apellido,
                                       user.Email,
                                       user.Reputacion,
                                       user.PerfilVerificado
                                   }).FirstOrDefaultAsync();

            if (userModel == null)
                return NotFound("Ese usuario no existe");
            return UserToDTO(userModel);
        }




        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDTO)
        {
            if (id != userDTO.IdUser)
                return BadRequest();
            


            var userModel = new User
            {
                IdUser      = userDTO.IdUser,
                NombreUser = userDTO.NombreUser,
                Clave = userDTO.Clave,
                Nombre = userDTO.Nombre,
                Apellido = userDTO.Apellido,
                Email = userDTO.Email,
                Dni = userDTO.Dni,
                Phone = userDTO.Phone,
                Domicilio = userDTO.Domicilio,
                Localidad = userDTO.Localidad,
                Provincia = userDTO.Provincia,
            };

            _context.Entry(userModel).State = EntityState.Modified;

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
            }
            return Ok("Elemento modificado satisfactoriamente");
        }




        //SIGNUP REGISTRO USUARIOS
        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO)
        {


            var userModel = new User
            {
                NombreUser = userDTO.NombreUser,
                Clave = userDTO.Clave,
                Email = userDTO.Email,
            }; 

            _context.User.Add(userModel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            //return CreatedAtAction("GetUser", new { id = userDTO.IdUser }, userModel);
            return Ok("Usuario creado");
        }

        //completar perfil prueba patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, UserDTO userDTO)
        {
            if (id != userDTO.IdUser)
                return BadRequest("El id es diferente al del usuario");

            var userQuery = await (from user in _context.User
                                   where user.IdUser == id
                                   select new
                                   {
                                       user.IdUser,
                                       user.NombreUser,
                                       user.Clave,
                                       user.Nombre,
                                       user.Apellido,
                                       user.Email,
                                       user.Dni,
                                       user.Phone,
                                       user.Domicilio,
                                       user.Localidad,
                                       user.Provincia,
                                       user.Reputacion,
                                       user.PerfilVerificado
                                   }).FirstOrDefaultAsync();

            if (userQuery == null)
                return NotFound("Ese usuario no existe");


            User userModel = new User
            {
                IdUser = id,
                NombreUser = userQuery.NombreUser,
                Clave = userQuery.Clave,
                Nombre = userDTO.Nombre,
                Apellido = userDTO.Apellido,
                Email = userDTO.Email,
                Dni = userDTO.Dni,
                Phone = userDTO.Phone,
                Domicilio = userDTO.Domicilio,
                Localidad = userDTO.Localidad,
                Provincia = userDTO.Provincia,
                Reputacion = userDTO.Reputacion,
                PerfilVerificado = userQuery.PerfilVerificado,
            };

            _context.Entry(userModel).State = EntityState.Modified;



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
            }
            return Ok("Elemento modificado satisfactoriamente");
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound("No se encontro el elemento a eliminar");
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok($"El usuario con id: {id} ha sido eliminado eliminado ");
        }




        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.IdUser == id);
        }




        private static UserDTO UserToDTO(dynamic UserBd) =>
            new UserDTO
            {
             IdUser           = UserBd.IdUser,
             NombreUser       = UserBd.NombreUser,
             Nombre           =   UserBd.Nombre,
             Apellido         =   UserBd.Apellido,
             Email            =  UserBd.Email,
             Reputacion       =  UserBd.Reputacion,
             PerfilVerificado =   UserBd.PerfilVerificado
            };
    }
}
