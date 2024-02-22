using System;
using FluentValidation;

namespace AzureFunctions.Domain
{
    public class GetFileQueryValidator : AbstractValidator<GetFileQuery>
    {
        public GetFileQueryValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("Name must be specified")

                .Length(5, 20)
                .WithMessage("Name must be between 5 and 20 chars");
        }
    }
}

