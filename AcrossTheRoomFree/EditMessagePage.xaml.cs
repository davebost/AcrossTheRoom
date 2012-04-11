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
using Microsoft.Advertising.Mobile.UI;

namespace AcrossTheRoom
{
    public partial class EditMessagePage : PhoneApplicationPage
    {
        const string EDIT_STATE_KEY = "CurrentEditMessage";

        private Message _currentMessage;
        private Boolean _isNewMessage = true;

        readonly string[] ColorList = { "Black", "Gray", "LightGray", "White", "Magenta", "Purple", "Brown", "Cyan", "Orange", "Blue", "Red", "Green", "Yellow" };

        // Constructor
        public EditMessagePage()
        {
            InitializeComponent();

            System.Diagnostics.Debug.WriteLine("EditMessagePage:ctor()");

            BackgroundListPicker.ItemsSource = ColorList;
            ForegroundListPicker.ItemsSource = ColorList;
            
            this.BackgroundListPicker.SelectionChanged += new SelectionChangedEventHandler(BackgroundListPicker_SelectionChanged);
            this.ForegroundListPicker.SelectionChanged += new SelectionChangedEventHandler(ForegroundListPicker_SelectionChanged);

            BackgroundListPicker.SelectedItem = "LightGray";
            ForegroundListPicker.SelectedItem = "Black";
            
            EditAdControl.AdControlError += new EventHandler<ErrorEventArgs>(EditAdControl_AdControlError);

#if PAID
            if (!App.IsTrial)
            {
                // Remove Ads for paying customers
                Grid grid = (Grid)this.LayoutRoot.FindName("AdControlPanel");
                if (grid != null)
                {
                    LayoutRoot.Children.Remove(grid);
                }
            }
            else
            {
                EditAdControl.Visibility = System.Windows.Visibility.Visible;
            }
#else
            EditAdControl.Visibility = System.Windows.Visibility.Visible;
#endif
        }

        void EditAdControl_AdControlError(object sender, ErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("{0} - {1}", e.ErrorCode, e.ErrorDescription));
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
                saveFormFields();

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

        private void returnToPreviousPage()
        {
            clearStateKey();
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            System.Diagnostics.Debug.WriteLine("EditMessagePage:OnNavigatedTo");

            string id = "";
            if (this.State.ContainsKey(EDIT_STATE_KEY))
            {
                // Retrieve Message from state
                Message stateMessage = (Message)this.State[EDIT_STATE_KEY];

                // Strange data binding behavior; on Activated from Tombstoning, we need to pull reference from 
                // our persistent storage and manually set values based on the last state of the form
                // TODO: Investigate weird data binding issue
                _currentMessage = MessageData.Instance.Messages.FirstOrDefault(m => m.ID == stateMessage.ID);
                if (_currentMessage != null)
                {
                    // Build form from state values
                    _currentMessage.Text = stateMessage.Text;
                    _currentMessage.AnimationType = stateMessage.AnimationType;
                    _currentMessage.Speed = stateMessage.Speed;
                    _currentMessage.FontScale = stateMessage.FontScale;
                    _currentMessage.ForegroundColor = stateMessage.ForegroundColor;
                    _currentMessage.BackgroundColor = stateMessage.BackgroundColor;
                    _isNewMessage = false;
                }
                else
                {
                    // if it's a new message, no need to manually set property values, a reference will do
                    _currentMessage = stateMessage;
                    _isNewMessage = true;
                }
            }
            else if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                _currentMessage = MessageData.Instance.Messages.FirstOrDefault(m => m.ID == id);
                if (_currentMessage != null)
                {
                    _isNewMessage = false;
                }
            }

            if (_currentMessage == null)
            {
                // New Message
                _currentMessage = new Message();
                _isNewMessage = true;
            }

            this.DataContext = _currentMessage;


        
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            System.Diagnostics.Debug.WriteLine("EditMessagePage:OnNavigatedFrom");

            // Determine if anything other than back button was pressed
            if (e.Uri.ToString().StartsWith("app"))
            {
                // Selecting an app button doesn't trigger the lost focus, therefore the current databinding field won't be updated
            //    saveFormFields();
                this.State[EDIT_STATE_KEY] = this.DataContext;
            }

        }

        private void DeleteMessage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this message?", "Delete Message", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                //Delete Message and return to main page
                MessageData.Instance.Messages.Remove(_currentMessage);
                MessageData.Instance.Save();

                returnToPreviousPage();
            }
        }

        private void saveFormFields()
        {
            string itemValue = AnimationListPicker.SelectedItem.ToString().Replace(" ", "");

            _currentMessage = (Message)this.DataContext;
            _currentMessage.Text = MessageTextBox.Text;
            _currentMessage.AnimationType = (AnimationType)Enum.Parse(typeof(AnimationType), itemValue, true);
            _currentMessage.Speed = (int)SpeedSlider.Value;
            _currentMessage.FontScale = (int)FontScaleSlider.Value;
            _currentMessage.ForegroundColor = ForegroundListPicker.SelectedItem.ToString();
            _currentMessage.BackgroundColor = BackgroundListPicker.SelectedItem.ToString();
        }

        private void clearStateKey()
        {
            if (this.State.ContainsKey(EDIT_STATE_KEY)) this.State.Remove(EDIT_STATE_KEY);
        }

        private void HelpPage_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }

    }
}