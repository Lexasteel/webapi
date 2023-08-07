using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Rou
    {
        [Key]
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public float Unit3 { get; set; } = 0;
        public float Unit3Big { get; set; } = 0;
        public float Unit4 { get; set; } = 0;
        public float Unit5 { get; set; } = 0;
        public float Unit6 { get; set; } = 0;
        public float Unit7 { get; set; } = 0;
        public float Unit7Big { get; set; } = 0;
        public float Unit8 { get; set; } = 0;
        public float Unit9 { get; set; } = 0;
        public float Unit9Big { get; set; } = 0;
        public double Summa24 { get { return Math.Round((Unit3 + Unit4 + Unit5 + Unit6 + Unit7 + Unit8 + Unit9),2); } }
        public float Kpd { get; set; } = 0.83f;

        const float N = 24.0f;

        public double B
        {
            get
            {
                if (Kpd == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Round((Summa24 * 720 * 0.82 * 1000) / (7 * Kpd * 0.98),0);
                }
            }
        }
        public int Output { get; set; } = 0;
        public double Hourly { get { return Math.Round(Output / N,0); } }


        public double Deltab
        {
            get
            {
                if (Hourly == 0)
                {
                    return 0;
                }
                else
                {

                    return Math.Round(B / Hourly,2);
                }
            }
        }
        public int Output3 { get; set; } = 0;
        public int Output7 { get; set; } = 0;
        public int Output9 { get; set; } = 0;

        public double Summa140 { get { return Math.Round((Unit3Big + Unit7Big + Unit9Big), 2); } }
        public double Deltab3
        {
            get
            {
                if (Output3 == 0)
                {
                    return 0;
                }
                else
                {

                    return Math.Round((Unit3Big * 810 * 24 * 1000 / (Kpd * 0.98 * 7)) / Output3, 2);
                }
            }
        }
        public double Deltab7
        {
            get
            {
                if (Output7 == 0)
                {
                    return 0;
                }
                else
                {

                    return Math.Round((Unit7Big * 810 * 24 * 1000 / (Kpd * 0.98 * 7)) / Output7, 2);
                }
            }
        }
        public double Deltab9
        {
            get
            {
                if (Output9 == 0)
                {
                    return 0;
                }
                else
                {

                    return Math.Round((Unit9Big * 810 * 24 * 1000 / (Kpd * 0.98 * 7)) / Output9, 2);
                }
            }
        }

        public double Deltab140
        {
            get
            {
                if (Output == 0)
                {
                    return 0;
                }
                else
                {

                    return Math.Round((Deltab3*Output3 + Deltab7*Output7+Deltab9*Output9)/Output, 2);
                }
            }
        }

        public double DeltaN
        {
            get
            {
                return Math.Round((Summa24 * 735 / 1000) * 0.429059 * 0.778 * 100 / 86, 2);
            }
        }

    }
}