using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using twitch.Models;

namespace twitch
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("/stream");
                    if (response.IsSuccessStatusCode)
                    {
                        var product = await response.Content.ReadAsAsync<Stream>();
                    }
                    else
                    {
                        Console.WriteLine("gg");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.InnerException);
                }
            }
        }
    }
}


