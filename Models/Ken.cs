using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Ken
    {
        [Key]
        public int ID { get; set; }
        public DateTime Datetime { get; set; }
        public int SignalID { get; set; }
        public float Amperage { get; set; }
        public float Time { get; set; }
        public float Downtime { get; set; }

        public string? Data { get; set; }
        public virtual Signal? Signal { get; set; }

    }

    public class Pivot_pump
    {
        [Key]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int SignalID { get; set; }
        public float Amperage { get; set; }
        public float Time { get; set; }
        public float Downtime { get; set; }
        public virtual Signal Signal { get; set; }
        public int Unit { get; set; }
        public string Pump { get; set; }
    }

}
