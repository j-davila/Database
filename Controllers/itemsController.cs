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
    }
}