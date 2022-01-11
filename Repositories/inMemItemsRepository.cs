using Database.Entities;

namespace Database.Repositories
{  
    public class InMemItemsRepository
    {   private readonly List<Item> items = new()
        {
            new Item {Id = Guid.NewGuid(), Name = "Potion of Strength", Price = 15, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id = Guid.NewGuid(), Name = "Silver Katana", Price = 80, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id = Guid.NewGuid(), Name = "Potion of invisibility", Price = 25, CreatedDate = DateTimeOffset.UtcNow}
        }; 

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItem(Guid id)
        {
            return items.Where(item => item.Id == id).SingleOrDefault(); // returns item id if found or null if nothing found
        }
    }
}