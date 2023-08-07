using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDPS
{
    public class ChartAlarm
    {
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public float Value { get; set; }
        public float Norm { get; set; }
        public float Spd { get; set; }
    }
}
