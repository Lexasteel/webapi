using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Normativ
	{
		public int ID { get; set; }
		public float Value { get; set; }
		public string ValUnits { get; set; }
		public float MinValue { get; set; }
		public float MaxValue { get; set; }
		public string EngUnits { get; set; }
		public int Time { get; set; }
		public int CategoryID { get; set; }
		public string Units { get; set; }
		public bool Speed { get; set; }
		public int State { get; set; }
		[NotMapped]
		public virtual List<Signal> Signals { get; set; }
		public virtual Category Category { get; set; }
	}
}
