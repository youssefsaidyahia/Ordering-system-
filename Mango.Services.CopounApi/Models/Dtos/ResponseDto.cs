namespace Mango.Services.CopounApi.Models.Dtos
{
    public class ResponseDto
    {
        public bool IsSucess { get; set; } = true;
        public object Result { get; set; }
        public string DisplayMessage { get; set; } = "";
        public List<string> ErrorMessages { get; set; }
    }
}
