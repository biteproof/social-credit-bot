using System;
using Coravel;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using RottenBot.DataAccess.Postgres;
using RottenBot.DataAccess.Postgres.Configurations;
using RottenBot.Domain.Abstractions;
using RottenBot.Domain.Configurations;
using RottenBot.Domain.Services;
using RottenBot.Web.Extensions;
using RottenBot.Web.Jobs;
using RottenBot.Web.Middlewares;

namespace RottenBot.Web
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration) => _configuration = configuration;

		public void ConfigureServices(IServiceCollection services)
		{
			var botOptions = _configuration.GetBotOptions("Bot");
			var pgOptions = _configuration.GetPostgresOptions("DataAccess:Postgres");

			services.Configure<BotOptions>(_configuration.GetSection("Bot"));
			services.Configure<PostgresOptions>(_configuration.GetSection("DataAccess:Postgres"));

			services
				.AddHostedService<MigrationHostedService>()
				.AddScoped<ICommandService, CommandService>()
				.AddScoped<ILimitsService, LimitsService>()
				.AddTransient<LimitsJob>()
				.AddScheduler()
				.AddDataAccessPostgres(pgOptions.ConnectionString)
				.AddTelegramBotClient(botOptions.Token)
				.AddControllers()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				})
				.AddFluentValidation();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			app.UseDeveloperExceptionPage();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});

			var provider = app.ApplicationServices;
			provider.UseScheduler(scheduler =>
			{
				scheduler.Schedule<LimitsJob>()
					.DailyAt(21, 0); // (24:00 utc+3)
			});
		}
	}
}
