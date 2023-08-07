namespace WebApi.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<Normativ> Normativs { get; set; }
        public virtual IEnumerable<Signal> Signals { get; set; }

    }
}
