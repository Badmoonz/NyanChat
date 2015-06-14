using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace TestMetroUI
{
    public static class NavigationService
    {
        // Copied from http://geekswithblogs.net/casualjim/archive/2005/12/01/61722.aspx
        private static readonly Regex RE_URL = new Regex(@"(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?");
        private static readonly Regex PekaRegex = new Regex(@"(:[a-z]+:)");
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(NavigationService),
            new PropertyMetadata(null, OnTextChanged)
        );

        public static string GetText(DependencyObject d)
        { return d.GetValue(TextProperty) as string; }

        public static void SetText(DependencyObject d, string value)
        { d.SetValue(TextProperty, value); }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var text_block = d as TextBlock;
            if (text_block == null)
                return;

            text_block.Inlines.Clear();

            var new_text = (string)e.NewValue;
            if (string.IsNullOrEmpty(new_text))
                return;

            // Find all URLs using a regular expression
            int lastPos = 0;
            foreach (Match match in RE_URL.Matches(new_text))
            {
                // Copy raw string from the last position up to the match
                if (match.Index != lastPos)
                {
                    var rawText = new_text.Substring(lastPos, match.Index - lastPos);
                    text_block.Inlines.AddRange(CheckNewlines(rawText));
                }

                // Create a hyperlink for the match
                var link = new Hyperlink(new Run(match.Value))
                {
                    NavigateUri = new Uri(match.Value)
                };
                link.Click += OnUrlClick;

                text_block.Inlines.Add(link);

                // Update the last matched position
                lastPos = match.Index + match.Length;
            }

            // Finally, copy the remainder of the string
            if (lastPos < new_text.Length)
                text_block.Inlines.AddRange(CheckNewlines(new_text.Substring(lastPos)));

        }

        private static IEnumerable<Inline> CheckNewlines(string text)
        {
            var splitted = text.Split(new [] {Environment.NewLine}, StringSplitOptions.None);
            foreach (var txt in CheckPekas(splitted[0]))
            {
                yield return txt;
            }
            foreach(var raw_text in splitted.Skip(1))
            {
                yield return new LineBreak();
                foreach (var txt in CheckPekas(raw_text))
                {
                    yield return txt; 
                }
            }
        }


        private static IEnumerable<Inline> CheckPekas(string text)
        {
            int lastPos = 0;
            foreach (Match match in PekaRegex.Matches(text))
            {
                if (match.Index != lastPos)
                {
                    yield return new Run(text.Substring(lastPos, match.Index - lastPos));
                }
                if (PekaGrabber.PekaDict.ContainsKey(match.Value))
                {
                    yield return AddPeka(match.Value);
                }
                else
                {
                    yield return new Run(match.Value);
                }
                lastPos = match.Index + match.Length;
            }

            if (lastPos < text.Length)
                yield return new Run(text.Substring(lastPos));
        }



        private static Random rnd = new Random();
        private static string[] keys = PekaGrabber.PekaDict.Keys.ToArray();
        private static Inline AddPeka(string key)
        {
            var imgMessage = new Image();
            imgMessage.ToolTip = key;
            var bi = PekaGrabber.PekaDict[key].Value;
            imgMessage.Source = bi;
            imgMessage.Width = bi.PixelWidth;
            imgMessage.Height = bi.PixelHeight;
            return new InlineUIContainer(imgMessage);
        }


        private static void OnUrlClick(object sender, RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;
            // Do something with link.NavigateUri like:
            Process.Start(link.NavigateUri.ToString());
        }
    }
}