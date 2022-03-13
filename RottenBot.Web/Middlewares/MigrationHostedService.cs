using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RottenBot.Domain.Abstractions;

namespace RottenBot.Web.Middlewares
{
	public class MigrationHostedService : IHostedService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public MigrationHostedService(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			EnsureDatabaseMigrated();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private void EnsureDatabaseMigrated()
		{
			using var scope = _serviceScopeFactory.CreateScope();
			var migrationService = scope.ServiceProvider.GetRequiredService<IDbMigrationService>();
			migrationService.MigrationForAuthService();
		}
	}
}
