using Business.Abstract;
using Core.Entities;
using DataAccess.Abstract;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class FavoriteService : IFavoriteService
	{
		private readonly IUnitOfWork _unitOfWork;

		public FavoriteService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task Favorite(Favorite favorite)
		{
			await _unitOfWork.Favorites.AddAsync(favorite);
			await _unitOfWork.CommitAsync();
		}

		public async Task Unfavorite(Favorite unFavorite)
		{
			_unitOfWork.Favorites.Remove(unFavorite);
			await _unitOfWork.CommitAsync();
		}
	}
}
