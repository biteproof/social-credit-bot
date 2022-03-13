using System;

namespace RottenBot.Domain.Models
{
	public sealed class User
	{
		public long Id { get; set; }
		public string Username { get; set; }
		public int Rating { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public long ChatId { get; set; }
	}
}
