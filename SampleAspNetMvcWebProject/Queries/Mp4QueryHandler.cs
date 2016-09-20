using SampleVideoStreamingSite.Abstract;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SampleVideoStreamingSite.Queries
{
    public class Mp4QueryHandler : IQueryHandler<Mp4Query, HttpResponseMessage>
    {
        private readonly IQueryHandler<Mp4FileLocationQuery, string> fileLocationQueryHandler;
        
        private const string MP4_MEDIA_TYPE = "video/mp4";

        public Mp4QueryHandler(IQueryHandler<Mp4FileLocationQuery, string> fileLocationQueryHandler)
        {
            this.fileLocationQueryHandler = fileLocationQueryHandler;
        }

        public HttpResponseMessage Handle(Mp4Query query)
        {
            var fileLocation = fileLocationQueryHandler.Handle(new Mp4FileLocationQuery(query.FileName));

            return GetResponse(fileLocation, query.Request);
        }

        private HttpResponseMessage GetResponse(string fileLocation, HttpRequestMessage request)
        {
            var okStatusCode = HttpStatusCode.OK;
            var mediaType = new MediaTypeHeaderValue(MP4_MEDIA_TYPE);

            HttpResponseMessage response;

            // Have to use FileShare.Read to allow other processes to access the same file
            var fileStream = File.Open(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);

            if (request.Method == HttpMethod.Head)
            {
                response = request.CreateResponse(okStatusCode);
                response.Content = new StringContent(okStatusCode.ToString());
                response.Content.Headers.ContentType = mediaType;
                response.Content.Headers.ContentLength = fileStream.Length;
            }
            else
            {
                // Check if streaming media requested (partial content)
                if (request.Headers.Range != null)
                {
                    response = request.CreateResponse(HttpStatusCode.PartialContent);
                    response.Content = new ByteRangeStreamContent(fileStream, request.Headers.Range, mediaType);
                }
                else
                {
                    response = request.CreateResponse(okStatusCode);
                    response.Content = new StreamContent(fileStream);
                    response.Content.Headers.ContentType = mediaType;
                    response.Content.Headers.ContentLength = fileStream.Length;
                }

                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = Path.GetFileName(fileLocation)
                };
            }

            return response;
        }
    }
}