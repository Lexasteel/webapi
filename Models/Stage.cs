using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Stage
    {
        public int ID { get; set; }
        public int Unit { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public DateTime? Stage1 { get
            {
                if (this.Number == 1) { return this.Date; }
                else return null;
            }
        }
        [NotMapped]
        public DateTime? Stage2
        {
            get
            {
                if (this.Number == 2) { return this.Date; }
                else return null;
            }
        }
        [NotMapped]
        public DateTime? Stage3
        {
            get
            {
                if (this.Number == 3) { return this.Date; }
                else return null;
            }
        }
        [NotMapped]
        public DateTime? Stage4
        {
            get
            {
                if (this.Number == 4) { return this.Date; }
                else return null;
            }
        }
        [NotMapped]
        public DateTime? Stage5
        {
            get
            {
                if (this.Number == 5) { return this.Date; }
                else return null;
            }
        }
        [NotMapped]
        public DateTime EndDate { get; set; }

        [NotMapped]
        public string Title
        {
            get
            {
                switch (Number)
                {
                    case 1: return "Набор вакуума";
                    case 2: return "Растопка котла";
                    case 3: return "Набор оборотов";
                    case 4: return "Включение в сеть";
                    case 5: return "Останов";
                    case 6: return "Вентиляция топки";
                    default: return "";
                }
            }
        }
        [NotMapped]
        public string icon
        {
            get
            {
                if (this.Number == 4)
                {
                    return "arrowup";
                } else
                if (this.Number == 5)
                {
                    return "arrowdown";
                }
                else
                {
                    return String.Empty;
                }
            }
        }
        [NotMapped]
        public string badge
        {
            get
            {
                        return this.Date.ToString("dd.MM HH:mm");
            }
        }
        [NotMapped]
        public string text
        {
            get
            {
                return this.Unit.ToString();
            }
        }
    }
}
