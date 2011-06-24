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
    public partial class DisplayBoardPage : PhoneApplicationPage
    {
        const int FONT_SCALE_MULTIPLYER = 25;
        const int SPEED_MULTIPLIER = 1;
        const int MAX_TIME = 15;
        const int PIXELS_PER_SECOND = 200;
        const int MAX_SPEED = 5;
        const double SPEED_INTERVAL = .3;

        Message _message;

        public DisplayBoardPage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(DisplayBoardPage_Loaded);

        }

        void DisplayBoardPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_message != null)
            {
                switch (_message.AnimationType)
                {
                    case AnimationType.SlideDown:
                        {
                            SlideDown();
                            return;
                        }
                    case AnimationType.Flash:
                        {
                            Flash();
                            return;
                        }
                    case AnimationType.None:
                        {
                            DisplayStaticMessage();
                            return;
                        }
                    default:
                        {
                            RightToLeftMarquee();
                            return;
                        }
                }
            }
            else
            {
                MessageBox.Show("Message not found.");
                NavigationService.Navigate(new Uri("/Mainpage.xaml", UriKind.Relative));
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string id = "";
            if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                System.Diagnostics.Debug.WriteLine("id: " + id.ToString());
                Message msg = MessageData.Instance.Messages.FirstOrDefault(m => m.ID == id);
                if (msg != null)
                {
                    _message = msg;
                    this.DataContext = _message;
                }
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void RightToLeftMarquee()
        {
            MarqueeTextBlock.FontSize = getFontSize();

            MainCanvas.Background = ColorHelper.GetBrushFromString(_message.BackgroundColor);
            MarqueeTextBlock.Foreground = ColorHelper.GetBrushFromString(_message.ForegroundColor);

            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            rectangleGeometry.Rect = new Rect(new Point(0, 0), new Size(2000, 480));
            MainCanvas.Clip = rectangleGeometry;

            MarqueeTextBlock.Text = _message.Text;

            MarqueeTextBlock.Width = MarqueeTextBlock.ActualWidth;
            double height = 480 - MarqueeTextBlock.ActualHeight;
            MarqueeTextBlock.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 800;
            doubleAnimation.To = -MarqueeTextBlock.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Calculate duration
            double marqueeWidth = MarqueeTextBlock.ActualWidth;

            int speed = (_message.Speed == 0 ? 0 : _message.Speed);
            double duration = marqueeWidth / (speed * PIXELS_PER_SECOND);

            System.Diagnostics.Debug.WriteLine("Width: " + marqueeWidth.ToString() + "         Duration: " + duration.ToString());
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(duration));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, MarqueeTextBlock);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Begin();

        }

        private int getFontSize()
        {
            int fontScale = (_message.FontScale == 0 ? 1 : _message.FontScale);
            return 150 + (fontScale * FONT_SCALE_MULTIPLYER);
        }

        private void SlideDown()
        {
            Storyboard storyBoard = new Storyboard();

            MainCanvas.Children.Clear();
            MainCanvas.Background = ColorHelper.GetBrushFromString(_message.BackgroundColor);

            string text = _message.Text;

            string[] words = text.Split(' ');
            for (var i = 0; i <= words.Length - 1; i++)
            {
                Border b = new Border();
                b.BorderThickness = new Thickness(0);
                b.Width = 800;
                b.Height = 480;

                TextBlock tb = getTextBlock();
                tb.Text = words[i];
                tb.FontSize = getFontSize();
                tb.Foreground = ColorHelper.GetBrushFromString(_message.ForegroundColor);
                while (tb.ActualWidth > 800)
                {
                    tb.FontSize -= 1;
                    System.Diagnostics.Debug.WriteLine("FontSize: " + tb.FontSize);
                }
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                b.Child = tb;
                b.SetValue(Canvas.TopProperty, Convert.ToDouble(-480));

                MainCanvas.Children.Add(b);

                double seconds = ((MAX_SPEED - _message.Speed) + 1) * SPEED_INTERVAL;

                double startingTime = (i * 2) * seconds;

                EasingDoubleKeyFrame edkf;

                // Hide elements during repeat

                DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();

                CircleEase ce = new CircleEase();
                ce.EasingMode = EasingMode.EaseOut;

                edkf = new EasingDoubleKeyFrame();
                edkf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(startingTime));
                edkf.Value = -480;
                edkf.EasingFunction = ce;
                animation.KeyFrames.Add(edkf);

                edkf = new EasingDoubleKeyFrame();
                edkf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(startingTime + seconds));
                edkf.Value = 0;
                edkf.EasingFunction = ce;
                animation.KeyFrames.Add(edkf);

                edkf = new EasingDoubleKeyFrame();
                edkf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(startingTime + (seconds * 2)));
                edkf.Value = 0;
                edkf.EasingFunction = ce;
                animation.KeyFrames.Add(edkf);

                edkf = new EasingDoubleKeyFrame();
                edkf.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(startingTime + (seconds * 3)));
                edkf.Value = 480;
                edkf.EasingFunction = ce;
                animation.KeyFrames.Add(edkf);

                Storyboard.SetTarget(animation, b);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Top)"));

                storyBoard.Children.Add(animation);

            }

            if (storyBoard.Children.Count > 0)
            {
                storyBoard.RepeatBehavior = RepeatBehavior.Forever;
                storyBoard.Begin();
            }

        }

        private void Flash()
        {
            Storyboard storyBoard = new Storyboard();

            MainCanvas.Children.Clear();
            MainCanvas.Background = ColorHelper.GetBrushFromString(_message.BackgroundColor);

            Grid g = new Grid();
            g.Width = 800;
            g.Height = 480;
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.RowDefinitions.Add(new RowDefinition());
            MainCanvas.Children.Add(g);

            string text = _message.Text;

            string[] words = text.Split(' ');
            for (var i = 0; i <= words.Length - 1; i++)
            {
                TextBlock tb = getTextBlock();
                tb.Text = words[i];
                tb.FontSize = getFontSize();
                tb.Foreground = ColorHelper.GetBrushFromString(_message.ForegroundColor);
                while (tb.ActualWidth > 800)
                {
                    tb.FontSize -= 1;
                    System.Diagnostics.Debug.WriteLine("FontSize: " + tb.FontSize);
                }
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tb.Visibility = System.Windows.Visibility.Collapsed;

                g.Children.Add(tb);

                double seconds = ((MAX_SPEED - _message.Speed) + 1) * SPEED_INTERVAL;

                double startingTime = ((i * 2) + 1) * seconds;

                ObjectAnimationUsingKeyFrames animation = new ObjectAnimationUsingKeyFrames();

                DiscreteObjectKeyFrame visibleKeyFrame = new DiscreteObjectKeyFrame();
                visibleKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(startingTime));
                visibleKeyFrame.Value = Visibility.Visible;
                animation.KeyFrames.Add(visibleKeyFrame);

                DiscreteObjectKeyFrame collapsedKeyFrame = new DiscreteObjectKeyFrame();
                collapsedKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(startingTime + seconds));
                collapsedKeyFrame.Value = Visibility.Collapsed;
                animation.KeyFrames.Add(collapsedKeyFrame);

                Storyboard.SetTarget(animation, tb);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Visibility)"));

                storyBoard.Children.Add(animation);

            }

            if (storyBoard.Children.Count > 0)
            {
                storyBoard.RepeatBehavior = RepeatBehavior.Forever;
                storyBoard.Begin();
            }
        }

        private void DisplayStaticMessage()
        {
            MainCanvas.Children.Clear();
            MainCanvas.Background = ColorHelper.GetBrushFromString(_message.BackgroundColor);

            Grid g = new Grid();
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.RowDefinitions.Add(new RowDefinition());
            g.Margin = new Thickness(0);
            g.Height = 480;
            g.Width = 800;
            MainCanvas.Children.Add(g);

            string text = _message.Text;

            TextBlock tb = getTextBlock();
            tb.Margin = new Thickness(10);
            tb.Text = text;

            int fontScale = (_message.FontScale == 0 ? 1 : _message.FontScale);
            int fontSize = 50 + (fontScale * FONT_SCALE_MULTIPLYER);
            tb.FontSize = fontSize;

            tb.Foreground = ColorHelper.GetBrushFromString(_message.ForegroundColor);
            tb.TextWrapping = TextWrapping.Wrap;
            tb.TextAlignment = TextAlignment.Center;

            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            while (tb.ActualWidth >= 780)
            {
                tb.FontSize -= 1;
                System.Diagnostics.Debug.WriteLine("FontSize: " + tb.FontSize.ToString());
            }

            while (tb.ActualHeight >= 460)
            {
                tb.FontSize -= 1;
                System.Diagnostics.Debug.WriteLine("FontSize: " + tb.FontSize.ToString());
            }

            g.Children.Add(tb);

            Grid.SetColumn(tb, 0);
            Grid.SetRow(tb, 0);

        }

        private TextBlock getTextBlock()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.NoWrap;
            textBlock.FontSize = getFontSize();
            return textBlock;
        }


    }
}