using AutoMapper;
using Domain.Models.Response;
using Domain.Repository.UnitOfWork;
using Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Domain.Entities;
using FluentValidation;

namespace Application.UseCase.Customers.Command.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserRepo userRepo)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var existUsers = await _userRepo.GetByEmail(request.Name);
            if (existUsers != null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = "Already existed",
                    Data = null,
                };
            }

            var newCustomer = new Customer
            {
                Name = request.Name,
                Address = request.Address,
            };

            _userRepo.AddCustomer(newCustomer);
            await _unitOfWork.SaveChangesAsync();
            var updatedUsers = _mapper.Map<CustomerResponseDto>(existUsers);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Successfull",
                Data = updatedUsers,
            };
        }
    }
}
