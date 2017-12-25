using System.IO;
#pragma warning disable 1591

namespace CastleGo.WebApi.Models
{
    /// <summary>
    /// Represents a model that contain information and data about received HttpRequest.
    /// </summary>
    public class HttpRequestModel
    {
        public string Method { get; set; }

        public string Host { get; set; }

        public string PathBase { get; set; }

        public string Path { get; set; }

        public string QueryString { get; set; }

        public string Scheme { get; set; }

        public string Protocol { get; set; }

        public string Body { get; set; }

        public string Uri
        {
            get
            {
                string str;
                if (!string.IsNullOrWhiteSpace(this.QueryString))
                    str = this.Scheme + "://" + this.Host + this.PathBase + this.Path + this.QueryString;
                else
                    str = this.Scheme + "://" + this.Host + this.PathBase + this.Path;
                return str;
            }
        }

        public static string ConvertToString(Stream stream)
        {
            stream.Position = 0L;
            using (StreamReader streamReader = new StreamReader(stream))
                return streamReader.ReadToEnd();
        }
    }
}
