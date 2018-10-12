using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreetServer
{	
	class Program
	{		
		static void Main(string[] args)
		{
			InitData();
			StreetDBServer server = new StreetDBServer(1024);			
			//Console.Read();			
		}		
		private static void InitData()
		{
			
			List<StreetDB> list = new List<StreetDB>()
			{
				new StreetDB(){Street = "street1", Zip = "50001"},
				new StreetDB(){Street = "street2", Zip = "50002"},
				new StreetDB(){Street = "street3", Zip = "50001"},
				new StreetDB(){Street = "street4", Zip = "50003"},
				new StreetDB(){Street = "street5", Zip = "50001"},
				new StreetDB(){Street = "street6", Zip = "50003"},
				new StreetDB(){Street = "street7", Zip = "50002"},				
			};
			

			using (DataModel db = new DataModel())
			{
				foreach(var addres in list)
				{
					if (db.StreetSet.FirstOrDefault(s => s.Street == addres.Street) == null)
					{
						db.StreetSet.Add(addres);
						db.SaveChanges();
					}
				}
			}
		}
	}
}
