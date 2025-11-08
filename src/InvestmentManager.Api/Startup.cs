using Amazon;
using Amazon.DynamoDBv2;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Application.Services;
using InvestmentManager.Infrastructure.Repositories;

namespace InvestmentManager.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Controllers
            services.AddControllers();

            // Repositories
            services.AddScoped<IFundRepository, FundRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();

            // Services
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IFundService, FundService>();
            services.AddScoped<ITransactionService, TransactionService>();

            // AWS DynamoDB
            services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var useLocalDynamo = configuration.GetValue<bool>("DynamoDB:UseLocal");
                var region = configuration.GetValue<string>("DynamoDB:Region") ?? "us-east-1";
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

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowFrontend");

            // Swagger solo en desarrollo
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}