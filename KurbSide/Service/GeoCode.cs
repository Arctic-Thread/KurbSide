using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace KurbSide.Service
{
    public class GeoCode
    {

        public async Task<Location> GetLocationAsync (string address)
        {
            string apiUrl = "https://api.opencagedata.com/geocode/v1/json?";
            string apiKey = Environment.GetEnvironmentVariable("opencagedata"); ;

            string requestUrl = $"{apiUrl}q={address}&key={apiKey}";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync("");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var jsonContent = JsonConvert.DeserializeObject<dynamic>(jsonString);

                    float lat = jsonContent["results"][0]["geometry"]["lat"];
                    float lng = jsonContent["results"][0]["geometry"]["lng"];
                    string fmt = jsonContent["results"][0]["formatted"];

                    return new Location(lat, lng, fmt);
                }
                else if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new Location(0f, 0f, "bad key");
                }
                else
                {
                    return new Location(0f, 0f, "invalid");
                }
            }
        }

        public double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.lat * (Math.PI / 180.0);
            var num1 = point1.lng * (Math.PI / 180.0);
            var d2 = point2.lat * (Math.PI / 180.0);
            var num2 = point2.lng * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

    }

    public class Location
    {
        public Location(float lat, float lng, string address)
        {
            this.lat = lat;
            this.lng = lng;
            this.address = address;
        }

        public float lat { get; }
        public float lng { get; }
        public string address { get; }
    }
}
