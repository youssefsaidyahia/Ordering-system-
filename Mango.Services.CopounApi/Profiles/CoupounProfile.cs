using AutoMapper;
using Mango.Services.CopounApi.Models;
using Mango.Services.CopounApi.Models.Dtos;

namespace Mango.Services.CopounApi.Profiles
{
    public class CoupounProfile : Profile
    {
        public CoupounProfile()
        {
           CreateMap<Coupon, CouponDto>();
           CreateMap<CouponDto, Coupon>();
        }
    }
}
