using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using SampleVideoStreamingSite.Abstract;
using SampleVideoStreamingSite.Queries;
using System.IO;
using System.Net.Http;

namespace SampleVideoStreamingSiteUnitTests.Queries
{
    [TestClass]
    public class Mp4QueryHandlerTests
    {
        [TestMethod]
        public void HandlerReturnsFullResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testFileName = "TestFile.txt";
                var currentDirectory = Directory.GetCurrentDirectory();
                var testFileLocation = Path.Combine(new string[] { currentDirectory, "Assets", testFileName });

                mock.Mock<IQueryHandler<Mp4FileLocationQuery, string>>()
                    .Setup(x => x.Handle(It.IsAny<Mp4FileLocationQuery>()))
                    .Returns(testFileLocation);

                var subject = mock.Create<Mp4QueryHandler>();

                var result = subject.Handle(new Mp4Query {
                    FileName = testFileName,
                    Request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/")
                });

                // TODO: assert and add more scenarios
            }
        }

        [TestMethod]
        public void HandlerReturnsHeadResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testFileName = "TestFile.txt";
                var currentDirectory = Directory.GetCurrentDirectory();
                var testFileLocation = Path.Combine(new string[] { currentDirectory, "Assets", testFileName });

                mock.Mock<IQueryHandler<Mp4FileLocationQuery, string>>()
                    .Setup(x => x.Handle(It.IsAny<Mp4FileLocationQuery>()))
                    .Returns(testFileLocation);

                var subject = mock.Create<Mp4QueryHandler>();

                var result = subject.Handle(new Mp4Query
                {
                    FileName = testFileName,
                    Request = new HttpRequestMessage(HttpMethod.Head, "http://www.test.com/")
                });

                // TODO: assert and add more scenarios
            }
        }

        private void SetUpMock(ref AutoMock mock)
        {
            var testFileName = "TestFile.txt";
            var currentDirectory = Directory.GetCurrentDirectory();
            var testFileLocation = Path.Combine(new string[] { currentDirectory, "Assets", testFileName });

            mock.Mock<IQueryHandler<Mp4FileLocationQuery, string>>()
                .Setup(x => x.Handle(It.IsAny<Mp4FileLocationQuery>()))
                .Returns(testFileLocation);
        }
    }
}
