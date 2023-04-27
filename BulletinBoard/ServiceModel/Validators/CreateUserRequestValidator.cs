using BulletinBoard.ServiceModel;
using FluentValidation;

namespace Models.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            int minLength = 5;
            RuleFor(u => u.Name)
                .MinimumLength(minLength)
                .WithMessage($"Имя пользователя не может быть короче {minLength} символов");
        }
    }
}
