using CIS.Infrastructure.StartupExtensions;

namespace NOBY.Api.StartupExtensions
{
    internal static class NobyDatabase
    {
        public static WebApplicationBuilder AddNobyDatabase(this WebApplicationBuilder builder)
        {
            /*var appOptions = configuration.Get<ApplicationOptions>();

            var dbContextOptions = new DbContextOptionsBuilder<UnitOfWork>()
                .UseSqlServer(appOptions.ConnectionStrings.SqlServer)
                .EnableSensitiveDataLogging(webHostEnvironment.IsDevelopment())
                .EnableDetailedErrors(webHostEnvironment.IsDevelopment())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            services.AddScoped<IUnitOfWork>(x => new UnitOfWork(dbContextOptions.Options));*/

            // dapper konsdb
            builder.Services.AddDapper<NOBY.Core.IKonsdbDapperConnectionProvider>(builder.Configuration.GetConnectionString("konsDb"));
            
            return builder;
        }
    }
}
