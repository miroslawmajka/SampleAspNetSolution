using System.Net.Http;

namespace SampleAspNetMvcWebProject.Helpers.HttpResponseHandlers
{
    public class HeadHttpResponseHandler : HttpResponseHandler
    {
        public HeadHttpResponseHandler(HttpRequestMessage request, string fileLocation)
            : base(request, fileLocation) { }

        internal override HttpResponseMessage GetResponse()
        {
            var response = Request.CreateResponse(OkStatusCode);
            response.Content = new StringContent(OkStatusCode.ToString());
            response.Content.Headers.ContentLength = GetFileStream().Length;
            response.Content.Headers.ContentType = MediaType;

            return response;
        }
    }
}