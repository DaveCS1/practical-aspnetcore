using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;

namespace NewRouting
{
    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public bool IsMarried { get; set; }

        public DateTimeOffset CurrentTime { get; set; }

        public bool? IsWorking { get; set; }

        public Dictionary<string, bool> Characters { get; set; }
    }


    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(route =>
            {
                route.MapGet("/", async context =>
                {
                    var payload = new Person
                    {
                        Name = "Annie",
                        Age = 33,
                        IsMarried = false,
                        CurrentTime = DateTimeOffset.UtcNow,
                        Characters = new Dictionary<string, bool>
                        {
                            {"Funny" , true},
                            {"Feisty" , true},
                            {"Brilliant" , true},
                            {"FOMA", false}
                        },
                        IsWorking = true
                    };

                    context.Response.Headers.Add(HeaderNames.ContentType, "application/json");
                    await JsonSerializer.SerializeAsync(context.Response.Body, payload, typeof(Person));
                });
            });
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().
                    UseEnvironment(Environments.Development);
                });
    }
}