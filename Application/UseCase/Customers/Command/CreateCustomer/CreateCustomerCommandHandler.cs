using AutoMapper;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repository;
using Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

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

            await _userRepo.AddCustomer(newCustomer);

            var updatedUsers = _mapper.Map<CustomerResponseDto>(newCustomer);
            var check = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = check > 0 ? "Successfull" : "Failed",
                Data = updatedUsers,
            };

        }
    }
}
