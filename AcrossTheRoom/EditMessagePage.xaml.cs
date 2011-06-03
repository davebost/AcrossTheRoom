using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using AcrossTheRoom.Models;
using System.Reflection;

namespace AcrossTheRoom
{
    public partial class EditMessagePage : PhoneApplicationPage
    {
        private Message _currentMessage;
        private Boolean _isNewMessage = true;

        //List<Color> _colorList = new List<Color> {Colors.Black, Colors.Blue, Colors.Brown, Colors.Cyan, Colors.DarkGray, 
        //    Colors.Gray, Colors.Green, Colors.LightGray, Colors.Magenta, Colors.Orange, Colors.Purple, Colors.Red, Colors.White, Colors.Yellow };

        readonly string[] ColorList = { "Black", "Gray", "LightGray", "White", "Magenta", "Purple", "Brown", "Cyan", "Pink", "Orange", "Blue", "Red", "Green", "Yellow" };

        // Constructor
        public EditMessagePage()
        {
            InitializeComponent();

            BackgroundListPicker.ItemsSource = ColorList;
            ForegroundListPicker.ItemsSource = ColorList;
            
            this.BackgroundListPicker.SelectionChanged += new SelectionChangedEventHandler(BackgroundListPicker_SelectionChanged);
            this.ForegroundListPicker.SelectionChanged += new SelectionChangedEventHandler(ForegroundListPicker_SelectionChanged);

            BackgroundListPicker.SelectedItem = "LightGray";
            ForegroundListPicker.SelectedItem = "Black";
        }

        void ForegroundListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string colorString = this.ForegroundListPicker.SelectedItem.ToString();
            MessageTextBox.Foreground = ColorHelper.GetBrushFromString(colorString);
        }

        void BackgroundListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string colorString = this.BackgroundListPicker.SelectedItem.ToString();
            MessageTextBox.Background = ColorHelper.GetBrushFromString(colorString);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            MessageData md = MessageData.Instance;

            if (MessageTextBox.Text.Trim().Length > 0)
            {
                _currentMessage.Text = MessageTextBox.Text;
                _currentMessage.AnimationType = (AnimationType)Enum.Parse(typeof(AnimationType), AnimationListPicker.SelectedItem.ToString(), true);
                _currentMessage.Speed = (int)SpeedSlider.Value;
                _currentMessage.FontScale = (int)FontScaleSlider.Value;
                _currentMessage.ForegroundColor = ForegroundListPicker.SelectedItem.ToString();
                _currentMessage.BackgroundColor = BackgroundListPicker.SelectedItem.ToString();

                if (_isNewMessage)
                    md.Messages.Add(_currentMessage);

                md.Save();
            }
            returnToPreviousPage();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            returnToPreviousPage();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
        //    this.DataContext = _currentMessage;
        }

        private void returnToPreviousPage()
        {
            NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string id = "";
            if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                Message msg = MessageData.Instance.Messages.FirstOrDefault(m => m.id == id);

                // Edit Message
                _isNewMessage = false;

                _currentMessage = msg;

            }
            else
            {
                // New Message
                _currentMessage = new Message();
                _isNewMessage = true;
            }

            this.DataContext = _currentMessage;
        }


    }
}