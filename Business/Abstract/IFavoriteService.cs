﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IFavoriteService
	{
		//void Favorite(Favorite favorite);
		Task Favorite(Favorite favorite);
		Task Unfavorite(Favorite unFavorite);
	}
}
