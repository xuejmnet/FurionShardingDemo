using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using ShardingCore.Core.VirtualRoutes.TableRoutes.RouteTails.Abstractions;
using ShardingCore.Sharding.Abstractions;

namespace FurionShardingDemo.EfCores
{
    [AppDbContext("SqlServerConnectionString", DbProvider.SqlServer)]
    public class DefaultDbContext : AppShardingDbContext<DefaultDbContext>,IShardingTableDbContext
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
        }

        public IRouteTail RouteTail { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).IsRequired().IsUnicode(false).HasMaxLength(50).HasComment("Id");
                entity.Property(o => o.Name).IsRequired().HasMaxLength(50).HasComment("名称");
                entity.ToTable(nameof(TodoItem));
            });
        }
    }
}
