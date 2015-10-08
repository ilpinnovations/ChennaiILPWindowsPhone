﻿using Chennai_ILP.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Chennai_ILP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        string empId, fullName, emailId, lgName;
        string location;

        public RegisterPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadUserInformation();
        }

        private async void tbSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Submit button clicked
            if (await ValidateData())
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("Passed the validation on the data");
#endif

                var response = await HostelManager.RegisterUser(empId, fullName, location, lgName, emailId);
                if (response)
                {
                    Helper.ShowMessage("Successfully Registered", "Information");

                    // Update with new user information
                    UpdateUserInformation();
                    
                    //Navigate
                    Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    Helper.ShowMessage("Cannot register. Please try after sometime", "Error");
                }
            }
        }

        /// <summary>
        /// Returns true on success, exception on failure
        /// </summary>
        /// <returns>true on success</returns>
        async Task<bool> ValidateData()
        {
            List<string> invalids = new List<string>();
            bool isValid = true;
            empId = tbEmployeeId.Text;
            fullName = tbEmployeeName.Text;
            lgName = tbLG.Text;
            emailId = tbEmailId.Text;
            int _lIndex = cbLocation.SelectedIndex;

            if (string.IsNullOrEmpty(empId))
            {
                isValid = false;
                invalids.Add("Employee ID");
            }
            if (string.IsNullOrEmpty(fullName))
            {
                isValid = false;
                invalids.Add("Employee Name");
            }
            if (_lIndex == -1)
            {
                isValid = false;
                invalids.Add("Location");
            }
            if (string.IsNullOrEmpty(lgName))
            {
                isValid = false;
                invalids.Add("LG Name");
            }

            if (string.IsNullOrEmpty(emailId) || 
                !emailId.EndsWith("@tcs.com"))
            {
                isValid = false;
                invalids.Add("Email Id");
            }

            if (!isValid)
            {
                string msg = "The following fields are invalid. Please correct them to continue:\n";
                msg += string.Join(", ", invalids);
                string title = "Error";

                Windows.UI.Popups.MessageDialog md = new Windows.UI.Popups.MessageDialog(msg, title);
                await md.ShowAsync();
                return false;
            }

            location = (cbLocation.SelectedItem as ComboBoxItem).Content.ToString();

            return true;
        }
        void LoadUserInformation()
        {
            tbEmployeeId.Text = string.Empty;
            tbEmployeeName.Text = string.Empty;
            tbLG.Text = string.Empty;
            tbEmailId.Text = string.Empty;
        }
        async void UpdateUserInformation()
        {
            UserInformation.UpdateUserInformation(empId, fullName, lgName, emailId, location);
            //await MyILPClient.GetEContacts();
        }
    }
}
