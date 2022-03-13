using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RottenBot.Domain.Abstractions;
using RottenBot.Domain.Models;
using RottenBot.Domain.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = RottenBot.Domain.Models.User;

namespace RottenBot.Domain.Services
{
	public sealed class CommandService : ICommandService
	{
		private readonly List<string> _commands = new()
		{
			"/help",
			"/rating",
			"/rating@GnillBot",
			"/orating",
			"/orating@GnillBot",
			"/ranks",
			"/ranks@GnillBot",
			"/limits",
			"/limits@GnillBot"
		};

		private readonly Dictionary<string, int> _stickersDictionary =
			new()
			{
				{ "AQAD0A0AAgIJOUhy", 5 },
				{ "AQADqw4AAkAHOEhy", 20 },
				{ "AQADUgsAAuTYOEhy", 50 },
				{ "AQAD1g0AAnK4MUhy", -5 },
				{ "AQADqg4AAinPMUhy", -20 },
				{ "AQADRQwAAu_DOEhy", -50 }
			};

		private readonly ITelegramBotClient _botClient;
		private readonly ISocialCreditRepository _repository;
		private readonly ILimitsService _limitsService;

		public CommandService(ITelegramBotClient botClient, ISocialCreditRepository repository,
			ILimitsService limitsService)
		{
			_botClient = botClient;
			_repository = repository;
			_limitsService = limitsService;
		}

		public bool ContainsCommandFor(Message message)
		{
			if (message.Type == MessageType.Text && message.Text.Length >= 5)
			{
				// todo add more validation
				var command = message.Text.Split(" ").FirstOrDefault();
				return _commands.Contains(command);
			}

			return false;
		}

		public bool IsMessageContainsRatingSticker(string fileUniqueId) =>
			_stickersDictionary.ContainsKey(fileUniqueId);

		public async Task ExecuteStickerEvent(Message message)
		{
			if (message.From.Id == message.ReplyToMessage.From.Id || message.ReplyToMessage.From.IsBot
			                                                      || message.From.IsBot) return;

			var targetUser = message.ReplyToMessage.From;

			var ratingModifier = _stickersDictionary
				.First(i => i.Key == message.Sticker.Thumb.FileUniqueId)
				.Value;

			var availableLimit = await _limitsService.GetAvailableLimit(message.From.Id);
			if (availableLimit == 0)
			{
				await _botClient.SendTextMessageAsync(message.Chat.Id,
					$"@{message.From.Username}, it's time to stop! You've reached your SC limit for today");
				return;
			}

			if (availableLimit <= Math.Abs(ratingModifier))
			{
				await UpdateUserRating(targetUser.Id, message.Chat.Id, targetUser.Username,
					ratingModifier < 0 ? -availableLimit : availableLimit);
			}
			else
			{
				await UpdateUserRating(targetUser.Id, message.Chat.Id, targetUser.Username, ratingModifier);
			}

			await _limitsService.UpdateLimit(message.From.Id, ratingModifier);
		}

		public async Task ExecuteCommandEvent(Message message)
		{
			var chatId = message.Chat.Id;
			var splitMessage = message.Text.Split(" ");

			if (splitMessage.Length > 2) return;
			if (splitMessage.Length == 2 && !splitMessage.Last().StartsWith("@")) return;

			var command = splitMessage.First();

			string userName = "";

			switch (command)
			{
				case "/rating":
					if (splitMessage.Length < 2) return;
					userName = splitMessage.Last().Replace("@", "");
					await PrintChatUserRating(chatId, userName);
					break;

				case "/rating@GnillBot":
					await PrintChatUserRating(chatId, message.From.Username);
					break;

				case "/orating":
					if (splitMessage.Length < 2) return;
					userName = splitMessage.Last().Replace("@", "");
					await PrintOverallUserRating(chatId, userName);
					break;

				case "/orating@GnillBot":
					await PrintOverallUserRating(chatId, message.From.Username);
					break;

				case "/ranks@GnillBot":
				case "/ranks":
					if (splitMessage.Length > 1) return;
					var ranks = await _repository.GetRanks(chatId);
					await PrintRanks(chatId, ranks);
					break;

				case "/limits":
				case "/limits@GnillBot":
					var limit = await _limitsService.GetAvailableLimit(message.From.Id);
					await _botClient.SendTextMessageAsync(chatId,
						$"@{message.From.Username} sc limit: {limit}");
					break;
			}
		}

		private async Task UpdateUserRating(long userId, long chatId, string userName, int ratingModifier)
		{
			var chatUser = await _repository.GetChatUserById(userId, chatId);
			if (chatUser == null)
			{
				chatUser = new User
				{
					Id = userId,
					Username = userName,
					Rating = ratingModifier,
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					ChatId = chatId,
				};
			}
			else
			{
				// todo check for overflow
				chatUser.Rating += ratingModifier;
				chatUser.UpdatedAt = DateTime.Now;
			}

			await _repository.UpsertChatUser(chatUser);
		}

		private async Task PrintChatUserRating(ChatId chatId, string userName)
		{
			var rating = await _repository.GetChatUserRatingByName(userName, chatId.Identifier ?? default) ?? 0;
			await _botClient.SendTextMessageAsync(chatId, $"@{userName}: {rating}");
		}

		private async Task PrintOverallUserRating(ChatId chatId, string userName)
		{
			var rating = await _repository.GetOverallUserRatingByName(userName) ?? 0;
			await _botClient.SendTextMessageAsync(chatId, $"@{userName} overall rating: {rating}");
		}

		private async Task PrintRanks(ChatId chatId, IEnumerable<Rank> ranks)
		{
			var ranksText = new StringBuilder("Chat ranks:\n");
			var i = 1;
			foreach (var rank in ranks)
			{
				ranksText.Append($"{i++}. {rank.Username}: {rank.Rating}\n");
			}

			await _botClient.SendTextMessageAsync(chatId, ranksText.ToString());
		}

		private async Task GetHelp(Message message)
		{
			var chatId = message.Chat.Id;
			await _botClient.SendTextMessageAsync(chatId, "Help is not found");
		}
	}
}
