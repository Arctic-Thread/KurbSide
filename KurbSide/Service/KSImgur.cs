using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace KurbSide.Service
{
    public class KSImgur
    {
        private static readonly string imgurApiUrl = "https://api.imgur.com/3/";
        private static readonly string imgurSecret = Environment.GetEnvironmentVariable("imgur_api_secret");
        private static readonly string imgurClientID = "Client-ID 1f6dd4de0fc6655";
        private static readonly List<string> validImageFileExtensions = new List<string> { "image/jpeg", "image/bmp", "image/gif", "image/png" };

        /// <summary>
        /// Uploads an image within a form to Imgur via their API
        /// </summary>
        /// <remarks>
        /// Liam De Rivers
        /// </remarks>
        /// <param name="fileToUpload">The image to be uploaded from the form</param>
        /// <returns>The link to the image on the Imgur website</returns>
        public static async Task<string> KSUploadImageToImgur(IFormFile fileToUpload)
        {
            try
            {
                if (!validImageFileExtensions.Contains(fileToUpload.ContentType)) // If the file extension is not valid (in validImageFileExtensions)
                {
                    return "Error: Invalid File Type";
                }

                if (fileToUpload.Length > 20971520) // If File size is greater than 20MB
                {
                    return "Error: Image Size Is Too Large";
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(imgurApiUrl);
                    client.DefaultRequestHeaders.Add("Authorization", imgurClientID);
                    client.DefaultRequestHeaders.Add("Token", imgurSecret);

                    MultipartFormDataContent formData = new MultipartFormDataContent(); // The form to be uploaded to the Imgur API

                    using (var memoryStream = new MemoryStream())
                    {
                        await fileToUpload.CopyToAsync(memoryStream); // Saves the entire image file (creation date, author etc.) to memory
                        var rawContents = memoryStream.ToArray(); // rawContents is the image contents alone
                        HttpContent imageContents = new StringContent(Convert.ToBase64String(rawContents)); // imageContents is an httpContent entity containing the image contents
                        formData.Add(imageContents, "image"); // Add the imageContents to the "form"

                        HttpResponseMessage rawResponse = await client.PostAsync("image", formData); // The raw response from the Imgur API
                        if (rawResponse.IsSuccessStatusCode) // If the response code is 200-299 then it succeeded
                        {
                            var responseContent = await rawResponse.Content.ReadAsStringAsync(); // The response content from the Imgur API
                            var jsonResponseContent = JsonConvert.DeserializeObject<dynamic>(responseContent); // "Json-ified" response content
                            var linkToImage = jsonResponseContent["data"]["link"];
                            if (linkToImage != null)
                            {
                                return linkToImage;
                            }
                            else
                            {
                                return "Error: Something Went Wrong";
                            }
                        }
                        else
                        {
                            return "Error: " + rawResponse.ReasonPhrase;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.GetBaseException().Message;
            }
        }
    }
}