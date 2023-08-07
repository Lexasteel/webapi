using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Signal
    {
        public int ID { get; set; }
        public int Unit { get; set; }
        public int? CategoryID { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string EngUnits { get; set; }
        public string AddInfo { get; set; }
        public string Type { get; set; }
        [NotMapped]
        public IDictionary<DateTime, double> DateValues { get; set; }
        public virtual Category Category { get; set; }
    }
}
