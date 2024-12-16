using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace KissMyBookApi.Model
{
	internal class ItemDbContext
	{
		// config
		private const string DB_NAME = "test";
		private const string COLLECTION_NAME = "items";

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
			string connectionString = "mongodb://10.241.167.184:27017";
			_client = new MongoClient(connectionString);
			var db = _client.GetDatabase(DB_NAME);

			var collectionNames = db.ListCollectionNames();
			if (!collectionNames.ToList().Contains(COLLECTION_NAME)) db.CreateCollection(COLLECTION_NAME);

			_collection = db.GetCollection<Item>(COLLECTION_NAME);
		}


		// CRUD operations
		public async Task<List<Item>> GetItems()
		{
			return await _collection.Find("{}").ToListAsync();
		}


	}
}
