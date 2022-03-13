using FluentValidation;

namespace RottenBot.DataAccess.Postgres.Configurations;

public sealed class PostgresOptionsValidator : AbstractValidator<PostgresOptions>
{
	public PostgresOptionsValidator()
	{
		RuleFor(_ => _.Host)
			.NotEmpty()
			.WithMessage($"Postgres {nameof(PostgresOptions.Host)} is required.");

		RuleFor(_ => _.Port)
			.NotEmpty()
			.WithMessage($"Postgres {nameof(PostgresOptions.Port)} is required.");


		RuleFor(_ => _.Database)
			.NotEmpty()
			.WithMessage($"Postgres {nameof(PostgresOptions.Database)} is required.");


		RuleFor(_ => _.Username)
			.NotEmpty()
			.WithMessage($"Postgres {nameof(PostgresOptions.Username)} is required.");


		RuleFor(_ => _.Password)
			.NotEmpty()
			.WithMessage($"Postgres {nameof(PostgresOptions.Password)} is required.");
	}
}
