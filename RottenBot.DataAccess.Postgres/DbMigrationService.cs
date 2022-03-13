using System.Data;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Logging;
using RottenBot.Domain.Abstractions;

namespace RottenBot.DataAccess.Postgres
{
	public sealed class DbMigrationService : IDbMigrationService
	{
		private readonly IDbConnection _dbConnection;
		private readonly ILogger<DbMigrationService> _logger;

		public DbMigrationService(IDbConnection dbConnection, ILogger<DbMigrationService> logger)
		{
			_dbConnection = dbConnection;
			_logger = logger;
		}

		public void MigrationForAuthService()
		{
			EnsureDatabase.For.PostgresqlDatabase(_dbConnection.ConnectionString);
			var migrator = DeployChanges.To
				.PostgresqlDatabase(_dbConnection.ConnectionString)
				.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
				.WithVariablesDisabled()
				.WithTransactionPerScript()
				.LogToConsole()
				.Build();

			var result = migrator.PerformUpgrade();
			if (!result.Successful)
			{
				throw result.Error;
			}
		}
	}
}
