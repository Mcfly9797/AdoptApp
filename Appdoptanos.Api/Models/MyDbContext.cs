using Appdoptanos.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appdoptanos.Api.Models
{
    public class MyDbContext : DbContext
    {

        //MyDbContext es mi contexto que "maneja todo" con la bd. Recordar siempre inicializar los servicios de las conecciones en el archivo "Startup.cs"
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        { 
        }

        //Los nombres que les de a estas variables van a ser los de cada tabla. Con cada DbSet creo una tabla.
        public DbSet<Especie> Especie { get; set; }
        public DbSet<Animal> Animal { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Adopcion> Adopcion { get; set; }



    }
}
