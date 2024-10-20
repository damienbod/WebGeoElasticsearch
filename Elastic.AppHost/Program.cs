var builder = DistributedApplication.CreateBuilder(args);

var passwordElastic = builder.AddParameter("passwordElastic", secret: true);

var elasticsearch = builder.AddElasticsearch("elasticsearch", password: passwordElastic)
    .WithDataVolume()
    .RunElasticWithHttpsDevCertificate(port: 9200);

builder.AddProject<Projects.WebGeoElasticsearch>("webgeoelasticsearch")
    .WithExternalHttpEndpoints()
    .WithReference(elasticsearch);
    //.WaitFor(elasticsearch);

builder.Build().Run();
