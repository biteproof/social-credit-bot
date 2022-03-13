using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using RottenBot.Domain.Abstractions;

namespace RottenBot.Web.Jobs
{
	public class LimitsJob : IInvocable
	{
		private readonly ILimitsService _limitsService;
		private readonly ILogger<LimitsJob> _logger;

		public LimitsJob(ILimitsService limitsService, ILogger<LimitsJob> logger)
		{
			_limitsService = limitsService;
			_logger = logger;
		}

		public async Task Invoke()
		{
			_logger.LogInformation("Start updating limits");
			await _limitsService.UpdateAllLimits();
			_logger.LogInformation("Finished updating limits");
		}
	}
}
