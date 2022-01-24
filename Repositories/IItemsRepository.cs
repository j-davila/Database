using Database.Entities;

namespace Database.Repositories
{
    public interface IItemsRepository
    {
        Task<Item> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateItemAsync(Item item); // receives the item that needs to be created in the repository
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);
    }
}