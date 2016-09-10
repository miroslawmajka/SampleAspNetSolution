using SampleVideoStreamingSite.Abstract;

namespace SampleVideoStreamingSite.Queries
{
    public class Mp4FileLocationQuery : IQuery<string>
    {
        public string FileName { get; private set; }

        public Mp4FileLocationQuery(string fileName)
        {
            FileName = fileName;
        }
    }
}