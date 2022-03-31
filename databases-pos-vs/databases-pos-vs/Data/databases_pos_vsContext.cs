using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using databseApp.Models;

namespace databases_pos_vs.Data
{
    public class databases_pos_vsContext : DbContext
    {
        public databases_pos_vsContext (DbContextOptions<databases_pos_vsContext> options)
            : base(options)
        {
        }

        public DbSet<databseApp.Models.ProductViewModel> ProductViewModel { get; set; }
    }
}
