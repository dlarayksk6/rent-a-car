using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RentACar.DataAccess
{
    public class RentACarDbContextFactory : IDesignTimeDbContextFactory<RentACarDbContext>
    {
        public RentACarDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RentACarDbContext>();
            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=rentacardb;user=root;password=abcde",
                new MySqlServerVersion(new Version(8, 0, 32))
            );
            return new RentACarDbContext(optionsBuilder.Options);
        }
    }
}

