using FurionShardingDemo.EfCores;
using ShardingCore.Core.EntityMetadatas;
using ShardingCore.VirtualRoutes.Mods;

namespace FurionShardingDemo.Shardings
{
    public class TodoItemVirtualTableRoute : AbstractSimpleShardingModKeyStringVirtualTableRoute<TodoItem>
    {
        public TodoItemVirtualTableRoute() : base(2, 8)
        {
        }
        public override void Configure(EntityMetadataTableBuilder<TodoItem> builder)
        {
            builder.ShardingProperty(x => x.Id);
        }
    }
}
