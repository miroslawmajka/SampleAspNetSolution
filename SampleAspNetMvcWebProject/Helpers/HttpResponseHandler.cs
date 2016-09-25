using SampleVideoStreamingSite.Helpers.HttpResponseHandlers;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SampleVideoStreamingSite.Helpers
{
    public abstract class HttpResponseHandler
    {
        protected HttpStatusCode OkStatusCode { get { return HttpStatusCode.OK; } }
        protected MediaTypeHeaderValue MediaType { get { return new MediaTypeHeaderValue("video/mp4"); } }
        protected HttpRequestMessage Request { get; private set; }

        private string fileLocation;

        protected HttpResponseHandler(HttpRequestMessage request, string fileLocation)
        {
            Request = request;
            this.fileLocation = fileLocation;
        }

        protected FileStream GetFileStream()
        {
            return File.Open(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        protected ContentDispositionHeaderValue GetInlineContentDisposition()
        {
            return new ContentDispositionHeaderValue("inline")
            {
                FileName = Path.GetFileName(fileLocation)
            };
        }

        // The polymorphism 'router' that decides which child class to return
        internal static HttpResponseHandler GetResponseHandler(HttpRequestMessage request, string fileLocation)
        {
            if (request.Method == HttpMethod.Head) return new HeadHttpResponseHandler(request, fileLocation);
            if (request.Headers.Range != null) return new RangeHttpResponseHandler(request, fileLocation);

            return new FullHttpResponseHandler(request, fileLocation);
        }

        // 'Template method' design pattern
        internal abstract HttpResponseMessage GetResponse();
    }
}