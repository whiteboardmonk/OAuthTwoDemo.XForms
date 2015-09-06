using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OAuthTwoDemo.XForms
{
	public class App
	{
		// just a singleton pattern so I can have the concept of an app instance
		static volatile App _Instance;
		static object _SyncRoot = new Object();
		public static App Instance
		{
			get 
			{
				if (_Instance == null) 
				{
					lock (_SyncRoot) 
					{
						if (_Instance == null) {
							_Instance = new App ();
                            _Instance.BulkMatchList = SetupUsers();
							_Instance.OAuthSettings = 
								new OAuthSettings (
									clientId: "1639469262962972",  		// your OAuth2 client id 
									scope: "public_profile,email,user_friends,user_about_me,user_status",  		// The scopes for the particular API you're accessing. The format for this will vary by API.
									authorizeUrl: "https://m.facebook.com/dialog/oauth/",  	// the auth URL for the service
									redirectUrl: "http://www.facebook.com/connect/login_success.html");   // the redirect URL for the service

							        // If you'd like to know more about how to integrate with an OAuth provider, 
									// I personally like the Instagram API docs: http://instagram.com/developer/authentication/
						}
					}
				}

				return _Instance;
			}
		}
        public static List<AppUser> SetupUsers()
        {
            var x = new List<AppUser>();
            x.Add(new AppUser() { name = "Steve Jobs", description = "yo", profileIcon = "Assets/john.jpg" });
            x.Add(new AppUser() { name = "Marilyn Monroe", description = "yo", profileIcon = "" });
            x.Add(new AppUser() { name = "Hitler", description = "yo", profileIcon = "" });
            x.Add(new AppUser() { name = "Narendra Modi", description = "yo", profileIcon = "" });
            x.Add(new AppUser() { name = "Barack Obama", description = "yo", profileIcon = "" });
            x.Add(new AppUser() { name = "Michelle Obama", description = "yo", profileIcon = "" });
            return x;
        }

		public OAuthSettings OAuthSettings { get; private set; }
        public User User;
        public List<AppUser> BulkMatchList;        
		public NavigationPage _NavPage;

		public Page GetMainPage ()
		{
			var profilePage = new ProfilePage();

			_NavPage = new NavigationPage(profilePage);

			return _NavPage;
		}

		public bool IsAuthenticated {
			get { return !string.IsNullOrWhiteSpace(_Token); }
		}

		string _Token;
		public string Token {
			get { return _Token; }
		}

		public async void SaveToken(string token)
		{
			_Token = token;
            GetUserInfo();
			// broadcast a message that authentication was successful
			MessagingCenter.Send<App> (this, "Authenticated");
		}

		public Action SuccessfulLoginAction
		{
			get {

                return new Action(() => {
                    _NavPage.Navigation.PopModalAsync();
                });
			}
		}
        public Action RedirectToReviews(ContentPage page)
        {
            return new Action(() =>
            {
                _NavPage.Navigation.PushAsync(page);
            });
        }
        public async void GetUserInfo()
        {
            var client = new HttpClient();
            var access_token = _Instance.Token;
            var apiUrl = "https://graph.facebook.com/v2.4/me?fields=id,name,posts{message},about,bio&access_token=" + access_token;
            var getUserDetailsTask = await client.GetAsync(apiUrl);
            if (getUserDetailsTask.IsSuccessStatusCode)
            {
                var responseJsonString = await getUserDetailsTask.Content.ReadAsStringAsync();
                var jsonData = JsonConvert.DeserializeObject<User>(responseJsonString);
                _Instance.User = jsonData;

                //Make api call to xLabsApi - Watson interface to fetch user's personality insights
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("text", @"Call me Ishmael. Some years ago-never mind how long precisely-having little or no money in my purse,
and nothing particular to interest me on shore, I thought I would sail about a little and see the watery
part of the world. It is a way I have of driving off the spleen and regulating the circulation. Whenever
I find myself growing grim about the mouth; whenever it is a damp, drizzly November in my soul; whenever
I find myself involuntarily pausing before coffin warehouses, and bringing up the rear of every funeral
I meet; and especially whenever my hypos get such an upper hand of me, that it requires a strong moral Call me Ishmael. Some years ago-never mind how long precisely-having little or no money in my purse,
and nothing particular to interest me on shore, I thought I would sail about a little and see the watery
part of the world. It is a way I have of driving off the spleen and regulating the circulation. Whenever
I find myself growing grim about the mouth; whenever it is a damp, drizzly November in my soul; whenever
I find myself involuntarily pausing before coffin warehouses, and bringing up the rear of every funeral
I meet; and especially whenever my hypos get such an upper hand of me, that it requires a strong moral"));
                var content = new FormUrlEncodedContent(values);
                var client2 = new HttpClient();
                var response = await client2.PostAsync("http://xlab.mybluemix.net/map", content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonOut = await response.Content.ReadAsStringAsync();
                    var y = JsonConvert.DeserializeObject(jsonOut);
                    _Instance.User.personality = y.ToString();

                    var client3 = new HttpClient();
                    var matchResponse = await client3.PostAsync("http://xlab.mybluemix.net/bulk_match", content).ConfigureAwait(false);
                    if (matchResponse.IsSuccessStatusCode)
                    {
                        var matchDataString = await matchResponse.Content.ReadAsStringAsync();
                        JArray matchDataJson = (JArray)JsonConvert.DeserializeObject(matchDataString);
                        for (int i = 0; i < matchDataJson.Count; i++)
			            {
                            BulkMatchList[i].matchData = JObject.FromObject(matchDataJson[i]);
			            }
                        //ListView lv = new ListView();
                        ////lv.ItemsSource = BulkMatchList;
                        //lv.ItemsSource = new[] { "a", "b", "c" };

                        //var reviewPage = new ContentPage
                        //{
                        //    Content = lv
                        //};
                        //reviewPage.Title = "Personalized Reviews";
                        //MessagingCenter.Send<App>(this, "Ready");
                        //await _Instance._NavPage.Navigation.PushModalAsync(reviewPage);
                        //RedirectToReviews(reviewPage);
                    }
                }
            }
        }
	}
}

