using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace RottenBot.Domain.Abstractions
{
	public interface ICommandService
	{
		bool ContainsCommandFor(Message message);
		bool IsMessageContainsRatingSticker(string fileUniqueId);

		Task ExecuteStickerEvent(Message message);
		Task ExecuteCommandEvent(Message message);
	}
}
