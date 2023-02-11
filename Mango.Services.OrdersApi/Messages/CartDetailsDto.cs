using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrdersApi.Messages
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; } 
        public string CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public virtual ProductDto Product { get; set; }
        public int  Count { get; set; }

   
    }
}
