using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Elasticsearch.Setting
{
    public class ElasticSetting
    {
        public string Url { get; set; } = string.Empty;
        public string? DefaultIndex { get; set; }
    }
}
