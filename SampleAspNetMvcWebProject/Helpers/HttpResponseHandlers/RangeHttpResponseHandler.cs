using System.Net;
using System.Net.Http;

namespace SampleVideoStreamingSite.Helpers.HttpResponseHandlers
{
    public class RangeHttpResponseHandler : HttpResponseHandler
    {
        public RangeHttpResponseHandler(HttpRequestMessage request, string fileLocation)
            : base(request, fileLocation) { }

        internal override HttpResponseMessage GetResponse()
        {
            var response = Request.CreateResponse(HttpStatusCode.PartialContent);
            response.Content = new ByteRangeStreamContent(GetFileStream(), Request.Headers.Range, MediaType);
            response.Content.Headers.ContentDisposition = GetInlineContentDisposition();

            return response;
        }
    }
}