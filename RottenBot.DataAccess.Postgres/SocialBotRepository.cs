using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using RottenBot.DataAccess.Postgres.Queries;
using RottenBot.Domain.Models;
using RottenBot.Domain.Repositories;
using User = RottenBot.Domain.Models.User;

namespace RottenBot.DataAccess.Postgres
{
	public class SocialBotRepository : ISocialBotRepository, IDisposable
	{
		private readonly IDbConnection _pgConnection;

		public SocialBotRepository(IDbConnection pgConnection)
		{
			if (pgConnection.State != ConnectionState.Open)
			{
				throw new ApplicationException("Postgres connection is closed");
			}

			_pgConnection = pgConnection;
		}

		public async Task<User> GetUserById(int userId)
		{
			var res = await _pgConnection.QueryAsync<User>(ScQueries.SelectChatUserById,
				new { Id = userId });
			return res.FirstOrDefault();
		}

		public async Task<int> GetUserRatingByName(string userName) =>
			await _pgConnection.QueryFirstOrDefaultAsync<int>(ScQueries.SelectChatUserRatingByName,
				new { Username = userName });

		public async Task<IEnumerable<Rank>> GetRanks(long chatId) =>
			await _pgConnection.QueryAsync<Rank>(ScQueries.SelectChatRanks, new { ChatId = chatId });

		public async Task UpsertUser(User user)
		{
			await _pgConnection.QueryAsync(ScQueries.UpsertChatUser,
				new
				{
					Id = user.Id,
					Username = user.Username,
					Rating = user.Rating,
					CreatedAt = user.CreatedAt,
					UpdatedAt = user.UpdatedAt,
				});
		}

		public async Task<User> GetChatUserById(int userId, long chatId)
		{
			var res = await _pgConnection.QueryAsync<User>(ScQueries.SelectChatUserById,
				new { Id = userId, ChatId = chatId });
			return res.FirstOrDefault();
		}

		public async Task<int> GetChatUserRatingByName(string userName, long chatId) =>
			await _pgConnection.QueryFirstOrDefaultAsync<int>(ScQueries.SelectChatUserRatingByName,
				new { Username = userName, ChatId = chatId });

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

		public void Dispose()
		{
			_pgConnection?.Dispose();
		}
	}
}
