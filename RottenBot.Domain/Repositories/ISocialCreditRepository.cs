using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using RottenBot.Domain.Models;
using User = RottenBot.Domain.Models.User;

namespace RottenBot.Domain.Repositories
{
	public interface ISocialCreditRepository
	{
		/// <summary>
		/// Returns overall user rating
		/// </summary>
		public Task<int?> GetOverallUserRatingByName(string userName);

		/// <summary>
		/// Returns social credit ranks for given chat 
		/// </summary>
		public Task<IEnumerable<Rank>> GetRanks(long chatId);

		/// <summary>
		/// Returns all user information for given chat
		/// </summary>
		public Task<User> GetChatUserById(long userId, long chatId);

		/// <summary>
		/// Returns user social credit for given chat 
		/// </summary>
		public Task<int?> GetChatUserRatingByName(string userName, long chatId);

		/// <summary>
		/// Upsert user information to db 
		/// </summary>
		public Task UpsertChatUser(User user);

		public Task<int?> GetLimit(long userId);
		public Task UpsertLimit(long userId, int availableLimit);
		public Task<IEnumerable<long>> GetUserIds();
		public Task UpdateAllLimits(int limitValue);
	}
}
