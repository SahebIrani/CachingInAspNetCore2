using System;
using CachingInAspNetCore2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace CachingInAspNetCore2.Controllers
{
	public class CachingController: Controller
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IDistributedCache _cache;

		public CachingController ( IMemoryCache memoryCacheme , IDistributedCache cache )
		{
			_memoryCache = memoryCacheme;
			_cache=cache;
		}

		public IActionResult Index ( ) => View( );

		public IActionResult IMemoryCache ( [FromServices]IMemoryCache cache )
		{
			#region Secton1

			//DateTime currentTime;
			//bool isExist = _memoryCache.TryGetValue("CacheTime", out currentTime);
			//if ( !isExist )
			//{
			//	currentTime = DateTime.Now;
			//	var cacheEntryOptions = new MemoryCacheEntryOptions()
			//	  .SetSlidingExpiration(TimeSpan.FromSeconds(30));

			//	_memoryCache.Set( "CacheTime" , currentTime , cacheEntryOptions );
			//}
			//return View( currentTime );

			#endregion Secton1

			#region Secton2

			//CacheViewModel model = new CacheViewModel
			//{
			//	FirstTime = _memoryCache.Get<DateTime?>("CacheTime"),
			//	SecondTime = _memoryCache.GetOrCreate("CacheTime", entry =>
			//	{
			//		entry.SlidingExpiration = TimeSpan.FromSeconds(45);
			//		return DateTime.Now;
			//	})
			//};
			//return View( model );

			#endregion Secton2

			#region Secton3

			DateTime testeCache = cache.GetOrCreate<DateTime>(
			"TesteCache", context =>
			{
				context.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
				context.SetPriority(CacheItemPriority.High);
				return DateTime.Now;
			});

			ViewBag.TesteCache = testeCache;

			#endregion Secton3

			#region Secton4

			DateTime currentTime = _memoryCache.GetOrCreate("CacheTime", entry =>
			{
				entry.SlidingExpiration = TimeSpan.FromSeconds(45);
				return DateTime.Now;
			});
			return View( currentTime );

			#endregion Secton4
		}

		[HttpPost]
		public IActionResult RemoveCache ( )
		{
			_memoryCache.Remove( "CacheTime" );
			DateTime? currentTime = _memoryCache.Get<DateTime?>("CacheTime");
			return View( "Index" , currentTime );
		}

		private void StoreValueCache ( string key , string value )
		{
			DistributedCacheEntryOptions optionsCache = new DistributedCacheEntryOptions();
			optionsCache.SetAbsoluteExpiration( TimeSpan.FromMinutes( 1 ) );
			_cache.SetString( key , value , optionsCache );
		}

		public IActionResult RedisCache ( )
		{
			string testeString =_cache.GetString("TesteString");
			if ( testeString == null )
			{
				testeString = "Test value";
				StoreValueCache( "TesteString" , testeString );
			}
			ViewBag.TesteString = testeString;

			TipoComplexo ObjectComplex = null;
			string strObjetoComplexo =_cache.GetString("TestObjectComplex");
			if ( strObjetoComplexo == null )
			{
				ObjectComplex = new TipoComplexo
				{
					Text = "Example value" ,
					IntegerValue = 2016 ,
					NumericoValue = 1914.99
				};

				strObjetoComplexo = JsonConvert.SerializeObject( ObjectComplex );
				StoreValueCache( "TestObjectComplex" , strObjetoComplexo );
			}
			else
			{
				ObjectComplex = JsonConvert.DeserializeObject<TipoComplexo>( strObjetoComplexo );
			}
			ViewBag.ObjectComplex = ObjectComplex;

			return View( );
		}

		//[ResponseCache( CacheProfileName = "ResponseCache" , Duration = 30 , Location = ResponseCacheLocation.Any )]
		[ResponseCache( Duration = 30 )]
		public IActionResult ResponseCache ( )
		{
			return View( );
		}

		public IActionResult SqlCache ( )
		{
			string testeString =_cache.GetString("TesteString");
			if ( testeString == null )
			{
				testeString = "Test value";
				StoreValueCache( "TesteString" , testeString );
			}
			ViewBag.TesteString = testeString;

			TipoComplexo ObjectComplex = null;
			string strObjetoComplexo =_cache.GetString("TestObjectComplex");
			if ( strObjetoComplexo == null )
			{
				ObjectComplex = new TipoComplexo
				{
					Text = "Example value" ,
					IntegerValue = 2016 ,
					NumericoValue = 1914.99
				};

				strObjetoComplexo = JsonConvert.SerializeObject( ObjectComplex );
				StoreValueCache( "TestObjectComplex" , strObjetoComplexo );
			}
			else
			{
				ObjectComplex = JsonConvert.DeserializeObject<TipoComplexo>( strObjetoComplexo );
			}
			ViewBag.ObjectComplex = ObjectComplex;

			return View( );
		}

		public IActionResult TagHelperCache ( )
		{
			return View( );
		}
	}
}