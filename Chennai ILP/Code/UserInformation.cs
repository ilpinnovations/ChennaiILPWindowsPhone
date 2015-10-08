using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Chennai_ILP
{
    class UserInformation
    {
        static string _employeeId, _fullName, _lgName, _emailId;
        static string _locationName;
        
        public static string EmployeeId { get { return _employeeId; } set { _employeeId = value; } }
        public static string FullName { get { return _fullName; } set { _fullName = value; } }
        public static string LGName { get { return _lgName; } set { _lgName = value.ToUpper(); } }
        public static string EmailId { get { return _emailId; } set { _emailId = value; } }
        public static string LocationName { get { return _locationName; } set { _locationName = value; } }
        public static void UpdateUserInformation(string employeeId,
            string fullName, string lgName, string emailId, string locationName)
        {
            EmployeeId = employeeId;
            FullName = fullName;
            LGName = lgName;
            EmailId = emailId;
            LocationName = locationName;

            //Save to disk
            SaveUserInformation();
        }
        public static bool GetUserInformation()
        {
            bool IsFirstLaunch = false;

            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("NOT_FIRST_LAUNCH"))
            {
                EmployeeId      = (string)localSettings.Values["KEY_EMP_ID"];
                FullName        = (string)localSettings.Values["KEY_FULL_NAME"];
                LGName          = (string)localSettings.Values["KEY_LG_NAME"];
                EmailId         = (string)localSettings.Values["KEY_EMAIL_ID"];
                LocationName    = (string)localSettings.Values["KEY_LOCATION_NAME"];
            }
            else
            {
                EmployeeId = FullName = LGName = EmailId = string.Empty;
                LocationName = "Chennai";
                IsFirstLaunch = true;
            }

            return IsFirstLaunch;
        }
        public static void ClearUserInformation()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Clear();
        }
        public static void SaveUserInformation()
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            localSettings.Values["KEY_EMP_ID"] = EmployeeId;
            localSettings.Values["KEY_FULL_NAME"] = FullName;
            localSettings.Values["KEY_LG_NAME"] = LGName;
            localSettings.Values["KEY_EMAIL_ID"] = EmailId;
            localSettings.Values["KEY_LOCATION_NAME"] = LocationName;

            #region MiscSaves
            //WARNING! App depends on the following key, do not delete!
            localSettings.Values["NOT_FIRST_LAUNCH"] = "Developed by: Milind Gour";
            #endregion        
        }
    }
}
