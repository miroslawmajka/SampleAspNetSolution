using SampleVideoStreamingSite.Abstract;
using System.Net.Http;

namespace SampleVideoStreamingSite.Queries
{
    public class Mp4Query : IQuery<HttpResponseMessage>
    {
        public HttpRequestMessage Request { get; set; }
        public string FileName { get; set; }
    }
}