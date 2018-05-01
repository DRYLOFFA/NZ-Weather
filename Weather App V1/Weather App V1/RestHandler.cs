using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using RestSharp;

namespace Weather_App_V1
{
    class RestHandler
    {
        public FiveDay.RootObject ExecuteRequest(string city)
        {
            var client = new RestClient("http://api.openweathermap.org/data/2.5/forecast/daily?APPID=eda779a0530f5471405b6257cb0bf2f1&q="+ city +",NZ&mode=json&units=metric&cnt=10");

            RestRequest request = new RestRequest();
            IRestResponse response = client.Execute(request);

            var obj = JsonConvert.DeserializeObject<FiveDay.RootObject>(response.Content);

            return obj;
        }

        public CurrentDay.RootObject ExecuteRequestCurrent(string city)
        {
            var client = new RestClient("http://api.openweathermap.org/data/2.5/weather?APPID=eda779a0530f5471405b6257cb0bf2f1&q="+city+",nz&units=metric");

            RestRequest request = new RestRequest();
            IRestResponse response = client.Execute(request);

            var obj = JsonConvert.DeserializeObject<CurrentDay.RootObject>(response.Content);

            return obj;
        }
        public FiveDay.RootObject ExecuteRequestlatlong(double lat, double lng)
        {

            //?lat={lat}&lon={lon}
            var client = new RestClient("http://api.openweathermap.org/data/2.5/forecast/daily?APPID=eda779a0530f5471405b6257cb0bf2f1&lat=" + lat + "&lon=" + lng + "&mode=json&units=metric&cnt=10");

            RestRequest request = new RestRequest();
            IRestResponse response = client.Execute(request);

            FiveDay.RootObject obj = new FiveDay.RootObject();
            obj = JsonConvert.DeserializeObject<FiveDay.RootObject>(response.Content);

            return obj;
        }

        public CurrentDay.RootObject ExecuteRequestCurrentlatlong(double lat, double lng)
        {
            var client = new RestClient("http://api.openweathermap.org/data/2.5/weather?APPID=eda779a0530f5471405b6257cb0bf2f1&lat=" + lat + "&lon=" + lng + "&units=metric");

            RestRequest request = new RestRequest();
            IRestResponse response = client.Execute(request);

            CurrentDay.RootObject obj = new CurrentDay.RootObject();
            obj = JsonConvert.DeserializeObject<CurrentDay.RootObject>(response.Content);

            return obj;
        }

    }
}