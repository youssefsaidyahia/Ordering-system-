﻿namespace Mango.Services.OrdersApi.Messages
{
    public class UpdatePaymentResultMessage
    {
        public int OrderId { get; set; }
        public bool status { get; set; }
        public string Email { get; set; }
    }
}
