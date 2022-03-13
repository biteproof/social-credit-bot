using FluentValidation;

namespace RottenBot.Domain.Configurations;

public sealed class BotOptionsValidator : AbstractValidator<BotOptions>
{
	public BotOptionsValidator()
	{
		RuleFor(_ => _.Name)
			.NotEmpty()
			.WithMessage($"Bot {nameof(BotOptions.Name)} is required.");

		RuleFor(_ => _.Token)
			.NotEmpty()
			.WithMessage($"Bot {nameof(BotOptions.Token)} is required.");
	}
}
