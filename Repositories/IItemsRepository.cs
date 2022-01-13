using Database.Entities;

namespace Database.Repositories
{
    public interface IItemsRepository
    {
        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();
        void CreateItem(Item item); // receives the item that needs to be created in the repository
        void UpdateItem(Item item);
        void DeleteItem(Guid id);
    }
}