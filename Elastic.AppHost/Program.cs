var builder = DistributedApplication.CreateBuilder(args);

var passwordElastic = builder.AddParameter("passwordElastic", secret: true);

var elasticsearch = builder.AddElasticsearch("elasticsearch", password: passwordElastic)
    .WithDataVolume()
    .RunElasticWithHttpsDevCertificate(port: 9200);

//var apiService = builder.AddProject<Projects.Elastic_ApiService>("apiservice");

//builder.AddProject<Projects.Elastic_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(apiService);

builder.AddProject<Projects.WebGeoElasticsearch>("webgeoelasticsearch");

//var apiService = builder.AddProject<Projects.Elastic_ApiService>("apiservice");

//builder.AddProject<Projects.Elastic_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(apiService);

builder.Build().Run();
