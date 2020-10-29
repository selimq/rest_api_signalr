using Login.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Login.Repo
{
    public class LoginDbContext:DbContext
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options) :base(options) { }

        public DbSet<Person> Girisler { get; set; }

        internal Task FindAsync(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
