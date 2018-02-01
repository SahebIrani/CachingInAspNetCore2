using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingInAspNetCore2.Models
{
	public class CacheViewModel
	{
		public DateTime? FirstTime { get; set; }
		public DateTime SecondTime { get; set; }
	}
}