using Inside.TestTask.MC1.Services;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTracing;
using OpenTracing.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Websocket.Client;

namespace Inside.TestTask.MC1
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

            services.AddControllers();
            services.AddOpenTracing();
            services.AddSingleton<ITimerService, TimedHostedService>();
            services.AddTransient<IDataLayer, MariadbDataLayer>();
            services.AddSingleton(wsClient =>
           {
               var url = new Uri(Configuration["WS:SocketMC2_URL"]);
               var client = new WebsocketClient(url);
               client.ReconnectTimeout = TimeSpan.FromSeconds(int.Parse(Configuration["WS:TimeOutInSec"]));
               return client;
           });
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var serviceName = serviceProvider.GetRequiredService<IWebHostEnvironment>().ApplicationName;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                // This is necessary to pick the correct sender, otherwise a NoopSender is used!
                Jaeger.Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
                    .RegisterSenderFactory<ThriftSenderFactory>();
                var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(new UdpSender(Configuration["JaegerHost"], 6831, 0))
                    .Build();

                // This will log to a default localhost installation of Jaeger.
                var tracer = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .WithSampler(new ConstSampler(true))
                    .WithReporter(reporter)
                    .Build();

                // Allows code that can't use DI to also access the tracer.
                GlobalTracer.Register(tracer);

                return tracer;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inside.TestTask.MC1", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inside.TestTask.MC1 v1"));
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
