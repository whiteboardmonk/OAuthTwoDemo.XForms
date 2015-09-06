using Xamarin.Forms;

namespace OAuthTwoDemo.XForms
{
	public class ProfilePage : BaseContentPage
	{
		public ProfilePage ()
		{
			// Using messaging center to ensure that content only gets loaded once authentication is successful.
			// Once Xamarin.Forms adds more support for view life cycle events, this kind of thing won't be as necessary.
			// The OnAppearing() and OnDisappearing() overrides just don't quite cut the mustard yet, nor do the Appearing and Disappearing delegates.
			var myButton = new Button();
            myButton.Text = "Personalized Reviews";
            myButton.Clicked += myButton_Clicked;
            MessagingCenter.Subscribe<App> (this, "Authenticated", (sender) => {
                Content = new StackLayout()
                {
                    Children = {
                        myButton
                    }
                };
            });
            
            //Content = new StackLayout(){
            //    Children = {
            //        myButton
            //    }
            //};
            
		}

        void myButton_Clicked(object sender, System.EventArgs e)
        {
            ListView lv = new ListView();
            lv.ItemsSource = App.Instance.BulkMatchList;
            //lv.ItemsSource = new[] { "a", "b", "c" };
            lv.ItemTemplate = new DataTemplate(typeof(VetCell));

            var reviewPage = new ContentPage
            {
                Content = lv
            };
            reviewPage.Title = "Personalized Reviews";
            Navigation.PushModalAsync(reviewPage);
        }

        public class CustomCell : ViewCell
        {
            public CustomCell()
            {
                //instantiate each of our views
                var image = new Image();
                StackLayout cellWrapper = new StackLayout();
                StackLayout horizontalLayout = new StackLayout();
                Label left = new Label();
                Label right = new Label();

                //set bindings
                left.SetBinding(Label.TextProperty, "name");
                right.SetBinding(Label.TextProperty, "description");
                image.SetBinding(Image.SourceProperty, "profileIcon");

                //Set properties for desired design
                cellWrapper.BackgroundColor = Color.FromHex("#eee");
                horizontalLayout.Orientation = StackOrientation.Horizontal;
                right.HorizontalOptions = LayoutOptions.EndAndExpand;
                left.TextColor = Color.FromHex("#f35e20");
                right.TextColor = Color.FromHex("503026");

                //add views to the view hierarchy
                horizontalLayout.Children.Add(image);
                horizontalLayout.Children.Add(left);
                horizontalLayout.Children.Add(right);
                cellWrapper.Children.Add(horizontalLayout);
                View = cellWrapper;
            }

        }
        public class VetCell : ViewCell
        {
            public VetCell()
            {
                var vetProfileImage = new CircleImage
                {
                    BorderColor = App.BrandColor,
                    BorderThickness = 2,
                    HeightRequest = 50,
                    WidthRequest = 50,
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
                vetProfileImage.SetBinding(Image.SourceProperty, "ImageSource");

                var nameLabel = new Label()
                {
                    FontFamily = "HelveticaNeue-Medium",
                    FontSize = 18,
                    TextColor = Color.Black
                };
                nameLabel.SetBinding(Label.TextProperty, "Name");

                var distanceLabel = new Label()
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 12,
                    TextColor = Color.FromHex("#666")
                };
                distanceLabel.SetBinding(
                                Label.TextProperty, new Binding(
                                        "Distance",
                                        stringFormat: "{0} Miles Away")
                        );

                // Online image and label
                var onlineImage = new Image()
                {
                    Source = "online.png",
                    HeightRequest = 8,
                    WidthRequest = 8
                };
                onlineImage.SetBinding(Image.IsVisibleProperty, "ShouldShowAsOnline");
                var onLineLabel = new Label()
                {
                    Text = "Online",
                    TextColor = App.BrandColor,
                    FontSize = 12,
                };
                onLineLabel.SetBinding(Label.IsVisibleProperty, "ShouldShowAsOnline");

                // Offline image and label
                var offlineImage = new Image()
                {
                    Source = "offline.png",
                    HeightRequest = 8,
                    WidthRequest = 8
                };
                offlineImage.SetBinding(Image.IsVisibleProperty, "ShouldShowAsOffline");
                var offLineLabel = new Label()
                {
                    Text = "5 hours ago",
                    TextColor = Color.FromHex("#ddd"),
                    FontSize = 12,
                };
                offLineLabel.SetBinding(Label.IsVisibleProperty, "ShouldShowAsOffline");

                // Vet rating label and star image
                var starLabel = new Label()
                {
                    FontSize = 12,
                    TextColor = Color.Gray
                };
                starLabel.SetBinding(Label.TextProperty, "Stars");

                var starImage = new Image()
                {
                    Source = "star.png",
                    HeightRequest = 12,
                    WidthRequest = 12
                };

                var ratingStack = new StackLayout()
                {
                    Spacing = 3,
                    Orientation = StackOrientation.Horizontal,
                    Children = { starImage, starLabel }
                };

                var statusLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { 
                                distanceLabel, 
                                onlineImage, 
                                onLineLabel, 
                                offlineImage, 
                                offLineLabel 
                        }
                };

                var vetDetailsLayout = new StackLayout
                {
                    Padding = new Thickness(10, 0, 0, 0),
                    Spacing = 0,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children = { nameLabel, statusLayout, ratingStack }
                };

                var tapImage = new Image()
                {
                    Source = "tap.png",
                    HorizontalOptions = LayoutOptions.End,
                    HeightRequest = 12,
                };

                var cellLayout = new StackLayout
                {
                    Spacing = 0,
                    Padding = new Thickness(10, 5, 10, 5),
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children = { vetProfileImage, vetDetailsLayout, tapImage }
                };

                this.View = cellLayout;
            }
        }
	}
}

