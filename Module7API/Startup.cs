using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Module7API.Dal;
using Module7API.Dal.Helpers;
using Module7API.Models;
using Module7API.Security;
using Module7API.Security.Model;
using Module7API.Security.Services;
using Module7API.Services;
using Module7API.Services.Models;
using Responses;
using Note = Module7API.Models.Note;

namespace Module7API
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddMvc();
            services.AddCors(o=> o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IUserRepository, MongoUserRepository>();
            services.AddScoped<ITokenBuilderService, TokenBuilderService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddSingleton<INoteRepository, MongoNoteRepository>();
            services.AddScoped<IResponseBuilderFactory, ResponseBuilderFactory>();
            services.AddSingleton<IRepository<Module7API.Dal.Model.User>, MongoRepository<Module7API.Dal.Model.User>>();
            services.AddSingleton<IRepository<Module7API.Dal.Model.Note>, MongoRepository<Module7API.Dal.Model.Note>>();
            services.AddScoped<IDateTimeWrapper, UtcDateTimeWrapper>();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserCreateViewModel, User>();
                cfg.CreateMap<User, Dal.Model.User>().ReverseMap();
                cfg.CreateMap<UserLoginViewModel, UserLoginModel>();
                cfg.CreateMap<NoteCreateViewModel, Services.Models.Note>();
                cfg.CreateMap<Services.Models.Note, Dal.Model.Note>().ReverseMap();
                cfg.CreateMap<Services.Models.Note, Note>().ReverseMap();
                cfg.CreateMap<NoteEditViewModel, Services.Models.EditNote>();
                cfg.CreateMap<EditNote, Dal.Model.EditNote>();
                cfg.CreateMap<NoteEditViewModel, Services.Models.Note>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
