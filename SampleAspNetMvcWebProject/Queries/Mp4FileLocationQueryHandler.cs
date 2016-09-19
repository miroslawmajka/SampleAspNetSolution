using SampleVideoStreamingSite.Abstract;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Http;

namespace SampleVideoStreamingSite.Queries
{
    public class Mp4FileLocationQueryHandler : IQueryHandler<Mp4FileLocationQuery, string>
    {
        private const string VIDEO_DIRECTORY = @"~/Assets/Video/";
        private const string MP4_FILE_EXTENSION = ".mp4";

        public string Handle(Mp4FileLocationQuery query)
        {
            var fileLocation = string.Empty;
            var videoDirectory = HttpContext.Current.Request.MapPath(VIDEO_DIRECTORY);

            if (string.IsNullOrWhiteSpace(query.FileName))
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            else
                fileLocation = $"{videoDirectory}{query.FileName}{MP4_FILE_EXTENSION}";

            if (!File.Exists(fileLocation)) throw new HttpResponseException(HttpStatusCode.NotFound);

            return fileLocation;
        }
    }
}