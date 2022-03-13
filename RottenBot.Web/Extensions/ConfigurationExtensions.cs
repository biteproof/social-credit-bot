using Microsoft.Extensions.Configuration;
using RottenBot.DataAccess.Postgres.Configurations;
using RottenBot.Domain.Configurations;

namespace RottenBot.Web.Extensions
{
	public static class ConfigurationsExtensions
	{
		public static PostgresOptions GetPostgresOptions(this IConfiguration configuration, string route)
		{
			var options = configuration.GetSection(route).Get<PostgresOptions>();
			var validator = new PostgresOptionsValidator();
			validator.ValidateConfigurationAndThrow(options);
			return options;
		}

		public static BotOptions GetBotOptions(this IConfiguration configuration, string route)
		{
			var options = configuration.GetSection(route).Get<BotOptions>();
			var validator = new BotOptionsValidator();
			validator.ValidateConfigurationAndThrow(options);
			return options;
		}
	}
}
