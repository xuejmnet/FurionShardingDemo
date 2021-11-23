using Furion.DatabaseAccessor;

namespace FurionShardingDemo.EfCores
{
    public class TodoItem:IPrivateEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
