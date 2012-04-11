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
using System.IO;
using Microsoft.Phone.Tasks;

namespace AcrossTheRoom
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Read ReleaseNotes.htm
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("AcrossTheRoom.ChangeLog.xaml"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string xaml = reader.ReadToEnd();
                        var x = System.Windows.Markup.XamlReader.Load(xaml);
                        if (x != null)
                        {
                            WhatsNew.Content = x;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in loading What's New XAML: " + ex.Message.ToString());
            }
        }

        private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask();
            task.Subject = "Feedback for AcrossTheRoom";
            task.To = "corganinteractive@live.com";
            task.Show();
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }

    }
}
