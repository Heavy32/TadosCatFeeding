using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text;
using DataBaseManagement.UserManagement;
using DataBaseManagement.CatManagement;
using DataBaseManagement.StatisticProvision;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.CatFeedingManagement;
using Services.UserManagement;
using Services;
using Services.StatisticProvision;
using Services.CatSharingManagement;
using Services.CatManagement;
using Services.CatFeedingManagement;
using Services.UserManagement.PasswordProtection;

namespace TadosCatFeeding
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            var userRepository = new UserRepository(Configuration.GetConnectionString("PetFeedingDB"));
            var catRepository = new CatRepository(Configuration.GetConnectionString("PetFeedingDB"));
            var statisticRepository = new StatisticRepository(Configuration.GetConnectionString("PetFeedingDB"));
            var catSharingRepository = new CatSharingRepository(Configuration.GetConnectionString("PetFeedingDB"));
            var catFeedingRepository = new CatFeedingRepository(Configuration.GetConnectionString("PetFeedingDB"));
            var statisticCalculation = new StatisticCalculation(Configuration.GetConnectionString("PetFeedingDB"));

            services.AddScoped<IUserEntrance>(userEntrance => new UserEntranceProvider(userRepository, new HashWithSaltProtector(10), new Mapper()));
            services.AddScoped<IUserCRUDService>(userCRUDservice => new UserCRUDService(userRepository, new HashWithSaltProtector(10), new Mapper()));
            services.AddScoped<IStatisticCalculation>(statistiCalculation => statisticCalculation);
            services.AddScoped<IStatisticService>(statisticCRUDService => new StatisticService(statisticRepository, statisticCalculation, new Mapper()));
            services.AddScoped<ICatSharingService>(catSharingService => new CatSharingService(catSharingRepository, catRepository, userRepository, new Mapper()));
            services.AddScoped<ICatCRUDService>(catCRUDService => new CatCRUDService(catRepository, catSharingRepository, userRepository, new Mapper()));
            services.AddScoped<ICatFeedingService>(catFeedingService => new CatFeedingService(catFeedingRepository, catRepository, userRepository, catSharingRepository, new Mapper()));
            services.AddScoped<IServiceResultStatusToResponseConverter>(responseConverter => new ServiceResultCodeToResponseConverter());

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = "TaskServer",
                            ValidateAudience = true,
                            ValidAudience = "http://localhost:44338/",
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("UNBELIEVABLEsecretKEEEEEYYYYYY!!!!!=)")),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tados Test Task", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tados Test Task");
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
