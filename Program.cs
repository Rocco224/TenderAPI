using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
//using TenderAPI.Authentication;
//using TenderAPI.Contexts;
using TenderAPI.Services.EmailServices;
using Hangfire;

namespace TenderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("PEPPER", "pepper");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Aggiungi i servizi di Hangfire
            builder.Services.AddHangfire(configuration => configuration
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("TenderDB")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                        "CorsPolicy",
                        builder => builder
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                    );
            });

            builder.Services.AddControllers().AddJsonOptions(
                options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddScoped<IEmailService, EmailService>();

           // builder.Services.AddDbContext<TenderDbContext>(
            //    options => options.UseSqlServer(builder.Configuration.GetConnectionString("TenderDB")));

            // Aggiunta autenticazione JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                // validazione token
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
            builder.Services.AddAuthorization();

            // Aggiunta configurazione da appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();
            IConfiguration configuration = app.Configuration;
            IWebHostEnvironment environment = app.Environment;

            // Esegui il job Hangfire
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            // Pianificazione email ogni giorno alle 9:00
            RecurringJob.AddOrUpdate<IEmailService>(x => x.SendEmail(new Models.EmailDto()), "0 9 * * *", TimeZoneInfo.Local); // sintassi cron: minuto - ora - giorno del mese - mese - giorno della settimana

            app.MapControllers();
            
            app.Run();
        }
    }
}