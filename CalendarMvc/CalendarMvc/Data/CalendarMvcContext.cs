using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CalendarMvc.Models
{
    public class CalendarMvcContext : DbContext
    {
        public CalendarMvcContext (DbContextOptions<CalendarMvcContext> options)
            : base(options)
        {
        }

        public DbSet<CalendarMvc.Models.Event> Event { get; set; }
    }
}
