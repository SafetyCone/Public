﻿using System;

using Microsoft.EntityFrameworkCore;

using EntityTypes = ExaminingEntityFramework.Lib.EntityTypes;


namespace ExaminingEntityFramework.Lib
{
    public class DatabaseContext : DbContext
    {
        public DbSet<EntityTypes.EntityA> EntityAs { get; set; }
        public DbSet<EntityTypes.EntityALabel> EntityALabels { get; set; }

        public DbSet<EntityTypes.Blog> Blogs { get; set; }
        public DbSet<EntityTypes.Post> Posts { get; set; }


        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityTypes.EntityA>().HasAlternateKey(x => x.GUID);
        }
    }
}
