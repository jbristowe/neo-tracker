﻿using System;
using Windows.UI.Xaml.Controls;

namespace SpaceViewTest
{
    public class SpaceViewItemClickedEventArgs : EventArgs
    {
        internal SpaceViewItemClickedEventArgs(ContentControl container, object item)
        {
            Container = container;
            Item = item;
        }

        public ContentControl Container { get; set; }
        public object Item { get; set; }
    }
}