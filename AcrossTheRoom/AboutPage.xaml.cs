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
        bool isBrowserLoaded = false;
        string _releaseNotesHtml;

        public AboutPage()
        {
            InitializeComponent();

            this.AboutPivot.LoadedPivotItem += new EventHandler<PivotItemEventArgs>(AboutPivot_LoadedPivotItem);

        }

        void AboutPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (e.Item.Name == "WhatsNew")
            {
                if (!isBrowserLoaded)
                {
                    webBrowser.NavigateToString(_releaseNotesHtml);
                    isBrowserLoaded = true;
                    webBrowser.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Read ReleaseNotes.htm
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("AcrossTheRoom.ReleaseNotes.htm"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    _releaseNotesHtml = reader.ReadToEnd();
                    if (_releaseNotesHtml.Length > 0) _releaseNotesHtml = applyStyleToHtml(_releaseNotesHtml);
                }
            }

        }

        private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask();
            task.Subject = "Feedback for AcrossTheRoom";
            task.To = "corganinteractive@live.com";
            task.Show();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (((Pivot)sender).SelectedIndex == 1)
            //{
            //    if (!isBrowserLoaded)
            //    {
            //        webBrowser.NavigateToString(applyStyleToHtml(_releaseNotesHtml));
            //        isBrowserLoaded = true;
            //        webBrowser.Visibility = System.Windows.Visibility.Visible;
            //    }
            //}
        }

        private string applyStyleToHtml(string html)
        {
            // apply style based on the current theme

            string foregroundColor;
            string backgroundColor;

            return string.Format(html, this.webBrowser.Width.ToString(), "black", "white", "12");
        }


    }
}
