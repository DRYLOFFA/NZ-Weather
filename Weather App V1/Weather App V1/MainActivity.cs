using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using System;
using Plugin.Geolocator;
using Android.Locations;

namespace Weather_App_V1
{
    [Activity(Label = "NZ Weather", Theme = "@android:style/Theme.DeviceDefault.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        EditText txtSearch;
        Button btnSearch;
        Button btnGPS;
        TextView txtCity;
        TextView txtMain;
        TextView txtTemp;
        ImageView imgMain;
        TextView txtDescription;
        ImageView imgDay1;
        ImageView imgDay2;
        ImageView imgDay3;
        ImageView imgDay4;
        ImageView imgDay5;
        TextView txtDay1;
        TextView txtDay2;
        TextView txtDay3;
        TextView txtDay4;
        TextView txtDay5;
        TextView txtDetails1;
        TextView txtDetails2;
        TextView txtDetails3;
        TextView txtDetails4;
        TextView txtDetails5;


        FiveDay.RootObject root;
        CurrentDay.RootObject rootDay;

        ImageView[] img = new ImageView[6];

        public double latitude;
        public double longitude;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            txtSearch = FindViewById<EditText>(Resource.Id.txtSearch);
            btnSearch = FindViewById<Button>(Resource.Id.btnSearch);
            btnGPS = FindViewById<Button>(Resource.Id.btnGPS);
            txtCity = FindViewById<TextView>(Resource.Id.txtCity);
            txtMain = FindViewById<TextView>(Resource.Id.txtMain);
            txtTemp = FindViewById<TextView>(Resource.Id.txtTemp);
            imgMain = FindViewById<ImageView>(Resource.Id.imgMain);
            txtDescription = FindViewById<TextView>(Resource.Id.txtDescription);
            imgDay1 = FindViewById<ImageView>(Resource.Id.imgDay1);
            imgDay2 = FindViewById<ImageView>(Resource.Id.imgDay2);
            imgDay3 = FindViewById<ImageView>(Resource.Id.imgDay3);
            imgDay4 = FindViewById<ImageView>(Resource.Id.imgDay4);
            imgDay5 = FindViewById<ImageView>(Resource.Id.imgDay5);
            txtDay1 = FindViewById<TextView>(Resource.Id.txtDay1);
            txtDay2 = FindViewById<TextView>(Resource.Id.txtDay2);
            txtDay3 = FindViewById<TextView>(Resource.Id.txtDay3);
            txtDay4 = FindViewById<TextView>(Resource.Id.txtDay4);
            txtDay5 = FindViewById<TextView>(Resource.Id.txtDay5);
            txtDetails1 = FindViewById<TextView>(Resource.Id.txtDetails1);
            txtDetails2 = FindViewById<TextView>(Resource.Id.txtDetails2);
            txtDetails3 = FindViewById<TextView>(Resource.Id.txtDetails3);
            txtDetails4 = FindViewById<TextView>(Resource.Id.txtDetails4);
            txtDetails5 = FindViewById<TextView>(Resource.Id.txtDetails5);


            btnGPS.Click += BtnGPS_Click;
            btnSearch.Click += BtnSearch_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Call Hamilton \\
            RestHandler objrest = new RestHandler();
            root = objrest.ExecuteRequest("Hamilton");
            rootDay = objrest.ExecuteRequestCurrent("Hamilton");

            LoadHamilton();
            Image();

            // 5 Day forcast \\
            int[] imresid = { Resource.Id.imgDay1, Resource.Id.imgDay2, Resource.Id.imgDay3, Resource.Id.imgDay4, Resource.Id.imgDay5 };

            for (int i = 0; i < 5; i++)
            {
                img[i] = FindViewById<ImageView>(imresid[i]);
            }
            LoadDay(1);
            LoadDay(2);
            LoadDay(3);
            LoadDay(4);
            LoadDay(5);
        }

        private void BtnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Searched for city typed \\
                string city = txtSearch.Text;

                RestHandler objrest = new RestHandler();
                root = objrest.ExecuteRequest(city);
                rootDay = objrest.ExecuteRequestCurrent(city);

                LoadHamilton();
                Image();
                LoadDay(1);
                LoadDay(2);
                LoadDay(3);
                LoadDay(4);
                LoadDay(5);
            }
            catch
            {
                Toast.MakeText(this, txtSearch.Text + " not found please try a larger town", ToastLength.Long).Show();
                return;
            }

        }



        private void BtnGPS_Click(object sender, System.EventArgs e)
        {
            // Gets current GPS location \\
            getGpsAsync();


            try
            {
                // Searched for city typed \\
                string city = txtSearch.Text;

                RestHandler objrest = new RestHandler();
                root = objrest.ExecuteRequestlatlong(latitude,longitude);
                rootDay = objrest.ExecuteRequestCurrentlatlong(latitude,longitude);

                LoadHamilton();
                Image();
                LoadDay(1);
                LoadDay(2);
                LoadDay(3);
                LoadDay(4);
                LoadDay(5);
            }
            catch
            {
                Toast.MakeText(this,"GPS location not found please move closer to a town and try again", ToastLength.Long).Show();
                return;
            }

        }

        private void TxtSearch_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            // Still need code \\
            AutoCompleteTextView textView = FindViewById<AutoCompleteTextView>(Resource.Id.txtSearch);
            var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, CITYS);

            textView.Adapter = adapter;
        }

        public void LoadHamilton()
        {
            var newdate1 = UnixTimeStampToDateTime(root.list[0].dt);

            // Fill Daily Boxes Based On Daily Info \\
            txtCity.Text = rootDay.name;
            txtMain.Text = rootDay.weather[0].main;
            txtTemp.Text = " | " + Math.Round(rootDay.main.temp) + "°C";
            txtDescription.Text = newdate1.AddDays(0).DayOfWeek.ToString() +
                "\n" + rootDay.weather[0].description +
                "\n" + "Min " + Math.Round(rootDay.main.temp_min) + "°C" +
                 " | Max " + Math.Round(rootDay.main.temp_max) + "°C";


            // Fill Boxes Based On Weekly Info \\
            txtDay1.Text = newdate1.AddDays(1).DayOfWeek.ToString();
            txtDay2.Text = newdate1.AddDays(2).DayOfWeek.ToString();
            txtDay3.Text = newdate1.AddDays(3).DayOfWeek.ToString();
            txtDay4.Text = newdate1.AddDays(4).DayOfWeek.ToString();
            txtDay5.Text = newdate1.AddDays(5).DayOfWeek.ToString();

            txtDetails1.Text = Math.Round(root.list[0].temp.day).ToString() + "°C" +
                "\n" + "-" + root.list[0].weather[0].description + "-";

            txtDetails2.Text = Math.Round(root.list[1].temp.day).ToString() + "°C" +
                "\n" + "-" + root.list[1].weather[0].description + "-";

            txtDetails3.Text = Math.Round(root.list[2].temp.day).ToString() + "°C" +
                "\n" + "-" + root.list[2].weather[0].description + "-";

            txtDetails4.Text = Math.Round(root.list[3].temp.day).ToString() + "°C" +
                "\n" + "-" + root.list[3].weather[0].description + "-";

            txtDetails5.Text = Math.Round(root.list[4].temp.day).ToString() +
                "\n" + "-" + root.list[4].weather[0].description + "-";

        }
        public void Image()
        {
            if (rootDay.weather[0].id > 199)
            {
                imgMain.SetImageResource(Resource.Drawable.Thunderstorm);
            }
            if (rootDay.weather[0].id > 299)
            {
                imgMain.SetImageResource(Resource.Drawable.Drizzle);
            }
            if (rootDay.weather[0].id > 499)
            {
                imgMain.SetImageResource(Resource.Drawable.Rain);
            }
            if (rootDay.weather[0].id > 599)
            {
                imgMain.SetImageResource(Resource.Drawable.Snow);
            }
            if (rootDay.weather[0].id == 800)
            {
                imgMain.SetImageResource(Resource.Drawable.Clear);
            }
            if (rootDay.weather[0].id == 801 || rootDay.weather[0].id == 802)
            {
                imgMain.SetImageResource(Resource.Drawable.Clouds);
            }
            if (rootDay.weather[0].id == 803 || rootDay.weather[0].id == 804)
            {
                imgMain.SetImageResource(Resource.Drawable.Cloudy);
            }
            if (rootDay.weather[0].id == 511)
            {
                imgMain.SetImageResource(Resource.Drawable.Snow);
            }
        }
        public void LoadDay(int day)
        {
            if (root.list[day].weather[0].id > 199)
            {
                img[day - 1].SetImageResource(Resource.Drawable.Thunderstorm);
            }
            if (root.list[day].weather[0].id > 299)
            {
                img[day - 1].SetImageResource(Resource.Drawable.Drizzle);
            }
            if (root.list[day].weather[0].id > 499)
            {
                img[day - 1].SetImageResource(Resource.Drawable.Rain);
            }
            if (root.list[day].weather[0].id > 599)
            {
                img[day - 1].SetImageResource(Resource.Drawable.Snow);
            }
            if (root.list[day].weather[0].id == 800)
            {
                img[day - 1].SetImageResource(Resource.Drawable.Clear);
            }
            if (root.list[day].weather[0].id == 801 || root.list[day].weather[0].id == 802)
            {
                img[day - 1].SetImageResource(Resource.Drawable.Clouds);
            }
            if (root.list[day].weather[0].id == 803 || root.list[day].weather[0].id == 804)
            {
                img[day - 1].SetImageResource(Resource.Drawable.Cloudy);
            }
            if (root.list[day].weather[0].id == 511)
            {
                img[day - 1].SetImageResource(Resource.Drawable.Snow);
            }
        }


        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch \\
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        static string[] CITYS = new string[]
        {
            // Citys listed by Wiki \\
                "Ahaura", "Ahipara", "Ahititi", "Ahuroa", "Akaroa", "Albert Town", "Albury", "Alexandra", "Amberley", "Aranga",
                "Arapohue", "Arrowtown", "Ashburton", "Ashhurst", "Auckland", "Auroa", "Awanui", "Balclutha", "Balfour",
                "Barrytown", "Beachlands", "Beaumont", "Bell Block", "Blackball", "Blenheim", "Bluff", "Brighton",
                "Brightwater", "Broadwood", "Bulls", "Bunnythorpe", "Cambridge", "Canvastown", "Carterton", "Cheviot",
                "Christchurch", "Clarksville", "Clive", "Coatesville", "Colville", "Coroglen", "Coromandel", "Cromwell",
                "Dairy Flat", "Dannevirke", "Darfield", "Dargaville", "Dobson", "Drury", "Dunedin", "Duntroon", "Eastbourne",
                "Edgecumbe", "Egmont Village", "Eketahuna", "Eltham", "Fairhall", "Fairlie", "Featherston", "Feilding", "Flaxmere",
                "Fox Glacier", "Foxton", "Foxton Beach", "Frankton, Otago", "Frankton, Waikato", "Franz Josef",
                "Geraldine", "Gisborne", "Glenorchy", "Gore", "Granity", "Greymouth", "Greytown", "Grovetown", "Haast",
                "Hakataramea", "Hamilton", "Hanmer Springs", "Hari Hari", "Hastings", "Haupiri", "Havelock", "Havelock North", "Hawea",
                "Hawera", "Helensville", "Henley", "Herekino", "Hikuai", "Hikurangi", "Hikutaia", "Hinuera",
                "Hokitika", "Horeke", "Houhora", "Howick", "Huapai", "Huiakama", "Huirangi", "Hukerenui", "Huntly", "Hurleyville",
                "Inangahua Junction", "Inglewood", "Invercargill", "Jacobs River", "Kaiapoi", "Kaihu", "Kaikohe",
                "Kaikoura", "Kaimata", "Kaingaroa", "Kaipara Flats", "Kaitaia", "Kaitangata", "Kaiwaka", "Kakaramea",
                "Kaniere", "Kaponga", "Karamea", "Karetu", "Katikati", "Kaukapakapa", "Kauri", "Kawakawa", "Kawerau",
                "Kennedy Bay", "Kerikeri", "Kihikihi", "Kinloch", "Kokatahi", "Kokopu", "Koromiko", "Kumara", "Kumeu",
                "Kurow", "Kawhia", "Lawrence", "Leeston", "Leigh", "Lepperton", "Levin", "Lincoln", "Linkwater", "Little River",
                "Lower Hutt", "Lumsden", "Lyttelton", "Makahu", "Manaia, South Taranaki", "Manaia, Coromandel",
                "Manakau", "Mangakino", "Mangamuka", "Mangatoki", "Mangawhai", "Manukau", "Manurewa", "Maraetai",
                "Marco", "Maromaku", "Marsden Bay", "Martinborough", "Marton", "Maruia", "Masterton", "Matakana",
                "Matakohe", "Matamata", "Matapu", "Matarau", "Matihetihe", "Maungakaramea", "Maungatapere",
                "Maungaturoto", "Mayfield", "Methven", "Middlemarch", "Midhirst", "Millers Flat", "Milton", "Mimi", "Moana",
                "Moenui", "Moeraki", "Moerewa", "Mokau", "Mokoia", "Morrinsville", "Mosgiel", "Mossburn", "Motatau",
                "Motueka", "Mount Maunganui", "Mount Somers", "Murchison", "Murupara", "Napier", "Naseby", "Nelson",
                "New Brighton", "New Plymouth", "Normanby", "Ngaere", "Ngamatapouri", "Ngapara", "Ngaruawahia",
                "Ngataki", "Ngongotaha", "Ngunguru", "Norfolk", "North Shore City", "Oakura", "Oamaru", "Oban",
                "Ohakune", "Ohaeawai", "Ohangai", "Ohoka", "Ohope Beach", "Ohura", "Okaihau", "Okato", "Omanaia",
                "Omarama", "Omata", "Omokoroa", "Opononi", "Opotiki", "Opua", "Opunake", "Oratia", "Orewa",
                "Oromahoe", "Oruaiti", "Otaika", "Otaki", "Otakou", "Otautau", "Otiria", "Otorohanga", "Oxford", "Paekakariki",
                "Paeroa", "Pahiatua", "Paihia", "Pakaraka", "Pakiri", "Pakotai", "Palmerston", "Palmerston North",
                "Pamapuria", "Panguru", "Papakura", "Papamoa", "Paparoa", "Papatoetoe", "Parakai", "Paraparaumu",
                "Paroa", "Parua Bay", "Patea", "Pauanui", "Pauatahanui", "Peka Peka", "Pembroke", "Peria", "Petone",
                "Picton", "Piopio", "Pipiwai", "Pirongia", "Pleasant Point", "Plimmerton", "Porirua", "Portland", "Poroti",
                "Port Chalmers", "Portobello", "Pukekohe", "Pukerua Bay", "Pukeuri", "Pukepoto", "Purua", "Putaruru",
                "Queenstown", "Raetihi", "Raglan", "Rahotu", "Rai Valley", "Ramarama", "Ranfurly", "Rangiora", "Rapaura",
                "Ratapiko", "Raumati", "Rawene", "Rawhitiroa", "Reefton", "Renwick", "Richmond", "Riverhead",
                "Riverlands", "Riversdale Beach", "Riverton", "Rolleston", "Ross", "Rotorua", "Roxburgh", "Ruatoria",
                "Ruawai", "Runanga", "Russell", "Sanson", "Seddon", "Sheffield and Waddington", "Shannon",
                "Snells Beach", "Springfield", "Stratford", "Silverdale", "Spring Creek", "Taharoa", "Taihape",
                "Taipa-Mangonui", "Tairua", "Takaka", "Tangiteroria", "Tapanui", "Tapu", "Tangowahine", "Tapora", "Taradale",
                "Tauhoa", "Taumarunui", "Taupaki", "Taupo", "Tauranga", "Tauraroa", "Tautoro", "Te Anau", "Te Arai",
                "Te Aroha", "Te Awamutu", "Te Hapua", "Te Horo", "Te Kao", "Te Kopuru", "Te Kuiti", "Te Puke", "Te Puru",
                "Temuka", "Te Rerenga", "Thames", "Tikorangi‎", "Timaru", "Tinopai", "Tinwald", "Tirau", "Titoki", "Tokarahi",
                "Toko", "Tokoroa", "Tolaga Bay", "Tomarata", "Towai", "Tuakau", "Tuamarina", "Turangi", "Twizel",
                "Umawera", "Upper Hutt", "Urenui", "Uruti", "Waiheke Island", "Waharoa", "Waiharara", "Waihi",
                "Waihi Beach", "Waihola", "Waikanae", "Waikawa, Marlborough", "Waikawa, Southland", "Waima",
                "Waimangaroa", "Waimate", "Waimate North", "Waimauku", "Wainui", "Wainuiomata", "Waioneke",
                "Waiouru", "Waiotira", "Waipawa", "Waipukurau", "Wairakei", "Wairau Valley", "Wairoa", "Waitahuna",
                "Waikouaiti", "Waikuku", "Waitakere", "Waitara", "Waitaria Bay", "Waitoa", "Waitoki", "Waitoriki", "Waitotara",
                "Waiuku", "Wakefield", "Wallacetown", "Walton", "Waverley", "Wanaka", "Wanganui", "Ward", "Wardville",
                "Warkworth", "Wellington", "Wellsford", "Westport", "Whakatane", "Whakamaru", "Whananaki",
                "Whangamata", "Whangamomona", "Whangarei", "Whangarei Heads", "Whangaruru", "Whataroa",
                "Whatuwhiwhi", "Whenuakite", "Whenuakura", "Whiritoa", "Whitford", "Whitby", "Whitianga", "Willowby",
                "Wimbledon", "Winscombe", "Winton", "Woodend", "Woodhill", "Woodville", "Wyndham",
        };
        public async void getGpsAsync()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            string test = "Getting gps";

            var position = await locator.GetPositionAsync(TimeSpan.FromTicks(10000));

            if (position == null)
            {
                test = "null gps :(";
                return;
            }
            test = string.Format("Time: {0} \nLat: {1} \nLong: {2} \n Altitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \n Heading: {6} \n Speed: {7}",
            position.Timestamp, position.Latitude, position.Longitude,
            position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            latitude = position.Latitude;
            longitude = position.Longitude;
            Toast.MakeText(this, " GPS location is" + test, ToastLength.Long).Show();
        }





        //public async Task<Address> ReverseGeocodeCurrentLocation()
        //{
        //    GeoCoder geocoder = new GeoCoder(this);
        //    IList<Address> addresslist =
        //        await geocoder.GetFromLocationAsync(latitude, longitude)

        //}
    }
}

