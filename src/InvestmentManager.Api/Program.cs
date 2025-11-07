using Amazon;
using Amazon.DynamoDBv2;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Application.Services;
using InvestmentManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Controllers
builder.Services.AddControllers();

// Repositories
builder.Services.AddScoped<IFundRepository, FundRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

// Services
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IFundService, FundService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// AWS DynamoDB
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var environment = builder.Environment.EnvironmentName;

    var useLocalDynamo = configuration.GetValue<bool>("DynamoDB:UseLocal");
    var region = configuration.GetValue<string>("DynamoDB:Region");
    var serviceUrl = configuration.GetValue<string>("DynamoDB:ServiceURL");

    var config = new AmazonDynamoDBConfig
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(region)
    };

    if (useLocalDynamo && !string.IsNullOrEmpty(serviceUrl))
    {
        config.ServiceURL = serviceUrl;
    }

    return new AmazonDynamoDBClient(config);
});


var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
