namespace twitch.Models
{
    class Stream
    {
        public string game { get; set; }
        public int viewers { get; set; }
        public double average_fps { get; set; }
        public int delay { get; set; }
        public int video_height { get; set; }
        public bool is_playlist { get; set; }
        public string created_at { get; set; }
        public int _id { get; set; }
        public Channel channel { get; set; }
    }

}
