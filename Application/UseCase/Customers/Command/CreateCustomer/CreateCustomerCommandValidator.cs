using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.Customers.Command.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name must not empty")
                .Length(5).WithMessage("Name must within 5 characters");
            RuleFor(c => c.Address)
                .NotEmpty().WithMessage("Address must not empty");
        }
    }
}
