using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RottenBot.Domain.Abstractions;
using RottenBot.Domain.Repositories;

namespace RottenBot.DataAccess.Postgres
{
	public static class DataAccessExtensions
	{
		public static IServiceCollection AddDataAccessPostgres(this IServiceCollection services, string connectionStr)
		{
			return services
				.AddTransient<IDbConnection>(_ => new NpgsqlConnection(connectionStr))
				.AddTransient<IDbMigrationService, DbMigrationService>()
				.AddTransient<ISocialCreditRepository, SocialCreditRepository>();
		}
	}
}
