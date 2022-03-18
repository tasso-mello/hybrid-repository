namespace api.hybrid.repository
{
    using api.hybrid.repository.Core.Compression;
    using api.hybrid.repository.Core.Exceptions;
    using api.hybrid.repository.Core.Injections;
    using api.hybrid.repository.Core.Middlewares;
    using AutoMapper;
    using core.hybrid.repository.Configurations;
    using data.hybrid.repository.Configurations;
    using data.hybrid.repository.Context;
    using data.hybrid.repository.Service;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using System;
    using System.Text;

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
			services.Configure<IISServerOptions>(options => options.AutomaticAuthentication = false);
			services.AddControllers();

			#region Configurations

			var dapperConfigurations = new DapperConfigurations();
			new ConfigureFromConfigurationOptions<DapperConfigurations>(Configuration.GetSection("ConnectionStrings")).Configure(dapperConfigurations);
			services.AddSingleton(dapperConfigurations);

			#endregion Configurations

			#region Mapper

			var config = new MapperConfiguration(c => c.AddMaps(AppDomain.CurrentDomain.Load("core.hybrid.repository")));
			services.AddSingleton(s => config.CreateMapper());

			#endregion Mapper

			#region Compression

			services.AddResponseCompression(options =>
			{
				options.Providers.Add<BrotliCompressionProvider>();
				options.EnableForHttps = true;
			});

			#endregion Compression

			#region Trust

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader());
			});

			#endregion Trust

			#region Compatibility

			services.AddMvc(options =>
			{
				options.EnableEndpointRouting = false;
				options.Filters.Add(typeof(CustomException));
			}).SetCompatibilityVersion(CompatibilityVersion.Latest)
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.PropertyNamingPolicy = null;
					options.JsonSerializerOptions.DictionaryKeyPolicy = null;
				});

			#endregion

			#region Injections

			//db-services
			services.AddScoped<IDapperDbService, DapperDbService>();

			//Business
			services.AddBusinessInjectionScoped();

			// Repositories
			services.AddRepositoryInjectionScoped();
			#endregion

			#region SQL

			services.AddDatabaseWithSeed(Configuration.GetConnectionString("SqlConnection"));

			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

			#endregion

			#region Swagger

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "api.hybrid.repository", Version = "v1" });
			});

			#endregion Swagger
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.hybrid.repository v1");
				c.RoutePrefix = string.Empty;
				c.DocumentTitle = "Api Hybrid Repository documentation";
				c.DocExpansion(DocExpansion.None);
			});

			app.UseResponseCompression();
			app.UseCors("CorsPolicy");
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();
			app.UseMiddleware<RequestResponseAuditMiddleware>();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
    }
}
