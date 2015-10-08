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
using Chennai_ILP.Code;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Chennai_ILP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly DataTemplate footerTemplate;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.footerTemplate = listHostel.FooterTemplate;

            detailsControl.SubmitClicked += detailsControl_SubmitClicked;
            detailsControl.CancelClicked += detailsControl_CancelClicked;
        }

        void detailsControl_CancelClicked(HostelItem hostelItem, FeedbackItem feedback)
        {
            screenRect.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Cancel clicked");
#endif
        }

        async void detailsControl_SubmitClicked(HostelItem hostelItem, FeedbackItem feedback)
        {
            screenRect.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            bool success = await HostelManager.RegisterFeedback(hostelItem, feedback);

            if (success)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("Submitted successfully");
#endif
                Helper.ShowMessage("Thank you for your valuable feedback", "Information");

            }
            else
            {
                Helper.ShowMessage("Feedback cannot be uploaded, please try again later.", "Error");
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HideDetailsControl();
            InitListView();
        }

        private async void InitListView()
        {
            listHostel.DataContext = await HostelManager.GetNextHostelItems();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Search button clicked
            string searchString = tbSearch.Text.Trim();

            if (String.IsNullOrEmpty(searchString)) return;

            LoadItemsInListView(searchString);
        }

        private void listHostel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // a hostel is selected in the list view
            int index = (sender as ListView).SelectedIndex;

            if (index == -1) return;

            HostelItem selectedHostel = (HostelItem)(sender as ListView).SelectedItem;
            (sender as ListView).SelectedIndex = -1;
            ShowDetailsControl(selectedHostel);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Load more button click in the list view
            LoadItemsInListView();
        }

        private async void LoadItemsInListView(string searchString = "")
        {
            bool fromSearch = !string.IsNullOrEmpty(searchString);

            ShowPB(true);

            if (fromSearch)
            {
                var newList = await HostelManager.GetHostelList(searchString);
                //listHostel.DataContext = null;
                listHostel.DataContext = newList;
                listHostel.FooterTemplate = null;

                btnReset.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                var newList = await HostelManager.GetNextHostelItems();
                //listHostel.DataContext = null;
                listHostel.DataContext = newList;
                listHostel.FooterTemplate = footerTemplate;
            }
            ShowPB(false);
        }
        private void ShowPB(bool show)
        {
            if (show) pbProgress.Visibility = Windows.UI.Xaml.Visibility.Visible;
            else pbProgress.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // Reset search button clicked
            HostelManager.ResetGetListHostel();
            btnReset.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            tbSearch.Text = string.Empty;
            LoadItemsInListView();
        }

        public void ShowDetailsControl(HostelItem selectedItem)
        {
            screenRect.Visibility = Windows.UI.Xaml.Visibility.Visible;
            detailsControl.ShowControlForHostel(selectedItem);
        }
        public void HideDetailsControl()
        {
            screenRect.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            detailsControl.HideControl();
        }
    }
}
