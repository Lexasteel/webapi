using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Json;

namespace WebApi.Models
{
    public class Device
    {
        [Key]
        public int ID { get; set; }
        [JsonProperty("datetime")]
        public DateTime Datetime { get; set; }
        public int? Unit { get; set; }
        public string? Data { get; set; }

        [NotMapped]
        public JObject JsonData
        {
            get
            {
                var json = JObject.Parse(Data.ToString());
                json["datetime"] = Datetime.ToString("s");
                return json;
            }
        }


    }
}
