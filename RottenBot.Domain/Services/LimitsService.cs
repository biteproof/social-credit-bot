using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RottenBot.Domain.Abstractions;
using RottenBot.Domain.Repositories;

namespace RottenBot.Domain.Services
{
	public sealed class LimitsService : ILimitsService
	{
		private readonly ISocialCreditRepository _repository;
		private const int MaxLimit = 200;

		public LimitsService(ISocialCreditRepository repository)
		{
			_repository = repository;
		}

		public async Task UpdateLimit(long userId, int ratingModifier)
		{
			ratingModifier = Math.Abs(ratingModifier);

			var availableLimit = await _repository.GetLimit(userId);

			if (availableLimit == null) // user is not added to 'limits' yet
			{
				await _repository.UpsertLimit(userId, MaxLimit - ratingModifier);
				return;
			}

			if (availableLimit.Value <= ratingModifier)
			{
				await _repository.UpsertLimit(userId, 0);
			}
			else
			{
				await _repository.UpsertLimit(userId, availableLimit.Value - ratingModifier);
			}
		}

		public async Task<bool> IsUserReachedLimit(long userId)
		{
			var availableLimit = await _repository.GetLimit(userId);

			if (!availableLimit.HasValue) return false; // user is not added to 'limits' yet

			return availableLimit.Value <= 0;
		}

		public async Task<int> GetAvailableLimit(long userId) => await _repository.GetLimit(userId) ?? MaxLimit;

		public async Task<IEnumerable<int>> GetUserIds()
		{
			throw new System.NotImplementedException();
		}

		public async Task UpdateAllLimits() => await _repository.UpdateAllLimits(MaxLimit);
	}
}
