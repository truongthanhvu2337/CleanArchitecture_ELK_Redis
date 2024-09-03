using Domain.Models.Response;
using MediatR;

namespace Application.UseCase.Customers.Queries.GetAllByPagination
{
    public class GetAllCustomerQueryByPagination : IRequest<APIResponse>
    {
        public int Page { get; }
        public int EachPage { get; }

        public GetAllCustomerQueryByPagination(int page, int eachPage)
        {
            Page = page;
            EachPage = eachPage;
        }
    }
}
