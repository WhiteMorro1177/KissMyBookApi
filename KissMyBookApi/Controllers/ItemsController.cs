using KissMyBookApi.Helpers;
using KissMyBookApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KissMyBookApi.Controllers
{
    [Route("/items")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly ILogger<ItemsController> _logger;


		public ItemsController(ILogger<ItemsController> logger) { 
			_logger = logger;
		}


		// get list of items
		[HttpGet]
		public async Task<ActionResult<List<Item>>> GetItems(
			[FromQuery(Name = "title")] string? titlePart
			) 
		{
			if (titlePart != null) 
				return await ItemDbContext.Instance.GetItemsByTitlePart(titlePart);
			return await ItemDbContext.Instance.GetItems();
		}

		// get one item by id
		[HttpGet("{id}")]
		public async Task<ActionResult<Item>> GetItemById(int id)
		{
			ItemSelectFilter filter = new ItemSelectFilter();
			filter.Id = id;

			List<Item> items = await ItemDbContext.Instance.GetItems(filter);

            Debug.WriteLine(items.Count);

			return items.First();
			
		}

		// create new item
		[HttpPost]
		public async Task<ActionResult<string>> AddItem(Item item)
		{
			Item addedItem = await ItemDbContext.Instance.SetItem(item);
			return addedItem.Id.ToString();
		}

		// update one item
		[HttpPut("{id}")]
		public async Task<ActionResult<string>> UpdateItem(int id, Item update)
		{
			string updateResult = await ItemDbContext.Instance.UpdateItem(id, update);
			return updateResult;
		}

		// delete item
		[HttpDelete("{id}")]
		public async Task<ActionResult<string>> DeleteItem(int id)
		{
			string deletedResult = await ItemDbContext.Instance.DeleteItem(id);
			return deletedResult;
		}




		// get list of filtered items
/*		[HttpGet()]
		public async Task<ActionResult<List<Item>>> GetFilteredItems([FromQuery(Name = "filter")] params string[] filters)
		{
			List<Item> items = await ItemDbContext.Instance.GetItemsByTitlePart(filter);
			return items;
		}*/
	}
}
