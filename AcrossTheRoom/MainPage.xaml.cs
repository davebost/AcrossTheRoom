﻿using System;
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
using Microsoft.Advertising.Mobile.UI;

namespace AcrossTheRoom
{
    public partial class MainPage : PhoneApplicationPage
    {
        const string AD_CENTER_APPLICATION_ID = "d37c36a7-37f5-4825-875f-cf3fdcfd7338";
        const string AD_CENTER_AD_UNIT_ID = "67648";

        MessageData _md = MessageData.Instance;

        AdControl adControl;
        string adCenterApplicationId;
        string adUnitId;

        public MainPage()
        {
            InitializeComponent();

#if DEBUG
            AdControl.TestMode = true;
            adCenterApplicationId = "test_client";
            adUnitId = "Image480x80";
#else
            AdControl.TestMode = false;
            adCenterApplicationId = AD_CENTER_APPLICATION_ID;
            adUnitId = AD_CENTER_AD_UNIT_ID;
#endif

            // Set up Ad Control
            AdControl adControl = new AdControl(adCenterApplicationId, // ApplicationID, d37c36a7-37f5-4825-875f-cf3fdcfd7338 
                                                adUnitId,    // AdUnitID
                                                AdModel.Contextual, // AdModel
                                                true);         // RotationEnabled
            adControl.Width = 480;
            adControl.Height = 80;

            // Add Ad Control to MainPage Layout
            Grid grid = (Grid)this.LayoutRoot.FindName("AdControlPanel");
            if (grid != null)
            {
                //grid.Children.Add(adControl);
                // Subscribe to this event if your application has multiple pages.
                // Single page applications do not need this code.
                this.Unloaded += new RoutedEventHandler(MainPage_Unloaded);
            }

            // Load the message list
            _md.Load();
        }

        void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)this.LayoutRoot.FindName("AdControlPanel");
            if (grid != null)
            {
                if (grid.Children.Contains(adControl))
                    grid.Children.Remove(adControl);

                this.Unloaded -= new RoutedEventHandler(MainPage_Unloaded);

                adControl = null;
            }
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
            if (MessageListBox.SelectedIndex == -1) return;

            string messageKey = ((Message)MessageListBox.SelectedItem).id.ToString();
            EditMessage(messageKey);

            MessageListBox.SelectedIndex = -1;
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