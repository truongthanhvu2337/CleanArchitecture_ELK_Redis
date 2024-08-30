using Azure;
using Domain.Entities;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using Infrastructure.Elasticsearch.Setting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Elasticsearch
{
    public class ElasticService : IElasticService
    {
        private readonly ElasticsearchClient _client;
        private readonly ElasticSetting _setting;

        public ElasticService(IOptions<ElasticSetting> options)
        {
            _setting = options.Value;

            var settings = new ElasticsearchClientSettings(new Uri(_setting.Url));
            _client = new ElasticsearchClient(settings);
        }


        public async Task<bool> IndexDocumentAsync(Customer customer)
        {
            var indexResponse = await _client.IndexAsync(customer, idx => idx
                .Index("customer").OpType(OpType.Create));
            return indexResponse.IsValidResponse;
        }

        public async Task<bool> BulkIndexDocumentsAsync(IEnumerable<Customer> customer/*, string indexName*/)
        {
            var response = await _client.BulkAsync(idx => idx.Index("customer")
                    .UpdateMany(customer, (ud, u) => ud.Doc(u).DocAsUpsert(true)));

            return response.IsValidResponse;
        }

        public async Task<Customer> Get(string key)
        {
            var response = await _client.GetAsync<Customer>(key, g => g.Index("customer"));
            return response.Source;
        }

        public async Task<IEnumerable<Customer>?> GetAll()
        {
            var response = await _client.SearchAsync<Customer>(a => a.Index("customer"));

            return response.IsValidResponse ? response.Documents.ToList() : default;
        }

        public async Task<bool> Remove(string key)
        {
            var response = await _client.DeleteAsync<Customer>(key, d => d.Index("customer"));
            return response.IsValidResponse;
        }

        public async Task<long?> RemoveAll()
        {
            var response = await _client.DeleteByQueryAsync<Customer>(d => d.Indices("customer"));
            return response.IsValidResponse ? response.Deleted : default;
        }


    }
}
