﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using ProductApi.Services;
using Microsoft.Extensions.Configuration;
using ProductApi.Entities;
using ProductApi.Models;

namespace ProductApi
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        //Use a json settings file to configure our environment.
        public Startup(IHostingEnvironment env)
        {
            //The second addJsonFile is used for a production environment. If there's a conflict between the two files,
            //The last file added wins.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //Set up the SQL Server connection
            //var connectionString = Startup.Configuration["connectionStrings:productApiDBConnectionString"];

            // Use to connect to the SQL DB on Azure (the one being used with VS TS)
            //var connectionString = Startup.Configuration["connectionStrings:DefaultConnection"];

            // Use to connect to the Github SQL DB (the one being used with my GitHub service)
            var connectionString = Startup.Configuration["connectionStrings:GitHubConnection"]; 
            
            //Add the DB context so we can inject it into our classes
            services.AddDbContext<ProductApiContext>(o => o.UseSqlServer(connectionString));

            //Scoped creates the ProductRepository once per request.
            services.AddScoped<IProductRepository, ProductRepository>();

            //If you want to return UpperCase property names instead of camelCase. For new applications,
            //you'll probably want camelCase (the .Net Core default), but if you're working with Old MVC apps, then you may want uppercase.
            //services.AddMvc()
            //   .AddJsonOptions(o => {
            //       if (o.SerializerSettings.ContractResolver != null)
            //       {
            //           var castedResolver = o.SerializerSettings.ContractResolver
            //               as DefaultContractResolver;
            //               castedResolver.NamingStrategy = null;
            //       }
            //   });


            //If you want to return XML instead of JSON when the consuming app specifies an XMLL "content-type"
            //services.AddMvc()
            //    .AddMvcOptions(o => o.OutputFormatters.Add(
            //        new XmlDataContractSerializerOutputFormatter()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ProductApiContext productApiContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            //Long way to add the nLogger extension to loggerFactory
            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());

            //Short-cut. Now we're also logging out to a file.
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler();
            }

            //Seed the database with data
            productApiContext.EnsureSeedDataForContext();

            //Create mappings from the application's DTOs to their respective entity classes returned from Sql.
            AutoMapper.Mapper.Initialize(cfg =>
            {
                //Syntax is CreateMap(SourceType, DestinationType). In other words: From an Entity to a DTO object
                cfg.CreateMap<Entities.ProductEntity, Models.Product>();
                cfg.CreateMap<Entities.ProductEntity, Models.ProductForCreation>();
                cfg.CreateMap<Entities.ShoppingCartEntity, Models.ShoppingCart>();
                cfg.CreateMap<Entities.CartItemEntity, Models.CartItem>();

                //Map the other for Post Requests
                cfg.CreateMap<Models.ShoppingCart, Entities.ShoppingCartEntity>();
                cfg.CreateMap<Models.CartItem, Entities.CartItemEntity>();
                cfg.CreateMap<Models.Product, Entities.ProductEntity>();
                cfg.CreateMap<Models.ProductForCreation, Entities.ProductEntity>();
            });

            //Enable CORS
            app.UseCors(builder =>
                    builder.WithOrigins(Startup.Configuration["allowedCorsOrigins:macLocalHost"])
                     .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            // Show Error pages when the consuming browser gets an error (e.g. instead of a silent 404 error)
            //app.UseStatusCodePages();
            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
