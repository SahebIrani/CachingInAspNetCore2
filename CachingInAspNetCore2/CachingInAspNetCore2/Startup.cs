﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CachingInAspNetCore2
{
	public class Startup
	{
		public Startup ( IConfiguration configuration )
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices ( IServiceCollection services )
		{
			//IMemoryCache
			services.AddMemoryCache( );
			//◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘
			//RedisCache
			services.AddDistributedRedisCache( options =>
			{
				options.Configuration =
				    Configuration.GetSection( "ConnectRedis" ).Value;
				options.InstanceName = "TesteRedisCache";
			} );
			//◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘
			//RSqlCache
			services.AddDistributedSqlServerCache( options =>
			{
				options.ConnectionString = Configuration
				    .GetConnectionString( "CacheSQLServer" );
				options.SchemaName = "dbo";
				options.TableName = "CacheData";
			} );
			//◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘
			services.AddMvc( );
			//◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure ( IApplicationBuilder app , IHostingEnvironment env )
		{
			if ( env.IsDevelopment( ) )
			{
				app.UseBrowserLink( );
				app.UseDeveloperExceptionPage( );
			}
			else
			{
				app.UseExceptionHandler( "/Home/Error" );
			}

			app.UseStaticFiles( );

			app.UseMvc( routes =>
			{
				routes.MapRoute(
				 name: "default" ,
				 template: "{controller=Home}/{action=Index}/{id?}" );
			} );
		}
	}
}