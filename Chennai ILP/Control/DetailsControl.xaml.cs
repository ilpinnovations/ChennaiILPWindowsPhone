using Chennai_ILP.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Chennai_ILP
{
    public delegate void ControlVisibilityHandler(HostelItem hostelItem, FeedbackItem feedback);
    public sealed partial class DetailsControl : UserControl
    {
        private HostelItem currentHostelItem = null;
        public bool IsVisible { get; set; }
        public event ControlVisibilityHandler SubmitClicked;
        public event ControlVisibilityHandler CancelClicked;

        private void OnSubmit(HostelItem hostelItem, FeedbackItem feedback)
        {
            if (SubmitClicked != null)
                SubmitClicked(hostelItem, feedback);
        }
        private void OnCancel(HostelItem hostelItem, FeedbackItem feedback)
        {
            if (CancelClicked != null)
                CancelClicked(hostelItem, feedback);
        }
        public DetailsControl()
        {
            this.InitializeComponent();

            IsVisible = true;
            Details_Exit.Completed += delegate
            {
                this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            };
        }

        private void ShowControl()
        {
            if (!IsVisible)
            {
                IsVisible = true;
                this.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ShowMap(false);
                Details_Entry.Begin();
            }
        }
        public void HideControl()
        {
            if (IsVisible)
            {
                IsVisible = false;
                Details_Exit.Begin();
            }
        }

        public async void ShowMap(bool show)
        {
            if (show)
            {
                mapControl.Visibility = Windows.UI.Xaml.Visibility.Visible;
                BasicGeoposition basicGeo = new BasicGeoposition();
                basicGeo.Latitude = currentHostelItem.Latitudes;
                basicGeo.Longitude = currentHostelItem.Longitudes;
                basicGeo.Altitude = 10;

                await mapControl.TrySetViewAsync(new Geopoint(basicGeo));
            }
            else
            {
                mapControl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }
        public void ShowControlForHostel(HostelItem hostelItem)
        {
            tbHostelName.Text = hostelItem.Title;
            tbHostelAddress.Text = hostelItem.Address;
            starRateControl.Value = 0;
            tbHostelComments.Text = string.Empty;
            tbAvgRating.Text = hostelItem.Rating.ToString();

            currentHostelItem = hostelItem;

            ShowControl();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Cancel clicked
            HideControl();
            OnCancel(currentHostelItem, new FeedbackItem(0, string.Empty));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Submit clicked
            if (ValidFields())
            {
                HideControl();
                OnSubmit(currentHostelItem, new FeedbackItem(starRateControl.Value, tbHostelComments.Text));
            }
        }

        private bool ValidFields()
        {
            if (starRateControl.Value <= 0)
            {
                Helper.ShowMessage("Rating cannot be zero!", "Error");
                return false;
            }

            return true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // map button clicked
            bool? toggleState = (sender as ToggleButton).IsChecked;

            if (toggleState.HasValue)
            {
                ShowMap(toggleState.Value);
            }
        }
    }
}
