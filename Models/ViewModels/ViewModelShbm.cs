using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PUDPS.Models.ViewModels
{
    public class ViewModelShbm
    {
        public DateTime Id { get; set; }
        public float Shbm5ATime { get; set; }
        public float Shbm5BTime { get; set; }
        public double Shbm5Downtime { get; set; }
        public float Shbm5APower { get; set; }
        public float Shbm5BPower { get; set; }
        public double Shbm5AveragePower
        {
            get
            {
                return Math.Round(GetAveragePower(Shbm5ATime, Shbm5BTime, Shbm5APower, Shbm5BPower), 1);
            }
        }
        public double Shbm5Effect { get; set; }
        public float Shbm6ATime { get; set; }
        public float Shbm6BTime { get; set; }
        public double Shbm6Downtime { get; set; }
        public float Shbm6APower { get; set; }
        public float Shbm6BPower { get; set; }
        public double Shbm6AveragePower
        {
            get
            {
                return Math.Round(GetAveragePower(Shbm6ATime, Shbm6BTime, Shbm6APower, Shbm6BPower), 1);
            }
        }
        public double Shbm6Effect { get; set; }
        public float Shbm7ATime { get; set; }
        public float Shbm7BTime { get; set; }
        public double Shbm7Downtime { get; set; }
        public float Shbm7APower { get; set; }
        public float Shbm7BPower { get; set; }
        public double Shbm7AveragePower
        {
            get
            {
                return Math.Round(GetAveragePower(Shbm7ATime, Shbm7BTime, Shbm7APower, Shbm7BPower), 1);
            }
        }
        public double Shbm7Effect { get; set; }
        public float Shbm8ATime { get; set; }
        public float Shbm8BTime { get; set; }
        public double Shbm8Downtime { get; set; }
        public float Shbm8APower { get; set; }
        public float Shbm8BPower { get; set; }
        public double Shbm8AveragePower
        {
            get
            {
                return Math.Round(GetAveragePower(Shbm8ATime, Shbm8BTime, Shbm8APower, Shbm8BPower), 1);
            }
        }
        public double Shbm8Effect { get; set; }
        public float Shbm9ATime { get; set; }
        public float Shbm9BTime { get; set; }
        public double Shbm9Downtime { get; set; }
        public float Shbm9APower { get; set; }
        public float Shbm9BPower { get; set; }
        public double Shbm9AveragePower
        {
            get
            {
                return Math.Round(GetAveragePower(Shbm9ATime, Shbm9BTime, Shbm9APower, Shbm9BPower), 1);
            }
        }
        public double Shbm9Effect { get; set; }

        //public double Ken9Effect
        //{
        //    get
        //    {
        //        return GetEffect(Ken9AverageCurrent, Ken9OneTime);
        //    }
        //}
        double GetAveragePower(float timeA, float timeB, float curA, float curB)
        {

            if ((timeA + timeB) == 0)
                return 0;
            else
            {
                return (curA * timeA + curB * timeB) * 1.0 / (timeA + timeB);
            }
        }

        //double GetEffect(double averCur, float oneTime)
        //{
        //    return Math.Round((averCur * Math.Sqrt(3.0) * 6300 * 0.85 * oneTime / 1000.0),1);
        //}


    }

}
