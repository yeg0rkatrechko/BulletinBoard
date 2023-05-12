using FluentValidation;
using Microsoft.Extensions.Options;
using Services.Options;

namespace BulletinBoard.ServiceModel.Validators
{
    public class CreateAdvertRequestValidator : AbstractValidator<CreateAdvertRequest>
    {
        private TextOptions _options;
        public CreateAdvertRequestValidator(IOptions<TextOptions> options)
        {
            _options = options.Value;
            
            RuleFor(a => a.Text)
                .MinimumLength(_options.MinTextLength)
                .WithMessage($"Текст должен быть длиннее {_options.MinTextLength} символов")
                .MaximumLength(_options.MaxTextLength)
                .WithMessage($"Текст не может быть длиннее {_options.MaxTextLength} символов")
                .When(a => a.Text != null);
            RuleFor(a => a.Heading)
                .MinimumLength(_options.MinHeadingLength)
                .WithMessage(($"Заголовок должен быть длиннее {_options.MinHeadingLength} символов"))
                .MaximumLength(_options.MaxHeadingLength)
                .WithMessage(($"Заголовок не может быть длиннее {_options.MaxHeadingLength} символов"))
                .When(a => a.Text != null);
        }
    }
}