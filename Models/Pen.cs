using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Pen
    {
        [Key]
        public int ID { get; set; }
        public DateTime Datetime { get; set; }
        public int Powr3A { get; set; }
        public float Flow3A { get; set; }
        public float Time3A { get; set; }
        public int Powr3B { get; set; }
        public float Flow3B { get; set; }
        public float Time3B { get; set; }
        public int Powr4A { get; set; }
        public float Flow4A { get; set; }
        public float Time4A { get; set; }
        public int Powr4B { get; set; }
        public float Flow4B { get; set; }
        public float Time4B { get; set; }
        public int Powr5A { get; set; }
        public float Flow5A { get; set; }
        public float Time5A { get; set; }
        public int Powr5B { get; set; }
        public float Flow5B { get; set; }
        public float Time5B { get; set; }
        public int Powr6A { get; set; }
        public float Flow6A { get; set; }
        public float Time6A { get; set; }
        public int Powr6B { get; set; }
        public float Flow6B { get; set; }
        public float Time6B { get; set; }
        public int Powr7A { get; set; }
        public float Flow7A { get; set; }
        public float Time7A { get; set; }
        public int Powr7B { get; set; }
        public float Flow7B { get; set; }
        public float Time7B { get; set; }
        public int Powr8A { get; set; }
        public float Flow8A { get; set; }
        public float Time8A { get; set; }
        public int Powr8B { get; set; }
        public float Flow8B { get; set; }
        public float Time8B { get; set; }
        public int Powr9A { get; set; }
        public float Flow9A { get; set; }
        public float Time9A { get; set; }
        public int Powr9B { get; set; }
        public float Flow9B { get; set; }
        public float Time9B { get; set; }
    }

    public class PenPowr
    {
        [Key]
        public int ID { get; set; }
        public DateTime Datetime { get; set; }
        public int Signal_ID { get; set; }
        public float Powr { get; set; }
      
    }
}
