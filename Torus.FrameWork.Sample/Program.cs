using Torus.Framework.Core.MultiTenancy;
using Torus.FrameWork.Sample.Tenants;

namespace Torus.FrameWork.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMultiTenancy().AddTenantIdResolver<QueryStringTenantIdResolver>(sp =>
            {
                return new QueryStringTenantIdResolver(sp, "tid");
            }).AddTenantStore<InMemTenantStore>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseMultiTenancy();

            app.MapControllers();

            app.Run();
        }
    }
}
