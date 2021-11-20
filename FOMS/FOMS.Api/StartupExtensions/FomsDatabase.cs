namespace FOMS.Api.StartupExtensions
{
    internal static class FomsDatabase
    {
        public static void AddFomsDatabase(this WebApplicationBuilder builder)
        {
            /*var appOptions = configuration.Get<ApplicationOptions>();

            var dbContextOptions = new DbContextOptionsBuilder<UnitOfWork>()
                .UseSqlServer(appOptions.ConnectionStrings.SqlServer)
                .EnableSensitiveDataLogging(webHostEnvironment.IsDevelopment())
                .EnableDetailedErrors(webHostEnvironment.IsDevelopment())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

            services.AddScoped<IUnitOfWork>(x => new UnitOfWork(dbContextOptions.Options));*/
        }
    }
}
