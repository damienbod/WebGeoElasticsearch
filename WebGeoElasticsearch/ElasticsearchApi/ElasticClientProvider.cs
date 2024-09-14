using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace WebGeoElasticsearch.ElasticsearchApi;

public class ElasticClientProvider
{
    private readonly ElasticsearchClient? _client = null;

    public ElasticClientProvider()
    {
        if (_client == null)
        {
            // TODO read from configuration
            var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
                .Authentication(new BasicAuthentication("elastic", "Password1!"));

            _client = new ElasticsearchClient(settings);
        }
    }
  
    public ElasticsearchClient GetClient()
    {
        if(_client != null)
        {
            return _client;
        }

        throw new Exception("Elasticsearch client not initialized");
    }

}