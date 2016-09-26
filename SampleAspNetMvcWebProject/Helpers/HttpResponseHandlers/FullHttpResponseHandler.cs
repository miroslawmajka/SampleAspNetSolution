using System.Net.Http;

namespace SampleAspNetMvcWebProject.Helpers.HttpResponseHandlers
{
    public class FullHttpResponseHandler : HttpResponseHandler
    {
        public FullHttpResponseHandler(HttpRequestMessage request, string fileLocation) 
            : base(request, fileLocation) { }

        internal override HttpResponseMessage GetResponse()
        {
            var fileStream = GetFileStream();

            var response = Request.CreateResponse(OkStatusCode);
            response.Content = new StreamContent(fileStream);
            response.Content.Headers.ContentLength = fileStream.Length;
            response.Content.Headers.ContentType = MediaType;
            response.Content.Headers.ContentDisposition = GetInlineContentDisposition();

            return response;
        }
    }
}