using KissMyBookApi.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace KissMyBookApi.Helpers
{
	public class ItemUpdateFilter
	{
		[BsonElement("title")]
		[BsonIgnoreIfNull]
		public string? Title { get; set; }

		[BsonElement("type")]
		[BsonIgnoreIfNull]
		public Item.ItemType? Type { get; set; }

		[BsonElement("description")]
		[BsonIgnoreIfNull]
		public string? Description { get; set; }

		[BsonElement("content")]
		[BsonIgnoreIfNull]
		public string Content { get; set; }

		[BsonElement("rating")]
		[BsonIgnoreIfNull]
		public float? Rating { get; set; }

		public ItemUpdateFilter() { }
	}
}
