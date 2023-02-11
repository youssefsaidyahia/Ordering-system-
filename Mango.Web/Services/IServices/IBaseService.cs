using Mango.Web.Models;
using Mango.Web.Models.Dtos;

namespace Mango.Web.Services.IServices
{
    public interface IBaseService : IDisposable
    {
        ResponseDto  response { get; set; }
        Task<T> SendASync<T>(APIRequest ApiRequest);
    }
}
