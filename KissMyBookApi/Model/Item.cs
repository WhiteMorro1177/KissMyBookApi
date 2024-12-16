using MongoDB.EntityFrameworkCore;

namespace KissMyBookApi.Model
{
	public class Item
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ItemType Type { get; set; }
		public string? Description { get; set; }
		public string Content { get; set; }
		public float Rating { get; set; }

		public Item() { }

		public Item(int id, string name, ItemType type, string? description, string content, float rating)
		{
			Id = id;
			Name = name;
			Type = type;
			Description = description;
			Content = content;
			Rating = rating;
		}

		public enum ItemType
		{
			Movie = 0,
			Book = 1
		}
	}
}
