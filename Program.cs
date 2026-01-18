
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Middleware;
using LibraryManagementSystem.Services.Implementations;
using LibraryManagementSystem.Services.Interface;
using LibraryManagementSystem.Services.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LibraryManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            // EF Core
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Options
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

            // Services
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddTransient<IBookService, BookService>();
            builder.Services.AddTransient<IAuthService, AuthService>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));



            builder.Services.AddControllers();
            // JWT Auth
            var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter: Bearer {your JWT token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
            });


            var app = builder.Build();

            // Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            // }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            // Seed DB on startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await DbSeeder.SeedAsync(db);
            }


            app.Run();
        }
    }
}
