using Furion;
using Furion.DatabaseAccessor;
using FurionShardingDemo.EfCores;
using FurionShardingDemo.Shardings;
using Microsoft.EntityFrameworkCore;
using ShardingCore;

namespace FurionShardingDemo
{
    [AppStartup(600)]
    public sealed class SqlServerEntityFrameworkCoreStartup : AppStartup
    {
        public static readonly ILoggerFactory efLogger = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole();
        });
        public void ConfigureServices(IServiceCollection services)
        {
            var connStr = DbProvider.GetConnectionString<DefaultDbContext>(/*这里可写可不写*/);
            // 配置数据库上下文，支持N个数据库
            services.AddDatabaseAccessor(options =>
            {
                // 配置默认数据库
                options.AddDb<DefaultDbContext>(o =>
                {
                    o.UseSqlServer(connStr).UseSharding<DefaultDbContext>().UseLoggerFactory(efLogger);
                });

            });
            services.AddShardingConfigure<DefaultDbContext>((s, builder) =>
            {
                builder.UseSqlServer(s).UseLoggerFactory(efLogger);
            }).Begin(o =>
            {
                o.CreateShardingTableOnStart = true;
                o.EnsureCreatedWithOutShardingTable = true;
                o.AutoTrackEntity = true;
            })
                 .AddShardingTransaction((connection, builder) =>
                     builder.UseSqlServer(connection).UseLoggerFactory(efLogger))
                 .AddDefaultDataSource("ds0", connStr)
                 .AddShardingTableRoute(o =>
                 {
                     o.AddShardingTableRoute<TodoItemVirtualTableRoute>();
                 }).End();
        }
    }
}
