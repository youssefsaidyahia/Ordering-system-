    using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto response { get; set; }
        public IHttpClientFactory httpClientFactory { get; set; }
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.response=new ResponseDto();
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<T> SendASync<T>(APIRequest ApiRequest)
        {
            try{
                var client=httpClientFactory.CreateClient("MangoApi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(ApiRequest.url);
                client.DefaultRequestHeaders.Clear();
                if(ApiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(ApiRequest.Data), Encoding.UTF8, "application/json");
                }
                if (!string.IsNullOrEmpty(ApiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiRequest.AccessToken);
                }
                HttpResponseMessage httpResponse = null;
                switch (ApiRequest.apiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                     default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                httpResponse = await client.SendAsync(message);
                var apiContent=await httpResponse.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponse;
            }
            catch(Exception ex)
            {
                var Dto = new ResponseDto()
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { ex.Message },
                    IsSucess = false
                };
            var RES = JsonConvert.SerializeObject(Dto);
            var apiResponse = JsonConvert.DeserializeObject<T>(RES);
            return apiResponse;
        }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
