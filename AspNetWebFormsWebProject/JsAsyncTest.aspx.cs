using System;
using System.Globalization;
using System.Threading;
using System.Web.Services;

namespace AspNetWebFormsWebProject
{
    public partial class JsAsyncTest : System.Web.UI.Page
    {
        [WebMethod]
        public static string GetCurrentTime(string name)
        {
            var timeString = DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);

            Thread.Sleep(new Random().Next(1000, 5000));

            return $"Hello, {name}. Current server time: {timeString}.";
        }
    }
}