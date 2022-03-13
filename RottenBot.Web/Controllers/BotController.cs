using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RottenBot.Domain.Abstractions;
using Telegram.Bot.Types;

namespace RottenBot.Web.Controllers
{
	[ApiController]
	[Route("api/message/update")]
	public class BotController : Controller
	{
		private readonly ICommandService _commandService;
		public BotController(ICommandService commandService) => _commandService = commandService;

		// GET api/values
		[HttpGet]
		public IActionResult Get() => Ok("It works");

		// POST api/values
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Update update)
		{
			if (update == null) return Ok();

			var message = update.Message;
			if (message == null) return Ok();

			if (message.Text?.StartsWith("/") ?? false)
			{
				if (_commandService.ContainsCommandFor(message))
				{
					await _commandService.ExecuteCommandEvent(message);
				}
			}
			else if (message.Sticker != null && message.ReplyToMessage != null)
			{
				if (_commandService.IsMessageContainsRatingSticker(message.Sticker.Thumb.FileUniqueId))
				{
					await _commandService.ExecuteStickerEvent(message);
				}
			}

			return Ok();
		}
	}
}
