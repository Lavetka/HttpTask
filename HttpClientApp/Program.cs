
using System.Net;

namespace HttpClientNamespace
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Task 1: URL
                    HttpResponseMessage responseTask1 = await client.GetAsync("http://localhost:8888/MyName/");
                    responseTask1.EnsureSuccessStatusCode();
                    string myNameResponse = await responseTask1.Content.ReadAsStringAsync();
                    Console.WriteLine($"Task 1: My Name - {myNameResponse}");

                    // Task 2: HTTP status messages
                    string[] urlsTask2 =
                    {
                        "http://localhost:8888/Information/",
                        "http://localhost:8888/Success/",
                        "http://localhost:8888/Redirection/",
                        "http://localhost:8888/ClientError/",
                        "http://localhost:8888/ServerError/"
                    };

                    foreach (string url in urlsTask2)
                    {
                        HttpResponseMessage responseTask2 = await client.GetAsync(url);
                        HttpStatusCode statusCode = responseTask2.StatusCode;
                        Console.WriteLine($"Task 2: {url} - {statusCode}");
                    }

                    // Task 3: Header
                    HttpResponseMessage responseTask3 = await client.GetAsync("http://localhost:8888/MyNameByHeader/");
                    responseTask3.EnsureSuccessStatusCode();
                    string myNameHeader = responseTask3.Headers.GetValues("X-MyName").ToString();
                    Console.WriteLine($"Task 3: My Name by Header - {myNameHeader}");

                    // Task 4: Cookies
                    HttpResponseMessage responseTask4 = await client.GetAsync("http://localhost:8888/MyNameByCookies/");
                    responseTask4.EnsureSuccessStatusCode();
                    string myNameCookie = responseTask4.Headers.GetValues("Set-Cookie").ToString();
                    Console.WriteLine($"Task 4: My Name by Cookies - {myNameCookie}");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
