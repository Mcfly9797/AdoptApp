//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace appdoptanosV2
//{
//    public class BK
//    {


//        [Route("api/[controller]")]
//        [ApiController]
//        public class AdopcionsController : ControllerBase
//        {
//            private readonly MyDbContext _context;

//            public AdopcionsController(MyDbContext context)
//            {
//                _context = context;
//            }




//            // GET: api/Adopcions
//            [HttpGet]
//            public async Task<ActionResult<IEnumerable<AdopcionDTO>>> GetAdopcion()
//            {

//                var lstAdopcionBd = await _context.Adopcion.ToListAsync();

//                //var lstAdopcionBd = await (from adopcion in _context.Adopcion
//                //                           select new
//                //                           {
//                //                               IdAdopcion = adopcion.IdAdopcion,
//                //                               IdAnimal = adopcion.AnimalId,
//                //                               NombreAnimal = adopcion.Animal.Nombre,
//                //                               RescatistaUserId = adopcion.AdoptanteUser.IdUser,
//                //                               RescatistaUserNombre = adopcion.RescatistaUser.Nombre,
//                //                               AdoptanteUserId = adopcion.AdoptanteUser.IdUser,
//                //                               AdoptanteUserNombre = adopcion.AdoptanteUser.Nombre
//                //                           }).ToListAsync();

//                var lstAdopcionDTO = new List<AdopcionDTO>();
//                foreach (var adopcionBd in lstAdopcionBd)
//                    lstAdopcionDTO.Add(AdopcionToDTO(adopcionBd));

//                if (lstAdopcionDTO.Count != 0)
//                    return Ok(lstAdopcionDTO);
//                else
//                    return NotFound("No se encuentran datos");
//            }



//            // GET: api/Adopcions/5
//            [HttpGet("{id}")]
//            public async Task<ActionResult<AdopcionDTO>> GetAdopcion(int id)
//            {



//                var adopcionBd = await _context.Adopcion.FirstOrDefaultAsync(x => x.IdAdopcion == id);


//                if (adopcionBd == null)
//                    return NotFound();
//                else
//                {
//                    AdopcionDTO adopcionDTO = new AdopcionDTO
//                    {
//                        IdAdopcion = adopcionBd.IdAdopcion,
//                        AnimalId = adopcionBd.AnimalId,
//                        NombreAnimal = adopcionBd.Animal.Nombre,
//                        RescatistaUserId = adopcionBd.AdoptanteUser.IdUser,
//                        RescatistaUserNombre = adopcionBd.RescatistaUser.NombreUser,
//                        AdoptanteUserId = adopcionBd.AdoptanteUser.IdUser,
//                        AdoptanteUserNombre = adopcionBd.RescatistaUser.NombreUser
//                    };
//                }


//                if (adopcionBd == null)
//                    return NotFound();
//                return AdopcionToDTO(adopcionBd);
//            }




//            // PUT: api/Adopcions/5
//            // To protect from overposting attacks, enable the specific properties you want to bind to, for
//            // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
//            [HttpPut("{id}")]
//            public async Task<IActionResult> PutAdopcion(int id, AdopcionDTO adopcionDTO)
//            {
//                if (id != adopcionDTO.IdAdopcion)
//                    return BadRequest();


//                _context.Entry(AdopcionToModel(adopcionDTO)).State = EntityState.Modified;

//                try
//                {
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!AdopcionExists(id))
//                        return NotFound();
//                    else
//                        throw;
//                }

//                return Ok("Elemento modificado correctamente");
//            }




//            // POST: api/Adopcions
//            // To protect from overposting attacks, enable the specific properties you want to bind to, for
//            // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
//            [HttpPost]
//            public async Task<ActionResult<Adopcion>> PostAdopcion(AdopcionDTO adopcionDTO)
//            {
//                _context.Adopcion.Add(AdopcionToModel(adopcionDTO).Result);
//                await _context.SaveChangesAsync();
//                return CreatedAtAction("GetAdopcion", new { id = adopcionDTO.IdAdopcion }, adopcionDTO);
//            }




//            // DELETE: api/Adopcions/5
//            [HttpDelete("{id}")]
//            public async Task<ActionResult<Adopcion>> DeleteAdopcion(int id)
//            {
//                var adopcion = await _context.Adopcion.FindAsync(id);
//                if (adopcion == null)
//                    return NotFound();

//                _context.Adopcion.Remove(adopcion);
//                await _context.SaveChangesAsync();

//                return Ok("Elemento eliminado correctamente");
//            }




//            //Comprueba que el elemento exista
//            private bool AdopcionExists(int id)
//            {
//                return _context.Adopcion.Any(e => e.IdAdopcion == id);
//            }




//            //Conversion de Model a DTO
//            private static AdopcionDTO AdopcionToDTO(Adopcion adopcionModel)
//            {
//                AdopcionDTO adopcionDTO = new AdopcionDTO
//                {
//                    IdAdopcion = adopcionModel.IdAdopcion,
//                    AnimalId = adopcionModel.AnimalId,
//                    NombreAnimal = adopcionModel.Animal.Nombre,
//                    RescatistaUserId = adopcionModel.AdoptanteUser.IdUser,
//                    RescatistaUserNombre = adopcionModel.RescatistaUser.NombreUser,
//                    AdoptanteUserId = adopcionModel.AdoptanteUser.IdUser,
//                    AdoptanteUserNombre = adopcionModel.RescatistaUser.NombreUser
//                };

//                return adopcionDTO;
//            }




//            //Conversion de DTO a Model
//            private async Task<Adopcion> AdopcionToModel(AdopcionDTO adopcionDTO) =>
//                new Adopcion
//                {
//                    IdAdopcion = adopcionDTO.IdAdopcion,
//                    AnimalId = adopcionDTO.AnimalId,
//                    RescatistaUser = await _context.User.FirstOrDefaultAsync(user => user.IdUser == adopcionDTO.RescatistaUserId),
//                    AdoptanteUser = await _context.User.FirstOrDefaultAsync(user => user.IdUser == adopcionDTO.AdoptanteUserId)
//                };
//        }



//    }
//}
