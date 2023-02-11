namespace Mango.Services.ShoppingCartApi.Models.Dtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public  IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
