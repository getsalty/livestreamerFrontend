using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using twitch.Models;

namespace twitchFrontend
{
    public partial class Form1 : Form
    {
        List<Streamer> _datasource = new List<Streamer>();

        public Form1()
        {
            InitializeComponent();

            _datasource.Add(new Streamer { name = "hi", url = "/stream", online = false });
            _datasource.Add(new Streamer { name = "bye", url = "/sss", online = false });
            _datasource.Add(new Streamer { name = "hello", url = "/stream2", online = false });

            dataGridView1.DataSource = _datasource;

            //Task llamaq = RunAsync2();
            Task.Run(() => RunAsync()).Wait();
        }


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
                    dataGridView1.SuspendLayout();
                    foreach (var item in _datasource)
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
                    dataGridView1.ResumeLayout();
                    dataGridView1.Refresh();
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

        async Task RunAsync()
        {
            var time2 = new Stopwatch();
            time2.Start();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    dataGridView1.SuspendLayout();

                    ////
                    //    PARALLEL PROCESSING
                    ////
                    var tasks = new List<Task>();
                    foreach (var item in _datasource)
                    {
                        tasks.Add(RetrieveStreamInfo(client, item));
                    }
                    await Task.WhenAll(tasks);
                    dataGridView1.ResumeLayout();
                    dataGridView1.Refresh();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.InnerException);
                }
            }

            time2.Stop();
            var ts = time2.Elapsed;
            var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Debug.WriteLine("RunTime for ASYNC1 " + elapsedTime);
        }

        async Task RetrieveStreamInfo(HttpClient client, Streamer item)
        {

            HttpResponseMessage response = await client.GetAsync(item.url);
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsAsync<TEST>();
                if (product.Name == item.name)
                    item.online = true;
                //var product = await response.Content.ReadAsAsync<Stream>();
                //if (product.channel.name == item.name)
                //    item.online = true;
            }
            else
            {
                Debug.WriteLine("Dangit");

                //this.pictureBox1.Image = global::twitchFrontend.Properties.Resources.red;
            }

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

        void cellFormattingImage(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.Columns[e.ColumnIndex].Name == "online")
            {
                e.Value = (bool)e.Value ? (System.Drawing.Image)Properties.Resources.green : (System.Drawing.Image)Properties.Resources.red;
                e.FormattingApplied = true;
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
    }
}
