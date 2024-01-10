using AutoMapper;
using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapping
{
	public class MapProfile : Profile
	{
		public MapProfile() 
		{
			CreateMap<AppUser, AppUserRegisterDto>().ReverseMap();
			CreateMap<AppUser, AppUserLoginDto>().ReverseMap();
			CreateMap<AppUser, AppUserDetailsDto>().ReverseMap();
		}
		
	}
}
