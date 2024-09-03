using FluentValidation;

namespace Application.UseCase.Customers.Command.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name must not empty")
                .Length(1, 10).WithMessage("Name must within 5 characters");
            RuleFor(c => c.Address)
                .NotEmpty().WithMessage("Address must not empty");
        }
    }
}
