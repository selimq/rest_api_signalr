using Login.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Login.Repo
{
    public class LoginDbContext:DbContext
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options) :base(options) { }

        public DbSet<Giris> Girisler { get; set; }

        public DbSet<Nobet> Nobetler { get; set; } 
    }
}
