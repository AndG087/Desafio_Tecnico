// Agenda.Application/Contacts/Validators/CreateContactValidator.cs
using FluentValidation;

public class CreateContactValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(120);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(180);
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\+?\d[\d\s\-]{7,}$").WithMessage("Telefone inválido");
    }
}
