using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DeportesWeb.Models
{
    public class Contexto
    {
        public class SportUsersDBContext : DbContext
        {
            public SportUsersDBContext() : base("DefaultConnection")
            {

            }
            public DbSet<SportUsers> SportUser { get; set; }
        }

        public class SportsDBContext : DbContext
        {
            public SportsDBContext() : base("DefaultConnection")
            {

            }
            public DbSet<Sports> Sports { get; set; }
        }

        public class SportRegistryDBContext : DbContext
        {
            public SportRegistryDBContext() : base("DefaultConnection")
            {

            }
            public DbSet<SportRegistry> SportRegistry { get; set; }
        }

        public class FrequencyDBContext : DbContext
        {
            public FrequencyDBContext() : base("DefaultConnection")
            {

            }
            public DbSet<Frequency> Frequency { get; set; }
        }

        public class UsersDataDBContext : DbContext
        {
            public UsersDataDBContext() : base("DefaultConnection")
            {

            }
            public DbSet<UsersData> UsersData { get; set; }
        }

        public class RegistryFrequencyDBContext : DbContext
        {
            public RegistryFrequencyDBContext() : base("DefaultConnection")
            {

            }
            public DbSet<RegistryFrequency> RegistryFrequency { get; set; }
        }
    }
}