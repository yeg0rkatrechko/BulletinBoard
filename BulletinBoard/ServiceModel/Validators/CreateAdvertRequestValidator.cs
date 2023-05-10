using FluentValidation;

namespace BulletinBoard.ServiceModel.Validators
{
    public class CreateAdvertRequestValidator : AbstractValidator<CreateAdvertRequest>
    {
        public CreateAdvertRequestValidator()
        {
            int minLength = 10;

            int maxLength = 500;

            RuleFor(a => a.Text)
                .MinimumLength(minLength)
                .WithMessage($"Текст должен быть длиннее {minLength} символов")
                .MaximumLength(maxLength)
                .WithMessage($"Текст не может быть длиннее {maxLength} символов");
            
            // todo validate Heading
        }
    }
}