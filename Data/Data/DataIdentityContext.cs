using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccess_Layer.Data
{
    public class DataIdentityContext : IdentityDbContext<ApplicationUser>
    {


        public DataIdentityContext(DbContextOptions<DataIdentityContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
