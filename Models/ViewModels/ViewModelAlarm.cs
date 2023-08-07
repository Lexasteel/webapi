using System;
using System.Collections.Generic;

namespace WebApi
{
    public class ViewModelAlarm
    {
        public int ID { get; set; }
        public int Unit { get; set; }
        public int Etap { get; set; }
        public int Criterion { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public string Interval
        {
            get
            {
                return EndTime.Subtract(BeginTime).TotalMinutes.ToString() + " мин.";
            }
        }
        public int Time { get; set; }
        public string NormaStr { get; set; }
        public string DevPoints { get; set; }
        public bool Speed { get; set; }
        public string Deviation
        {
            get
            {
                if (!Speed)
                {
                    double totMin = EndTime.Subtract(BeginTime).TotalMinutes - Time;
                    if (totMin > 0)
                    {
                        return "Превышение на " + totMin + " мин";
                    }
                    else
                    {
                        return DevPoints;
                    }
                }
                else
                {
                    return DevPoints;
                }

            }
        }
        public ViewModelAlarm(int criterion)
        {
            Criterion = criterion;
        }
        public List<string> AddInfo { get; set; } = new List<string>();
    }





    public class Series
    {
        public int id { get; set; }
        public string valueField { get; set; }
        public string name { get; set; }
        public string axis { get; set; }
    }

    //Public class ConstantLines
    //    {
    //            label: {
    //                text: ""
    //            },
    //            width: 2,
    //            value: constValue,
    //            color: "#8c8cff",
    //            dashStyle: "dash"
    //        }]

    
}
