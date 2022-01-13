// class that receives request from client and handles the request.
using Microsoft.AspNetCore.Mvc;
using Database.Repositories;
using Database.Entities;
using Database.Dtos;

namespace Database.Controllers
{
    [ApiController]
    [Route("[controller]")] // which http route this controller will be respoding
    public class ItemsController : ControllerBase // always inherit from controllerbase to convert class to controller class
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        // method is invoked when getting an item from the list. 
        // Get /items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        // Get /items/id
        [HttpGet("{id}")]
        // Action result lets you return more than one type from the method
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id);

            if(item is null)
            {
                return NotFound();
            }

            return item.AsDto();
        }     

        // Post / items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(item);

            // returns the item that was created and header for information on the item
            return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto()); 
        }  

        // PUT /items/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItem(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            // copy of existing item with the two properties
            Item updatedItem = existingItem with {

                   Name = itemDto.Name,
                   Price = itemDto.Price
            };

            repository.UpdateItem(updatedItem);

            return NoContent(); // nothing is returned, item is updated
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = repository.GetItem(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            repository.DeleteItem(id);

            return NoContent();
        }
    }
}