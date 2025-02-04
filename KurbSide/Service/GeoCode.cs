﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KurbSide.Service
{
    public class GeoCode
    {
        public static async Task<Location> GetLocationAsync (string address)
        {
            string apiUrl = "https://api.opencagedata.com/geocode/v1/json?";
            string apiKey = Environment.GetEnvironmentVariable("opencagedata"); ;

            string requestUrl = $"{apiUrl}q={address}&key={apiKey}";

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(requestUrl);
                    HttpResponseMessage response = await client.GetAsync("");

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var jsonContent = JsonConvert.DeserializeObject<dynamic>(jsonString);

                        double lat = jsonContent["results"][0]["geometry"]["lat"];
                        double lng = jsonContent["results"][0]["geometry"]["lng"];
                        string fmt = jsonContent["results"][0]["formatted"];

                        return new Location(lat, lng, fmt);
                    }

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return new Location(0f, 0f, "bad key");
                    }

                    return new Location(0f, 0f, "invalid");
                }
            }
            catch (Exception)
            {
                return new Location(0f, 0f, "Invalid");
            }
        }

        public static Distance CalculateDistanceLocal(Location point1, Location point2)
        {
            var d1 = point1.lat * (Math.PI / 180.0);
            var num1 = point1.lng * (Math.PI / 180.0);
            var d2 = point2.lat * (Math.PI / 180.0);
            var num2 = point2.lng * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            var final = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));

            return new Distance(final, -1, point1, point2, "local");
        }

        public static async Task<Distance> CalculateDistanceAsync(Location point1, Location point2)
        {
            string apiUrl = "https://api.openrouteservice.org/v2/directions/driving-car?";
            string apiKey = Environment.GetEnvironmentVariable("openrouteservice");

            string requestUrl = $"{apiUrl}api_key={apiKey}&start={point1.lng},{point1.lat}&end={point2.lng},{point2.lat}";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync("");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var jsonContent = JsonConvert.DeserializeObject<dynamic>(jsonString);

                    JValue dst = jsonContent["features"][0]["properties"]["segments"][0]["distance"];
                    JValue tme = jsonContent["features"][0]["properties"]["segments"][0]["duration"];

                    return new Distance(dst.Value<float>(), tme.Value<float>(), point1, point2, jsonString);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new Distance(0, 0, point1, point2, requestUrl);
                }

                return new Distance(0, 0, point1, point2, "invalid");
            }
        }

    }
    public class Distance
    {
        public Distance(double dst, double tme, Location l1, Location l2, string debug  )
        {
            distance = dst;
            time = tme;
            loc1 = l1;
            loc2 = l2;
            this.debug = debug;
        }

        public double distance { get; }
        public double time { get; }

        public string debug { get; }

        public Location loc1 { get; }
        public Location loc2 { get; }
    }

    public class Location
    {
        public Location(double lat, double lng, string address)
        {
            this.lat = lat;
            this.lng = lng;
            this.address = address;
        }

        public double lat { get; }
        public double lng { get; }
        public string address { get; }
    }
}
