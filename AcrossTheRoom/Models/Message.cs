using System;
using System.Windows.Media;
using System.ComponentModel;


namespace AcrossTheRoom.Models
{
    public class Message : INotifyPropertyChanged
    {
        public string id { get; private set; }

        string _text = "";
        public string Text {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                NotifyPropertyChanged("Text");
            }
 
        }

        private string _foregroundColor;
        public string ForegroundColor
        {
            get
            {
                return _foregroundColor;
            }
            set {
                _foregroundColor = value;
                NotifyPropertyChanged("ForegroundColor");
            } 
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                NotifyPropertyChanged("BackgroundColor");
            }
        }

        private int _speed;
        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                NotifyPropertyChanged("Speed");
            }
        }

        private AnimationType _animationType;
        public AnimationType AnimationType
        {
            get
            {
                return _animationType;
            }
            set
            {
                _animationType = value;
                NotifyPropertyChanged("Color");
            }
        }

        private int _fontScale;
        public int FontScale
        {
            get
            {
                return _fontScale;
            }
            set
            {
                _fontScale = value;
                NotifyPropertyChanged("FontScale");
            }
        }

        public Message()
        {
            id = Guid.NewGuid().ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }

    public enum AnimationType
    {
        None,
        Scrolling,
        Flash
    }

}
