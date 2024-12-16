using KissMyBookApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace KissMyBookApi.Controllers
{
	[Route("api/v1/items")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly ILogger<ItemsController> _logger;

		public ItemsController(ILogger<ItemsController> logger) { 
			_logger = logger;
		}


		// endpoints here
		[HttpGet]
		public async Task<ActionResult<List<Item>>> GetItems()
		{
			List<Item> itemsFromDb = await ItemDbContext.Instance.GetItems();
			return itemsFromDb;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Item>> GetItemById(long id)
		{
			return new Item();
		}

		[HttpPost]
		public async Task<ActionResult<Item>> AddItem(Item item)
		{
			return CreatedAtAction(nameof(Item), new { id = item.Id }, item);
		}
	}
}
