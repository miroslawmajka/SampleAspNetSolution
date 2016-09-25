using SampleVideoStreamingSite.Abstract;
using SampleVideoStreamingSite.Helpers;
using System.Net.Http;

namespace SampleVideoStreamingSite.Queries
{
    public class Mp4QueryHandler : IQueryHandler<Mp4Query, HttpResponseMessage>
    {
        private readonly IQueryHandler<Mp4FileLocationQuery, string> fileLocationQueryHandler;

        public Mp4QueryHandler(IQueryHandler<Mp4FileLocationQuery, string> fileLocationQueryHandler)
        {
            this.fileLocationQueryHandler = fileLocationQueryHandler;
        }

        public HttpResponseMessage Handle(Mp4Query query)
        {
            var fileLocation = fileLocationQueryHandler.Handle(new Mp4FileLocationQuery(query.FileName));
            
            var responseHandler = HttpResponseHandler.GetResponseHandler(query.Request, fileLocation);

            return responseHandler.GetResponse();
        }
    }
}