using System.Collections.Generic;
using System.Threading.Tasks;

namespace RottenBot.Domain.Abstractions
{
	public interface ILimitsService
	{
		public Task UpdateLimit(long userId, int ratingModifier);
		public Task<bool> IsUserReachedLimit(long userId);
		public Task<int> GetAvailableLimit(long userId);
		public Task<IEnumerable<int>> GetUserIds();
		public Task UpdateAllLimits();
	}
}
