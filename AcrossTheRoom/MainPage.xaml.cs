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

        private void MessageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string messageKey = ((Message)MessageListBox.SelectedItem).id.ToString();
            EditMessage(messageKey);
        }

        private void ContextMenuItem_Edit(object sender, RoutedEventArgs e)
        {
            string messageKey = ((Message)((FrameworkElement)sender).DataContext).id.ToString();
            EditMessage(messageKey);
        }

        private void ContextMenuItem_Play(object sender, RoutedEventArgs e)
        {
            string messageKey = ((Message)((FrameworkElement)sender).DataContext).id.ToString();
            PlayMessage(messageKey);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            string messageKey = ((Message)((FrameworkElement)sender).DataContext).id.ToString();
            PlayMessage(messageKey);
        }

        private void PlayMessage(string messageKey)
        {
            NavigationService.Navigate(new Uri(String.Format("/DisplayBoardPage.xaml?id={0}", messageKey), UriKind.Relative));
        }

        private void EditMessage(string messageKey)
        {
            NavigationService.Navigate(new Uri(String.Format("/EditMessagePage.xaml?id={0}", messageKey), UriKind.Relative));
        }

    }
}