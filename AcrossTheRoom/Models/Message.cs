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
                if (_foregroundColor == null || _foregroundColor.Length == 0) _foregroundColor = "Black";
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
                if (_backgroundColor == null || _backgroundColor.Length == 0) _backgroundColor = "White";
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
                if (_speed == 0) _speed = 3;
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
                if (_fontScale == 0) _fontScale = 3;
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

}
