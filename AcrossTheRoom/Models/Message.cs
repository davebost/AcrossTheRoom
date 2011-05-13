using System;
using System.Windows.Media;


namespace AcrossTheRoom.Models
{
    public class Message
    {
        public string Text { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public int Speed { get; set; }
        public AnimationType AnimationType { get; set; }
        public int FontSize { get; set; }

        public Message()
        { }

    }

    public enum AnimationType
    {
        None,
        Scrolling,
        Flash
    }

}
