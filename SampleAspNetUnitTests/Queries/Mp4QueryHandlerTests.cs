using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SampleAspNetMvcWebProject.Abstract;
using SampleAspNetMvcWebProject.Queries;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SampleAspNetUnitTests.Queries
{
    [TestClass]
    public class Mp4QueryHandlerTests
    {
        private const string ASSET_FOLDER_NAME = "Assets";
        private const string TEST_FILE_NAME = "TestFile.txt";
        private string testFileLocation;

        [TestInitialize]
        public void Init()
        {
            testFileLocation = Path.Combine(new string[] {
                    Directory.GetCurrentDirectory(),
                    ASSET_FOLDER_NAME,
                    TEST_FILE_NAME
                });
        }

        [TestMethod]
        public void HandlerReturnsFullResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IQueryHandler<Mp4FileLocationQuery, string>>()
                    .Setup(x => x.Handle(It.IsAny<Mp4FileLocationQuery>()))
                    .Returns(testFileLocation);

                var subject = mock.Create<Mp4QueryHandler>();

                // Act
                var result = subject.Handle(new Mp4Query {
                    FileName = TEST_FILE_NAME,
                    Request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/")
                });

                // Assert
                Assert.IsTrue(result.IsSuccessStatusCode);
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.AreEqual(typeof(StreamContent), result.Content.GetType());
                Assert.AreEqual("OK", result.ReasonPhrase);
                Assert.AreEqual("video/mp4", result.Content.Headers.ContentType.MediaType);
                Assert.AreEqual("inline", result.Content.Headers.ContentDisposition.DispositionType);
                Assert.AreEqual(TEST_FILE_NAME, result.Content.Headers.ContentDisposition.FileName);
            }
        }

        [TestMethod]
        public void HandlerReturnsByteRangeResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IQueryHandler<Mp4FileLocationQuery, string>>()
                    .Setup(x => x.Handle(It.IsAny<Mp4FileLocationQuery>()))
                    .Returns(testFileLocation);

                var subject = mock.Create<Mp4QueryHandler>();

                // Act
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/");
                requestMessage.Headers.Range = new RangeHeaderValue(0, 0) {
                    Unit = "bytes"
                };

                var result = subject.Handle(new Mp4Query {
                    FileName = TEST_FILE_NAME,
                    Request = requestMessage
                });

                // Assert
                Assert.IsTrue(result.IsSuccessStatusCode);
                Assert.AreEqual(HttpStatusCode.PartialContent, result.StatusCode);
                Assert.AreEqual("Partial Content", result.ReasonPhrase);
                Assert.AreEqual(typeof(ByteRangeStreamContent), result.Content.GetType());
                Assert.AreEqual("video/mp4", result.Content.Headers.ContentType.MediaType);
                Assert.AreEqual("inline", result.Content.Headers.ContentDisposition.DispositionType);
                Assert.AreEqual(TEST_FILE_NAME, result.Content.Headers.ContentDisposition.FileName);
            }
        }

        [TestMethod]
        public void HandlerReturnsHeadResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Arrange
                mock.Mock<IQueryHandler<Mp4FileLocationQuery, string>>()
                    .Setup(x => x.Handle(It.IsAny<Mp4FileLocationQuery>()))
                    .Returns(testFileLocation);

                var subject = mock.Create<Mp4QueryHandler>();

                // Act
                var result = subject.Handle(new Mp4Query {
                    FileName = TEST_FILE_NAME,
                    Request = new HttpRequestMessage(HttpMethod.Head, "http://www.test.com/")
                });

                // Assert
                Assert.IsTrue(result.IsSuccessStatusCode);
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
                Assert.AreEqual("OK", result.ReasonPhrase);
                Assert.AreEqual(typeof(StringContent), result.Content.GetType());
                Assert.AreEqual("video/mp4", result.Content.Headers.ContentType.MediaType);
            }
        }
    }
}