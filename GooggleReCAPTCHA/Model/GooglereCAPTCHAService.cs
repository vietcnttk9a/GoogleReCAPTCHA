using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GooggleReCAPTCHA.Model
{
    public class GooglereCAPTCHAService
    {
        private ReCAPTCHASetting _setting;
        public GooglereCAPTCHAService(IOptions<ReCAPTCHASetting> settings )
        {
            _setting = settings.Value;
        }
        public virtual async Task<GoogleRespo> RecVer(string _Token)
        {
            GooglereCAPTCHAData _MyData = new GooglereCAPTCHAData
            {
                response = _Token,
                secret = _setting.ReCAPTCHA_Secret_Key
            };
            HttpClient client = new HttpClient();
            var repsponse = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_MyData.secret}&response={_MyData.response}");
            var capreps = JsonConvert.DeserializeObject<GoogleRespo>(repsponse);
            return capreps;
        }
    }
    public class GooglereCAPTCHAData
    {
        public string response { get; set; }
        public string secret { get; set; }
    }

    public class GoogleRespo
    {
        public bool success { get; set; }  //       "success": true|false,      // whether this request was a valid reCAPTCHA token for your site
        public double score { get; set; }   //"score": number             // the score for this request (0.0 - 1.0)
        public string action { get; set; }   //"action": string            // the action name for this request (important to verify)
        public DateTime challenge_ts { get; set; } //"challenge_ts": timestamp,  // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        public bool hostname { get; set; }  //"hostname": string,         // the hostname of the site where the reCAPTCHA was solved
        //public bool codes { get; set; } //"error-codes": [...]        // optional
    }
}
