using AutoMapper;
using Mango.Services.CopounApi.DbContexts;
using Mango.Services.CopounApi.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CopounApi.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext  _context;
        private IMapper _mapper;

        public CouponRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByCode(string copounCode)
        {
            var coupon =await _context.Coupons.FirstOrDefaultAsync(x => x.CouponCode == copounCode);
            return _mapper.Map<CouponDto>(coupon);
        }
    }
}
