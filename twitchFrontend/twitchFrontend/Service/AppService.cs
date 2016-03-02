using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using twitch.Models;

namespace twitchFrontend.Service
{
    class AppService
    {
        List<Streamer> _datasourceStreamer;

        public AppService(List<Streamer> _datasource)
        {
            _datasourceStreamer = _datasource;
        }

        public async Task UpdateStreamers()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var tasks = new List<Task>();
                    foreach (var item in _datasourceStreamer)
                    {
                        tasks.Add(RetrieveStreamInfo(client, item));
                    }
                    await Task.WhenAll(tasks);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.InnerException);
                }
            }
        }

        async Task RetrieveStreamInfo(HttpClient client, Streamer item)
        {
            HttpResponseMessage response = await client.GetAsync(item.url);
            if (response.IsSuccessStatusCode)
            {
                //var product = await response.Content.ReadAsAsync<TEST>();
                //if (product.Name == item.name)
                //    item.online = true;
                var product = await response.Content.ReadAsAsync<Stream>();
                if (product.channel.name == item.name)
                {
                    item.online = true;
                    item.viewers = product.viewers;
                }
            }
            else
            {
                Debug.WriteLine("Dangit");

                //this.pictureBox1.Image = global::twitchFrontend.Properties.Resources.red;
            }
        }

        class TEST
        {
            string name;

            public string Name
            {
                get
                {
                    return name;
                }

                set
                {
                    name = value;
                }
            }
        }

        public void DisplayStream(List<Streamer> selectedStreamers)
        {
            var sb = new StringBuilder();
            foreach (var item in selectedStreamers)
            {
                sb.Clear();
                sb.Append("/c livestreamer twitch.tv/");
                sb.Append(item.name);
                sb.Append(" ");
                sb.Append("source");
                var process = new Process();
                var startInfo = new ProcessStartInfo();

                //WindowStyle = ProcessWindowStyle.Hidden,
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = sb.ToString();

                process.StartInfo = startInfo;
                process.Start();
            }
        }


        //----------------------------------------------------

        async Task RunAsync2()
        {
            var time = new Stopwatch();
            time.Start();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    foreach (var item in _datasourceStreamer)
                    {
                        HttpResponseMessage response = await client.GetAsync(item.url);
                        if (response.IsSuccessStatusCode)
                        {
                            var product = await response.Content.ReadAsAsync<TEST>();
                            if (product.Name == item.name)
                                item.online = true;
                        }
                        else
                        {
                            Debug.WriteLine("Dangit");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.InnerException);
                }
            }

            time.Stop();
            var ts = time.Elapsed;
            var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Debug.WriteLine("RunTime " + elapsedTime);
        }

        async Task RunAsync22()
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
                        var product = await response.Content.ReadAsAsync<TEST>();
                        Console.WriteLine("Got it");
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
