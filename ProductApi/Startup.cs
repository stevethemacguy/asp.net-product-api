using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

            //Old connectionString
            //var connectionString = Startup.Configuration["connectionStrings:productApiDBConnectionString"];

            //Used this to connect to the SQL DB on Azure (the one being used with VS TS)
            var connectionString = Startup.Configuration["connectionStrings:DefaultConnection"];

            //Use this to connect to the Github SQL DB (the one being used with my GitHub service)
            //var connectionString = Startup.Configuration["connectionStrings:GitHubConnection"]; 
            
            //Add the DB context so we can inject it into our classes
            services.AddDbContext<ProductApiContext>(o => o.UseSqlServer(connectionString));

            //Scoped creates the ProductRepository once per request.
            services.AddScoped<IProductRepository, ProductRepository>();

            services.Configure<IdentityOptions>(options =>
            {
                //Password settings
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                //options.Password.RequireLowercase = false;

                //Lockout settings
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //options.Lockout.MaxFailedAccessAttempts = 10;

                //Cookie settings
                //options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                //options.Cookies.ApplicationCookie.LoginPath = "/login";
                //options.Cookies.ApplicationCookie.LogoutPath = "/register";

                //By default, the Identity framework redirects users to a login page when they are unathorized (i.e. not logged in).
                //Since ProductApi is an API only (i.e. there are no front-end views), this will not work. The code below makes it so 
                //instead of returning a 302 redirect url, a 401 is returned instead, which allows the Front End to handle it differently.
                //With angular, for example, you can redirect the user to the login page using $state.go or $location (depending on the router).
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        return Task.FromResult(0);
                    }
                };

                // User settings
                //options.User.RequireUniqueEmail = true;
            });

            //Add the Identity framework for user authentication. Use our custom "User" Class and use the built-in IdentityRole Class
            //You can also extend the built in IdentityRole. If you do, then you will need to update the databaseContext and schema as well.
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ProductApiContext>() //Use to generate Identity-related entities to our Database
                //TokenProviders are used for 2-way or external authentication, or when using a token service, etc.
                .AddDefaultTokenProviders();

            //If you want to use the built-in User and Role classes, you would just use the following:
            //services.AddIdentity<IdentityUser, IdentityRole>();

            // You can do this all in one line if you want
            //services.AddEntityFramework()
            //    .AddSqlServer()
            //    .AddDbContext<ProductApiContext>(options =>
            //        options.UseSqlServer(connectionString));

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

            //Add cookie-based authentication. This must come BEFORE use MVC! When use Identity is enabled,
            //this will return 302 redirects instead of a 401: Unauthorized
            app.UseIdentity();

            //Seed the database with data. You may want to seed data directly with SQL commands instead (e.g. with a PowerShell script).
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

                //Shopping Cart Mappings
                cfg.CreateMap<Entities.BillingAddressEntity, Models.BillingAddress>();
                cfg.CreateMap<Entities.ShippingAddressEntity, Models.ShippingAddress>();
                //cfg.CreateMap<Entities.ShoppingCartEntity, Models.ShoppingCart>();
                //cfg.CreateMap<Entities.CartItemEntity, Models.CartItem>();

                //Shopping Cart return Mappings
                cfg.CreateMap<Models.BillingAddress, Entities.BillingAddressEntity>();
                cfg.CreateMap<Models.ShippingAddress, Entities.ShippingAddressEntity>();
                //cfg.CreateMap<Models.Product, Entities.ProductEntity>();
                //cfg.CreateMap<Models.ProductForCreation, Entities.ProductEntity>();

                //Payment Method mappings
                cfg.CreateMap<Entities.PaymentMethodEntity, Models.PaymentMethod>();
                cfg.CreateMap<Models.PaymentMethod, Entities.PaymentMethodEntity>();

                //Orders
                cfg.CreateMap<Entities.OrderEntity, Models.Order>();
                cfg.CreateMap<Models.Order, Entities.OrderEntity>();

                cfg.CreateMap<Entities.OrderItemEntity, Models.OrderItem>();
                cfg.CreateMap<Models.OrderItem, Entities.OrderItemEntity>();
            });

            //Enable CORS
            app.UseCors(builder =>
                    builder.AllowCredentials()
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()

                    //This works locally, but is causing errors on production when using IIS Integration.
                    //Pheraps because I need to accept more headers and don't know the heroku IP address.
                    //builder.WithOrigins(Startup.Configuration["allowedCorsOrigins:macLocalHost"],
                    //                    Startup.Configuration["allowedCorsOrigins:macPublic"])
                    //.WithHeaders("accept", "content-type", "origin")
                    //.WithMethods("POST", "GET", "PUT","DELETE","OPTIONS")
                    //.AllowCredentials()

                    // You can normally use this, but when you send FE requests using credentials, the browser requires that 
                    // you use a more strict cors policy like the one above.
                    //.AllowAnyOrigin()
                    //.AllowAnyHeader()
                    //.AllowAnyMethod()
            );

            // Show Error pages when the consuming browser gets an error (e.g. instead of a silent 404 error)
            //app.UseStatusCodePages();
            app.UseMvc();

           //Do I need app.run here?
        }
    }
}
