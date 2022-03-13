using FluentValidation;

namespace RottenBot.Web.Extensions
{
	public static class OptionsValidatorExtensions
	{
		public static void ValidateConfigurationAndThrow<T>(this AbstractValidator<T> validator, T model)
			where T : class
		{
			if (model == null)
			{
				throw new ValidationException($"The required configurations {typeof(T).Name} were not specified.");
			}

			validator.ValidateAndThrow(model);
		}
	}
}
