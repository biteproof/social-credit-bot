using System.Collections.Generic;
using System.Threading.Tasks;
using RottenBot.Domain.Models;
using User = RottenBot.Domain.Models.User;

namespace RottenBot.Domain.Repositories
{
	public interface ISocialBotRepository
	{
		// delete this methods sometime
		public Task<User> GetUserById(int userId);
		public Task<int> GetUserRatingByName(string userName);
		public Task<IEnumerable<Rank>> GetRanks(long chatId);
		public Task UpsertUser(User user);
		
		// actual methods
		public Task<User> GetChatUserById(int userId, long chatId);
		public Task<int> GetChatUserRatingByName(string userName, long chatId);
		public Task UpsertChatUser(User user);
	}
}
