﻿namespace Mango.Services.ShoppingCartApi.Models.Dtos
{
    public class CouponDto
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
    }
}
