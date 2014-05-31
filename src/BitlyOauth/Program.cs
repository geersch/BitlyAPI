using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace BitlyOauth
{
    class Program
    {
        private const string clientId = "your_client_id";
        private const string clientSecret = "your_client_secret";

        static void Main(string[] args)
        {
            var redirectUri = "your_redirect_uri";

            // Step 1 - Authorize the application
            var uri = "https://bitly.com/oauth/authorize";

            var authorizeUri = new StringBuilder(uri);
            authorizeUri.AppendFormat("?client_id={0}&", clientId);
            authorizeUri.AppendFormat("redirect_uri={0}", redirectUri);

            var startInfo = new ProcessStartInfo();
            startInfo.FileName = authorizeUri.ToString();
            Process.Start(startInfo);

            // Step 2 - Get the OAuth access token
            var code = "your_code";
            var requestUri = new StringBuilder("https://api-ssl.bitly.com/oauth/access_token");
            requestUri.AppendFormat("?client_id={0}&", clientId);
            requestUri.AppendFormat("client_secret={0}&", clientSecret);
            requestUri.AppendFormat("code={0}&", code);
            requestUri.AppendFormat("redirect_uri={0}", redirectUri);

            var request = (HttpWebRequest)WebRequest.Create(requestUri.ToString());
            request.Method = WebRequestMethods.Http.Post;

            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var accessToken = reader.ReadToEnd();

                var parts = accessToken.Split('&');
                var token = parts[0].Substring(parts[0].IndexOf('=') + 1);
                var login = parts[1].Substring(parts[1].IndexOf('=') + 1);
                var apiKey = parts[2].Substring(parts[2].IndexOf('=') + 1);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
