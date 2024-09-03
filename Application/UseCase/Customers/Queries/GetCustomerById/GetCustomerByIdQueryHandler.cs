using AutoMapper;
using Domain.Models.Response;
using Domain.Repository;
using Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCase.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepo _userRepo;
        private readonly IMapper mapper;

        public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork, IUserRepo userRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            this.mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepo.GetById(request.Id);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "OK",
                Data = mapper.Map<CustomerResponseDto>(users),
            };
        }
    }
}
