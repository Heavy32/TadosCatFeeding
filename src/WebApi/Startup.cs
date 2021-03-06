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
using DataBaseRepositories.UserManagement;
using DataBaseRepositories.CatRepository;
using DataBaseRepositories.StatisticProvision;
using DataBaseRepositories.CatSharingRepository;
using DataBaseRepositories.CatFeedingRepository;
using BusinessLogic.UserManagement;
using BusinessLogic;
using BusinessLogic.StatisticProvision;
using BusinessLogic.CatSharingManagement;
using BusinessLogic.CatManagement;
using BusinessLogic.CatFeedingManagement;
using BusinessLogic.UserManagement.PasswordProtection;

namespace WebApi
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

            var userRepository = new UserRepository(Configuration.GetConnectionString("CatFeedingDB"));
            var catRepository = new CatRepository(Configuration.GetConnectionString("CatFeedingDB"));
            var statisticRepository = new StatisticRepository(Configuration.GetConnectionString("CatFeedingDB"));
            var catSharingRepository = new CatSharingRepository(Configuration.GetConnectionString("CatFeedingDB"));
            var catFeedingRepository = new CatFeedingRepository(Configuration.GetConnectionString("CatFeedingDB"));
            var statisticCalculation = new StatisticCalculation(Configuration.GetConnectionString("CatFeedingDB"));

            services.AddScoped<IUserEntrance>(userEntrance => new UserEntranceProvider(userRepository, new HashWithSaltProtector(10), new Mapper()));
            services.AddScoped<IUserCRUDService>(userCRUDservice => new UserCRUDService(userRepository, new HashWithSaltProtector(10), new Mapper()));
            services.AddScoped<IStatisticCalculation>(statistiCalculation => statisticCalculation);
            services.AddScoped<IStatisticService>(statisticCRUDService => new StatisticService(statisticRepository, statisticCalculation, new Mapper()));
            services.AddScoped<ICatSharingService>(catSharingService => new CatSharingService(catSharingRepository, catRepository, userRepository, new Mapper()));
            services.AddScoped<ICatCRUDService>(catCRUDService => new CatCRUDService(catRepository, catSharingRepository, userRepository, new Mapper()));
            services.AddScoped<ICatFeedingService>(catFeedingService => new CatFeedingService(catFeedingRepository, catRepository, userRepository, catSharingRepository, new Mapper()));
            services.AddScoped<IServiceResultStatusToResponseConverter>(responseConverter => new ServiceResultCodeToResponseConverter());
            services.AddScoped<IMapper>(Mapper => new Mapper());

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
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
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
