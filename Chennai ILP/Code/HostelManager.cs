using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Chennai_ILP.Code
{
    class HostelManager
    {
        const int FETCH_SIZE = 10;
        static int cursorPosition = 0;

        static ObservableCollection<HostelItem> _hostelCache;
        static ObservableCollection<HostelItem> _searchCache;
        public const string BASE_URL = "http://localhost/chennai/";
        //public const string BASE_URL = "http://theinspirer.in/chennai/";

        public static ObservableCollection<HostelItem> HostelItems { get { return _hostelCache; } }
        public static ObservableCollection<HostelItem> SearchItems { get { return _searchCache; } }

        public static void ResetGetListHostel()
        {
            cursorPosition = 0;
        }

        public async static Task<bool> RegisterFeedback(HostelItem targetHostel, FeedbackItem feedback)
        {
            string url = string.Format("uploadcomment.php?ratingtable={0}&name={1}&empid={2}&comment={3}&rating={4}",
                targetHostel.TableName, UserInformation.FullName, UserInformation.EmployeeId, feedback.Comments, feedback.Rating);

            string response = await GetURLContentAsString(url);

            response = response.Trim();
            if (response == "sucess")
            {
                return true;
            }

            return false;
        }
        public async static Task<bool> RegisterUser(string empId, string name, string location, string lg, string email)
        {
            string url = string.Format("register.php?employee_id={1}&name={2}&location={3}&lg={4}&email={5}&imei=winphonenull",
                "", empId, name, location, lg, email);
            string response = await GetURLContentAsString(url);

            response = response.Trim();
            if (response.Length == 0)
            {
                return true;
            }

            return false;
        }
        public async static Task<ObservableCollection<HostelItem>> GetNextHostelItems()
        {
            ObservableCollection<HostelItem> _newList;
            if (cursorPosition == 0)
            {
                _hostelCache = new ObservableCollection<HostelItem>();
                _newList = await GetHostelList();
            } else
            {
                _newList = await GetHostelList(cursorPosition, FETCH_SIZE);
            }

            cursorPosition += _newList.Count;

            ObservableCollection<HostelItem> _newMaster = AddToList(_newList);
            return _newMaster;
        }

        private static ObservableCollection<HostelItem> AddToList(ObservableCollection<HostelItem> newItems)
        {
            foreach (HostelItem item in newItems)
            {
                if (!_hostelCache.Contains(item))
                {
                    _hostelCache.Add(item);
                }
            }
                return _hostelCache;
        }
        public async static Task<ObservableCollection<HostelItem> > GetHostelList()
        {
            List<HostelItem> _temp = new List<HostelItem>();

            var response = await GetURLContentAsString("list.php");
            
            return ParseHostelList(response);
        }
        public async static Task<ObservableCollection<HostelItem>> GetHostelList(int from, int count)
        {
            List<HostelItem> _temp = new List<HostelItem>();

            string url = string.Format("list.php?from={0}&to={1}", from, count);
            var response = await GetURLContentAsString(url);

            return ParseHostelList(response);
        }
        public async static Task<ObservableCollection<HostelItem>> GetHostelList(string pattern)
        {
            _searchCache = new ObservableCollection<HostelItem>();

            string url = string.Format("list.php?pattern={0}", pattern);
            var response = await GetURLContentAsString(url);

            _searchCache = ParseHostelList(response);
            return _searchCache;
        }

        private static ObservableCollection<HostelItem> ParseHostelList(string jsonString)
        {
            ObservableCollection<HostelItem> _hostelList = new ObservableCollection<HostelItem>();
            if (string.IsNullOrEmpty(jsonString)) return _hostelList;

            JArray root = JArray.Parse(jsonString);
            foreach (JObject obj in root)
            {
                string _title = (string)obj["title"];
                //string _image = (string)obj["image"];
                int _rating = int.Parse((string)obj["rating"]);
                int _releaseYear = int.Parse((string)obj["releaseYear"]);

                JArray genre = (JArray)obj["genre"];
                string _tableName = (string)genre[0];
                double _latitudes = double.Parse((string)genre[1]);
                double _longitudes = double.Parse((string)genre[2]);
                string _address = (string)genre[3];
                string _image = (string)genre[4];

                _hostelList.Add(new HostelItem()
                    {
                        Title = _title,
                        Rating = _rating,
                        ReleaseYear = _releaseYear,
                        TableName = _tableName,
                        Latitudes = _latitudes,
                        Longitudes = _longitudes,
                        Address = _address,
                        Image = _image
                    });
            }

            return _hostelList;
        }

        private async static Task<string> GetURLContentAsString(string relativeURL)
        {
            HttpClient client = new HttpClient();
            string url = BASE_URL + relativeURL;
            var response = await client.GetAsync(url);
            //response.EnsureSuccessStatusCode();

#if DEBUG
            System.Diagnostics.Debug.WriteLine("*******\nURL\t\t\t\t: " + url + "\nPayload length\t: " + response.Content.Headers.ContentLength + " byte(s)\n*******");
#endif

            return response.Content.ReadAsStringAsync().Result;
        }

    }
}
