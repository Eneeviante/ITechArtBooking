using ITechArtBooking.Domain.Interfaces;
using ITechArtBooking.Domain.Models;
using ITechArtBooking.Infrastucture.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ITechArtBooking.Infrastucture.Repositories.Fakes;
using ITechArtBooking.Domain.Services;
using ITechArtBooking.Domain.Services.ServiceInterfaces;

namespace ITechArtBooking
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
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ITechArtBooking", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme {
                        In = ParameterLocation.Header,
                        Description = @"JWT Authorization header using the Bearer scheme. 
                                      Enter 'Bearer' [space] and then your token in the text input below.
                                      Example: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    Array.Empty<string>()
                }
                });
            });

            string connection = Configuration.GetConnectionString("DefaultConnection");
            //gets the options object that configures the database for the context class
            services.AddDbContext<EFBookingDBContext>(options => {
                options.UseSqlServer(connection);
            });

            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IHotelService, HotelService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IAccountService, AccountService>();

            //services.AddTransient<IUserRepository, EFUserRepository>();          //defines a service that creates a new instance
            services.AddTransient<IHotelRepository, EFHotelRepository>();            //of the EFUserRepository class
            services.AddTransient<ICategoryRepository, EFCategoryRepository>();      //every time an instance of the IUserRepository type is required
            services.AddTransient<IReviewRepository, EFReviewRepository>();
            services.AddTransient<IRoomRepository, EFRoomRepository>();
            services.AddTransient<IBookingRepository, EFBookingRepository>();

            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<EFBookingDBContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(
                     options => {
                         options.TokenValidationParameters = new TokenValidationParameters {
                             ValidateIssuer = false,
                             ValidateAudience = false,
                             ValidateLifetime = false,
                             ValidateIssuerSigningKey = false,
                             IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes("aksdokjafbkjasbfjabojsfbda"))
                         };
                     }
                 );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ITechArtBooking v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedFirstData.SeedAdminUser(serviceProvider);
        }
    }
}
