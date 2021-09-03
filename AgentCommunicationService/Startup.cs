using AgentCommunicationService.Controllers;
using AgentCommunicationService.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AgentCommunicationService.ContentBuilders;

namespace AgentCommunicationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                // Service Components
                .AddScoped<IAgentComponent, AgentComponent>()
                // Factories
                .AddScoped<TransmissionValidatorFactory>()
                .AddScoped<AcordContentBuilderFactory>()
                // Repositories
                .AddScoped<IAgentRepository, AgentRepository>()
                //Enable CORS
                .AddCors(c =>
                    c.AddPolicy("AllowOrigin", options =>
                    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
                )
                ;
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgentCommunicationService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
                options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgentCommunicationService v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(cfg => cfg.MapControllers());
        }
    }
}
