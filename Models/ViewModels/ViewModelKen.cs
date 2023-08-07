using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDPS.Models.ViewModels
{
    public class ViewModelKen
    {
        public DateTime Id { get; set; }
        public Dictionary<string, float> data { get; set; } = new Dictionary<string, float>();
     
    }


    public class PairKen
    {
        public string Name { get; set; }
        public float Value { get; set; }
    }

    public class ViewKen
    {
        public DateTime DateTime { get; set; }
        public List<PairKen> Data { get; set; } = new List<PairKen>();
    }

}
