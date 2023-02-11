namespace Mango.Web.Models.Dtos
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public string CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public virtual CartHeaderDto cartHeader { get; set; }
        public virtual ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
