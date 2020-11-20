using MoesTavern.Data;
using MoesTavern.Api.Schema;
using GraphQL.Server;
using GraphQL.Server.Ui.Altair;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MoesTavern.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHttpContextAccessor()        
                .LoadDatabase()
                .AddSingleton<IInventorySchema, InventorySchema>()
                .AddSingleton<IInventoryQuery, InventoryQuery>()
                .AddSingleton<IInventoryMutation, InventoryMutation>()
                .AddGraphQL((options, provider) =>
                {
                    options.EnableMetrics = false;
                })
                .AddSystemTextJson()
                .AddGraphTypes(typeof(InventorySchema))
                .AddDataLoader();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseGraphQL<IInventorySchema>("/graphql");

            app.UseGraphQLAltair(new GraphQLAltairOptions
            {
                Path = "/ui/altair",
                GraphQLEndPoint = "/graphql"
            });
        }
    }
}