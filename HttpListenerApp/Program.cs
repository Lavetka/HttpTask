using System.Net;
using System.Text;

namespace HttpListenerNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add("http://localhost:8888/");
                listener.Start();
                Console.WriteLine("Listener started. Waiting for connections...");

                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    string requestUrl = context.Request.Url.AbsolutePath.ToLower();

                    if (requestUrl == "/myname/")
                    {
                        GetMyName(context);
                    }
                    else if (requestUrl == "/information/")
                    {
                        SendResponse(context, HttpStatusCode.Continue);
                    }
                    else if (requestUrl == "/success/")
                    {
                        SendResponse(context, HttpStatusCode.OK);
                    }
                    else if (requestUrl == "/redirection/")
                    {
                        SendResponse(context, HttpStatusCode.Redirect);
                    }
                    else if (requestUrl == "/clienterror/")
                    {
                        SendResponse(context, HttpStatusCode.BadRequest);
                    }
                    else if (requestUrl == "/servererror/")
                    {
                        SendResponse(context, HttpStatusCode.InternalServerError);
                    }
                    else if (requestUrl == "/mynamebyheader/")
                    {
                        GetMyNameByHeader(context);
                    }
                    else if (requestUrl == "/mynamebycookies/")
                    {
                        GetMyNameByCookies(context);
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }

                    context.Response.Close();
                }
            }
        }

        static void GetMyName(HttpListenerContext context)
        {
            string responseMessage = "Your Name";
            byte[] buffer = Encoding.UTF8.GetBytes(responseMessage);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        static void SendResponse(HttpListenerContext context, HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)statusCode;
        }

        static void GetMyNameByHeader(HttpListenerContext context)
        {
            context.Response.Headers.Add("X-MyName", "Your Name");

            string responseMessage = "Response with custom header \"X-MyName\" has been sent.";
            byte[] buffer = Encoding.UTF8.GetBytes(responseMessage);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        static void GetMyNameByCookies(HttpListenerContext context)
        {
            Cookie myNameCookie = new Cookie("MyName", "Your Name");
            context.Response.SetCookie(myNameCookie);

            string responseMessage = "Response with cookie \"MyName\" has been sent.";
            byte[] buffer = Encoding.UTF8.GetBytes(responseMessage);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}
