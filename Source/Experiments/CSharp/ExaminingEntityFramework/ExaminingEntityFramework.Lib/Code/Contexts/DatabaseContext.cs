using System;

using Microsoft.EntityFrameworkCore;


namespace ExaminingEntityFramework.Lib
{
    public class DatabaseContext : DbContext
    {
        public DbSet<EntityTypes.EntityA> EntityAs { get; set; }
        public DbSet<EntityTypes.EntityALabel> EntityALabels { get; set; }

        public DbSet<EntityTypes.EntityB> EntityBs { get; set; }
        public DbSet<EntityTypes.EntityC> EntityCs { get; set; }

        public DbSet<EntityTypes.EntityBToEntityCMapping> EntityBToEntityCMappings { get; set; }

        public DbSet<EntityTypes.Event> Events { get; set; }
        public DbSet<EntityTypes.EventType> EventTypes { get; set; }

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
