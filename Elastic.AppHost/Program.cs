using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var passwordElastic = builder.AddParameter("passwordElastic", secret: true);

var elasticsearch = builder.AddElasticsearch("elasticsearch", password: passwordElastic)
    .WithDataVolume()
    .RunElasticWithHttpsDevCertificate(port: 9200);

builder.AddProject<Projects.WebGeoElasticsearch>("webgeoelasticsearch")
    .WithReference(elasticsearch);

builder.Build().Run();
