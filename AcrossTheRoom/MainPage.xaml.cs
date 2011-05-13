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
    public partial class MainPage : PhoneApplicationPage
    {
        MessageData _md = MessageData.Instance;

        public MainPage()
        {
            InitializeComponent();
            _md.Load();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditMessagePage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            MessageListBox.ItemsSource = _md.Messages;
        }

        private void MenuItem_Edit(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Play(object sender, RoutedEventArgs e)
        {

        }
    }
}