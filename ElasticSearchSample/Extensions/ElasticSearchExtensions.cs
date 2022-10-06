using Elasticsearch.Net;
using ElasticSearchSample.Models;
using Nest;

namespace ElasticSearchSample.Extensions
{
    public static class ElasticSearchExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            //var url = "http://localhost:9200";//configuration["ELKConfiguration:url"];
            //var defaultIndex = "Book";//configuration["ELKConfiguration:index"];

            //var settings = new ConnectionSettings(new Uri(url)).BasicAuthentication("elastic", "GjFGec3oGklhOwbd+Ezj")
            //    .PrettyJson()
            //    .DefaultIndex(defaultIndex);

            //AddDefaultMappings(settings);

            //var client = new ElasticClient(settings);

            //services.AddSingleton<IElasticClient>(client);

            //CreateIndex(client, defaultIndex);


            var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));
            var defaultIndex = "book";
            var settings = new ConnectionSettings(pool)
                .DefaultIndex(defaultIndex)
                .CertificateFingerprint("5a04f415968426b7d1786e74c2c042fc8157fe145c126767cf5158deff424712")
                .BasicAuthentication("elastic", "GjFGec3oGklhOwbd+Ezj")
                .EnableApiVersioningHeader();

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<Book>(m => m
                    .Ignore(p => p.Status)
                    .Ignore(p => p.Authors)
                );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<Book>(x => x.AutoMap())
            );
        }
    }
}
