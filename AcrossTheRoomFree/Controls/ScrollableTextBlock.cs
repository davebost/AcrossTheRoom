using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Text;

namespace AcrossTheRoom
{
    public class ScrollableTextBlock : Control
    {
        private StackPanel stackPanel;
        private TextBlock measureTextBlock;

        public ScrollableTextBlock()
        {
            // Get the style from generic.xaml
            this.DefaultStyleKey = typeof(ScrollableTextBlock);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(ScrollableTextBlock),
                new PropertyMetadata("ScrollableTextBlock", OnTextPropertyChanged));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public new double ActualHeight
        {
            get
            {
                double maxHeight = 0;
                foreach (TextBlock tb in this.stackPanel.Children)
                {
                    if (maxHeight < tb.ActualHeight) maxHeight = tb.ActualHeight;
                }
                return maxHeight;
            }
            private set { }
        }

        public new double ActualWidth
        {
            get
            {
                double actualWidth = 0;
                if (this.stackPanel.Orientation == Orientation.Horizontal)
                {
                    foreach (TextBlock tb in this.stackPanel.Children)
                    {
                        actualWidth += tb.ActualWidth;
                    }
                }
                else
                {
                    double maxWidth = 0;
                    foreach (TextBlock tb in this.stackPanel.Children)
                    {
                        if (maxWidth < tb.ActualWidth) maxWidth = tb.ActualWidth;
                    }
                    actualWidth = maxWidth;
                }
                return actualWidth;
            }
            private set { }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollableTextBlock source = (ScrollableTextBlock)d;
            string value = (string)e.NewValue;
            source.ParseText(value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.stackPanel = this.GetTemplateChild("StackPanel") as StackPanel;
            this.ParseText(this.Text);
        }

        private void ParseText(string value)
        {

            if (this.stackPanel == null)
            {
                return;
            }

            //// Clear previous TextBlocks
            this.stackPanel.Children.Clear();

            // Calculate max char count
            TextBlock tb = this.GetTextBlock();
            tb.Text = value;
            double actualWidth = tb.ActualWidth;

            if (actualWidth < 2048)
            {
                TextBlock textBlock = this.GetTextBlock();
                textBlock.Text = value;
                this.stackPanel.Children.Add(textBlock);
            }
            else
            {

                // Cut up message into multiple textblocks

                // TODO: A HACK ROUTINE THAT NEEDS TO BE CLEANED UP

                int nWidth = 0;

                StringBuilder sb = new StringBuilder();
                string[] words = value.Split(' ');
                sb.Append(words[0]);

                for (int i = 1; i < words.Length; i++)
                {
                    if (sb.ToString().TrimEnd().Length > 0) sb.Append(" ");
                    sb.Append(words[i]);
                    nWidth = (int)MeasureString(sb.ToString()).Width;

                    if (nWidth > 2048)
                    {
                        sb.Remove(sb.Length - words[i].Length, words[i].Length);

                        TextBlock newTextBlock = this.GetTextBlock();
                        newTextBlock.Text = sb.ToString().TrimEnd();
                        this.stackPanel.Children.Add(newTextBlock);

                        sb = new StringBuilder();
                        sb.Append(" " + words[i]);
                    }
                }

                // grab the last bits
                if (sb.Length > 0)
                {
                    TextBlock newTextBlock = this.GetTextBlock();
                    newTextBlock.Text = sb.ToString();
                    this.stackPanel.Children.Add(newTextBlock);
                }

            }

        }

        private Size MeasureString(string text)
        {
            if (measureTextBlock == null)
            {
                measureTextBlock = this.GetTextBlock();
            }

            measureTextBlock.Text = text;

            return new Size(measureTextBlock.ActualWidth, measureTextBlock.ActualHeight);
        }

        private Size MeasureText(string value)
        {
            TextBlock textBlock = this.GetTextBlockOnly();
            textBlock.Text = value;
            return new Size(textBlock.ActualWidth, textBlock.ActualHeight);
        }

        private TextBlock GetTextBlockOnly()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.NoWrap;
            textBlock.FontSize = this.FontSize;
            textBlock.FontFamily = this.FontFamily;
            textBlock.FontWeight = this.FontWeight;
            return textBlock;
        }

        private TextBlock GetTextBlock()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.NoWrap;
            textBlock.FontSize = this.FontSize;
            textBlock.FontFamily = this.FontFamily;
            // textBlock.FontStyle = this.FontStyle;
            textBlock.FontWeight = this.FontWeight;
            textBlock.Foreground = this.Foreground;
            //textBlock.Margin = new Thickness(0, 0, MeasureText(" ").Width, 0);
            textBlock.Margin = new Thickness(0);
            return textBlock;
        }

    }
}
