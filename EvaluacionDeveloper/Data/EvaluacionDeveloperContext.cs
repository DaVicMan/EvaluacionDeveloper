using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EvaluacionDeveloper.Models;

namespace EvaluacionDeveloper.Data
{
    public class EvaluacionDeveloperContext : DbContext
    {
        public EvaluacionDeveloperContext (DbContextOptions<EvaluacionDeveloperContext> options)
            : base(options)
        {
        }

        public DbSet<EvaluacionDeveloper.Models.AuthorModel> Author { get; set; } = default!;
    }
}
