using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DataBaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;
        public DataBaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }
        public DataBaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DataBaseContext> optionsBuilder = new();
            _configureDbContext(optionsBuilder);
            return new DataBaseContext(optionsBuilder.Options); 
        }
    }
}