using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthDemoXForms.Models.Facebook
{
    public class FacebookSigninModel
    {
        public string Username { get; set; }
        public Properties Properties { get; set; }
        public Cookies Cookies { get; set; }
    }

    public class Properties
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("data_access_expiration_time")]
        public string DataAccessExpirationTime { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }

    public class Cookies
    {
        public int Capacity { get; set; }
        public int Count { get; set; }
        public int MaxCookieSize { get; set; }
        public int PerDomainCapacity { get; set; }
    }

    public class PictureData
    {
        public int Height { get; set; }
        [JsonProperty("is_silhouette")]
        public bool IsSilhouette { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
    }
}
