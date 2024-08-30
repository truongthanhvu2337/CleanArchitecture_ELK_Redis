using Domain.Entities;

namespace Infrastructure.Elasticsearch
{
    public interface IElasticService
    {
        Task<bool> IndexDocumentAsync(Customer customer);
        Task<bool> BulkIndexDocumentsAsync(IEnumerable<Customer> customer/*, string indexName*/);
        Task<Customer> Get(string key);
        Task<IEnumerable<Customer>?> GetAll();
        Task<bool> Remove(string key);
        Task<long?> RemoveAll();
    }
}

