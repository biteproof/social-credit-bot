using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace RottenBot.Web
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection,
			string token)
		{
			return serviceCollection.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(token));
		}
	}
}
