// class that receives request from client and handles the request.
using Microsoft.AspNetCore.Mvc;
using Database.Repositories;
using Database.Entities;
using Database.Dtos;

namespace Database.Controllers
{
    [ApiController]
    [Route("items")] // which http route this controller will be respoding
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
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync())
                .Select(item => item.AsDto());
            return items;
        }

        // Get /items/id
        [HttpGet("{id}")]
        // Action result lets you return more than one type from the method
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);

            if(item is null)
            {
                return NotFound();
            }

            return item.AsDto();
        }     

        // Post / items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);

            // returns the item that was created and header for information on the item
            return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id}, item.AsDto()); 
        }  

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            // copy of existing item with the two properties
            Item updatedItem = existingItem with {

                   Name = itemDto.Name,
                   Price = itemDto.Price
            };

            await repository.UpdateItemAsync(updatedItem);

            return NoContent(); // nothing is returned, item is updated
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}