using static Mango.Web.SD;

namespace Mango.Web.Models
{
    public class APIRequest
    {
        public ApiType apiType { get; set; } = ApiType.GET;
        public string url { get; set; } 
        public object  Data { get; set; }
        public string  AccessToken { get; set; }
    }
}
