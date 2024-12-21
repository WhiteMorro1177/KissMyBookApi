using KissMyBookApi.Helpers;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Diagnostics;

namespace KissMyBookApi.Model
{
    internal class ItemDbContext
	{
		// config
		private const string DB_NAME = "test";
		private const string COLLECTION_NAME = "items";

		// "NULL" item
		private readonly Item NULL_ITEM = new Item(-1, "NULL", 0, "Item does not found", "NULL", 0f);

		// fields
		private static ItemDbContext? _instance; // singleton instance of ItemsDbContext

		private MongoClient _client; // connected MongoDB client
		private static IMongoCollection<Item> _collection; // main Items collection

		public static ItemDbContext Instance
		{
			get
			{
				_instance ??= new ItemDbContext();
				return _instance;
			}
		}

		private ItemDbContext()
		{
			string connectionString = "mongodb://admin:password@192.168.142.131:27017";
			_client = new MongoClient(connectionString);
			var db = _client.GetDatabase(DB_NAME);

			var collectionNames = db.ListCollectionNames();
			if (!collectionNames.ToList().Contains(COLLECTION_NAME)) db.CreateCollection(COLLECTION_NAME);

			_collection = db.GetCollection<Item>(COLLECTION_NAME);
		}


		// CRUD operations
		public async Task<List<Item>> GetItems(ItemSelectFilter? itemFilter = null)
		{
			var filter = itemFilter != null ? itemFilter.ToBsonDocument() : new BsonDocument();

			return await _collection.Find(filter).ToListAsync();
		}

		public async Task<List<Item>> GetItemsByTitlePart(string titlePart)
		{
			List<Item> items = await GetItems();
			List<Item> filteredItems = items
				.Where(item => item.Title.Contains(titlePart, StringComparison.CurrentCultureIgnoreCase)).ToList();

			filteredItems.Sort((x, y) => x.Title.CompareTo(y.Title));

			return filteredItems;
		}

		public async Task<Item> SetItem(Item newItem)
		{
			await _collection.InsertOneAsync(newItem);

			List<Item> addedItem = _collection
				.Find(new BsonDocument())
				.ToEnumerable()
				.Where(x => x.Id == newItem.Id)
				.ToList();

			if (addedItem.Count == 1) return addedItem.First();
			else return NULL_ITEM;
		}

		public async Task<string> UpdateItem(int id, Item update)
		{
			ItemSelectFilter filter = new ItemSelectFilter();
			filter.Id = id;
			update.Id = id;

			ReplaceOneResult result = await _collection.ReplaceOneAsync(filter: filter.ToBsonDocument(), update);
			
			return $"Match: {result.MatchedCount}, Modified: {result.ModifiedCount}, ID: {result.UpsertedId}";
		}

		public async Task<string> DeleteItem(int id)
		{
			ItemSelectFilter filter = new ItemSelectFilter();
			filter.Id = id;

			var result = await _collection.DeleteOneAsync(filter.ToBsonDocument());

			return $"Deleted: {result.DeletedCount}";
		}



	}
}
