using AutoMapper;
using Domain.Models.Response;
using Domain.Repository;
using Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCase.Customers.Queries.GetAllByPagination
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllCustomerQueryByPagination, APIResponse>
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IMapper mapper, IUserRepo userRepo)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetAllCustomerQueryByPagination request, CancellationToken cancellationToken)
        {
            var users = await _userRepo.Pagination(request.Page, request.EachPage);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "OK",
                Data = _mapper.Map<IEnumerable<CustomerResponseDto>>(users),
            };
        }
    }

}
