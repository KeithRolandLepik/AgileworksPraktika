using Domain.Feedbacks;
using Infra.Feedbacks;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weasel.Postgresql;

namespace Soft
{
    public class LoggerDecorator : IFeedbackRepository
    {
        public LoggerDecorator(FeedbackRepository feedbackRepository)
        {
            FeedbackRepository = feedbackRepository;
        }

        public FeedbackRepository FeedbackRepository { get; }

        public Task<Feedback> Add(Feedback obj)
        {
            // LOG logic

            return FeedbackRepository.Add(obj);
        }

        public Task Delete(int id)
        {
            return FeedbackRepository.Delete(id);
        }

        public Task<List<Feedback>> Get()
        {
            return FeedbackRepository.Get();
        }

        public Task<Feedback> Get(int id)
        {
            return FeedbackRepository.Get(id);
        }

        public Task Update(Feedback obj)
        {
            return FeedbackRepository.Update(obj);
        }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMarten(options =>
            {
                options.Connection(Configuration.GetConnectionString("Default"));
                
                options.AutoCreateSchemaObjects = AutoCreate.All;
            });

            services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend v1"));
                app.UseCors("MyPolicy");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
