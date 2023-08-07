using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{

    public abstract class Abstract_Histvalue
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Data { get; set; } = "{}";
        [NotMapped]
        public JObject JsonData { get { return JObject.Parse(Data); } }
    }
    public class HistValue: Abstract_Histvalue
    {


    }

    public class Hist3Value: Abstract_Histvalue
    { 
      
    }
    public class Hist4Value : Abstract_Histvalue
    { }
    public class Hist5Value : Abstract_Histvalue
    { }
    public class Hist6Value : Abstract_Histvalue
    { }
    public class Hist7Value : Abstract_Histvalue
    { }
    public class Hist8Value : Abstract_Histvalue
    { }
    public class Hist9Value : Abstract_Histvalue
    { }
}
