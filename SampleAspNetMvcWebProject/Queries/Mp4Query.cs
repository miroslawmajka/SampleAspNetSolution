using SampleAspNetMvcWebProject.Abstract;
using System.Net.Http;

namespace SampleAspNetMvcWebProject.Queries
{
    public class Mp4Query : IQuery<HttpResponseMessage>
    {
        public HttpRequestMessage Request { get; set; }
        public string FileName { get; set; }
    }
}