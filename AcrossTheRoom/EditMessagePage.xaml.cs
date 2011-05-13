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

namespace AcrossTheRoom
{
    public partial class EditMessagePage : PhoneApplicationPage
    {
        private Message _currentMessage = new Message();
        private Boolean _newMessage = true;

        // Constructor
        public EditMessagePage()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            MessageData md = MessageData.Instance;

            if (MessageTextBox.Text.Trim().Length > 0)
            {
                _currentMessage.Text = MessageTextBox.Text;
                _currentMessage.AnimationType = (AnimationType)Enum.Parse(typeof(AnimationType), AnimationListPicker.SelectedItem.ToString(), true);
                _currentMessage.Speed = (int)SpeedSlider.Value;
                _currentMessage.FontSize = (int)FontSizeSlider.Value;

                if (_newMessage)
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
            this.DataContext = _currentMessage;
        }

        private void returnToPreviousPage()
        {
            NavigationService.GoBack();
        }



    }
}