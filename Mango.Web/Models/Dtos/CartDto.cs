namespace Mango.Web.Models.Dtos
{
    public class CartDto
    {
        public CartHeaderDto cartHeader { get; set; }
        public IEnumerable<CartDetailsDto> cartDetails { get; set; }
    }
}
