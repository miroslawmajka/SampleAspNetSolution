using SampleAspNetMvcWebProject.Abstract;
using SampleAspNetMvcWebProject.Queries;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SampleAspNetMvcWebProject.Controllers.Api
{
    public class Mp4Controller : ApiController
    {
        private readonly IQueryHandler<Mp4Query, HttpResponseMessage> mp4QueryHandler;

        public Mp4Controller(IQueryHandler<Mp4Query, HttpResponseMessage> mp4QueryHandler)
        {
            this.mp4QueryHandler = mp4QueryHandler;
        }

        [HttpGet]
        [HttpHead]
        public HttpResponseMessage Get([FromUri]Mp4Query query)
        {
            if (query == null) throw new HttpResponseException(HttpStatusCode.BadRequest);

            query.Request = Request;

            return mp4QueryHandler.Handle(query);
        }
    }
}