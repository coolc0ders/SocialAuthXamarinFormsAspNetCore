using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthDemoXForms.Models
{
    [JsonObject]
    public class APIUser
    {
        public string TokenId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("verified_email")]
        public bool VerifiedEmail { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }
    }

    public class FacebookUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("profile_pic")]
        public string ProfilePic { get; set; }
        [JsonProperty("locale")]
        public string Locale { get; set; }
        [JsonProperty("timeZone")]
        public int Timezone { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
    }
}
