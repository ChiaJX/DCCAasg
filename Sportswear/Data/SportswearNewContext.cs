using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sportswear.Models;

namespace Sportswear.Data
{
    public class SportswearNewContext : DbContext
    {
        public SportswearNewContext (DbContextOptions<SportswearNewContext> options)
            : base(options)
        {
        }
      
        public DbSet<Sportswear.Models.ContactUs> ContactUs { get; set; }

        public DbSet<Sportswear.Models.Transaction> Transaction { get; set; }
    }
}
