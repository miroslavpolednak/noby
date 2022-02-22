using CIS.Infrastructure.StartupExtensions;

namespace FOMS.Api.StartupExtensions
{
    internal static class FomsDatabase
    {
        public static WebApplicationBuilder AddFomsDatabase(this WebApplicationBuilder builder)
        {
            /*var appOptions = configuration.Get<ApplicationOptions>();

            var dbContextOptions = new DbContextOptionsBuilder<UnitOfWork>()
                .UseSqlServer(appOptions.ConnectionStrings.SqlServer)
                .EnableSensitiveDataLogging(webHostEnvironment.IsDevelopment())
                .EnableDetailedErrors(webHostEnvironment.IsDevelopment())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            services.AddScoped<IUnitOfWork>(x => new UnitOfWork(dbContextOptions.Options));*/

            // dapper konsdb
            builder.Services.AddDapper<FOMS.Core.IKonsdbDapperConnectionProvider>(builder.Configuration.GetConnectionString("konsDb"));
            
            return builder;
        }
    }
}
