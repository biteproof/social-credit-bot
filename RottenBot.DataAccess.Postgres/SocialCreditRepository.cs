using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using RottenBot.DataAccess.Postgres.Configurations;
using RottenBot.DataAccess.Postgres.Queries;
using RottenBot.Domain.Models;
using RottenBot.Domain.Repositories;
using User = RottenBot.Domain.Models.User;

namespace RottenBot.DataAccess.Postgres
{
	public class SocialCreditRepository : ISocialCreditRepository, IDisposable
	{
		private readonly IDbConnection _pgConnection;

		public SocialCreditRepository(IDbConnection pgConnection)
		{
			if (pgConnection.State != ConnectionState.Open)
			{
				throw new ApplicationException("Postgres connection is closed");
			}

			_pgConnection = pgConnection;
		}

		public async Task<int?> GetOverallUserRatingByName(string userName) =>
			await _pgConnection.QueryFirstOrDefaultAsync<int?>(ScQueries.SelectOverallUserRatingByName,
				new { Username = userName });

		public async Task<IEnumerable<Rank>> GetRanks(long chatId) =>
			await _pgConnection.QueryAsync<Rank>(ScQueries.SelectChatRanks, new { ChatId = chatId });

		public async Task<User> GetChatUserById(long userId, long chatId)
		{
			var res = await _pgConnection.QueryAsync<User>(ScQueries.SelectChatUserById,
				new { Id = userId, ChatId = chatId });
			return res.FirstOrDefault();
		}

		public async Task<int?> GetChatUserRatingByName(string userName, long chatId)
		{
			if (chatId != default)
			{
				return await _pgConnection.QueryFirstOrDefaultAsync<int?>(ScQueries.SelectChatUserRatingByName,
					new { Username = userName, ChatId = chatId });
			}

			return default;
		}

		public async Task UpsertChatUser(User user)
		{
			await _pgConnection.QueryAsync(ScQueries.UpsertChatUser,
				new
				{
					Id = user.Id,
					Username = user.Username,
					Rating = user.Rating,
					CreatedAt = user.CreatedAt,
					UpdatedAt = user.UpdatedAt,
					ChatId = user.ChatId
				});
		}

		public async Task<int?> GetLimit(long userId) =>
			await _pgConnection.QueryFirstOrDefaultAsync<int?>(LimitsQueries.GetUserLimit, new { UserId = userId });

		public async Task UpsertLimit(long userId, int availableLimit) =>
			await _pgConnection.ExecuteAsync(LimitsQueries.UpsertUserLimit,
				new { UserId = userId, Limit = availableLimit, UpdatedAt = DateTime.UtcNow.Date });

		public async Task<IEnumerable<long>> GetUserIds() =>
			await _pgConnection.QueryAsync<long>(LimitsQueries.GetUserIds);

		public async Task UpdateAllLimits(int limitValue) =>
			await _pgConnection.ExecuteAsync(LimitsQueries.UpdateAllLimitsFromValue,
				new { ScLimit = limitValue, UpdatedAt = DateTime.UtcNow });

		public void Dispose()
		{
			_pgConnection?.Dispose();
		}
	}
}
