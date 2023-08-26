using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  


    public class Ken 
    {
        [Key]
        public int ID { get; set; }
        public DateTime Datetime { get; set; }
        public string? Data { get; set; }

        [NotMapped]
        public JObject JsonData
        {
            get
            {
                if (Data is null)
                {
                    return new JObject();
                }
                return JObject.Parse(Data);
            }
        }



        public int? SignalID { get; set; }
        public double? Amperage { get; set; }
        public double? Time { get; set; }
        public double? Downtime { get; set; }
        public Signal Signal { get; set; }
    }
}

