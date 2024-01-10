using Core.Entities;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
	public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
	{
		public FavoriteRepository(AppDbContext context) : base(context) { }
	}
}
