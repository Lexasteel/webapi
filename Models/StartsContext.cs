using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace WebApi.Models
{

    public class StartsContext : DbContext
    {
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Signal> Signals { get; set; }
        public DbSet<Normativ> Normativs { get; set; }
        public DbSet<Rou> Rous { get; set; }
        public DbSet<Pen> Pens { get; set; }
        public DbSet<Ken> Kens { get; set; }
        ////public DbSet<SeriesData> SeriesDatas { get; set; }
        public DbSet<Category> Categories { get; set; }

        ////  public DbSet<HistValue> HistValues { get; set; }
        public DbSet<Hist3Value> Hist3Values { get; set; }
        public DbSet<Hist4Value> Hist4Values { get; set; }
        public DbSet<Hist5Value> Hist5Values { get; set; }
        public DbSet<Hist6Value> Hist6Values { get; set; }
        public DbSet<Hist7Value> Hist7Values { get; set; }
        public DbSet<Hist8Value> Hist8Values { get; set; }
        public DbSet<Hist9Value> Hist9Values { get; set; }


        // public DbSet<Temp> Temps { get; set; }
        public DbSet<Log> Logs { get; set; }
        public StartsContext(DbContextOptions<StartsContext> options)
            : base(options)
        {
            // Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }

    public class Temp
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Data { get; set; }

        public JObject JsonData { get { return JObject.Parse(Data); } }
        public int Block { get; set; }
    }

    public class Log
    {
        public int ID { get; set; }
        public DateTime DTime { get; set; }
        public string Desc { get; set; }
        public Log(string desc)
        {
            DTime = DateTime.Now;
            Desc = desc;
        }


    }
    //public static class MyExtensions
    //{
    //    public static DateTime ToDateTime(this String date)
    //    {
    //        DateTime _date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    //        if (!String.IsNullOrEmpty(date))
    //        {
    //            int[] d = date.Split('-').Select(s => int.Parse(s)).ToArray();
    //            _date = new DateTime(d[0], d[1], d[2]);
    //        }
    //        return _date;
    //    }
    //}


    public static class Tables
    {
        public static List<HistValue> GetTable(StartsContext db, int unit, DateTime minDate, DateTime maxDate)
        {
            object histtable = new object();
            switch (unit)
            {
                case 3:
                    return db.Hist3Values.Where(w => w.Date >= minDate && w.Date < maxDate).Select(s => new HistValue() { Data = s.Data, Date = s.Date, ID = s.ID }).ToList();
                case 4:
                    return db.Hist4Values.Where(w => w.Date >= minDate && w.Date < maxDate).Select(s => new HistValue() { Data = s.Data, Date = s.Date, ID = s.ID }).ToList();
                case 5:
                    return db.Hist5Values.Where(w => w.Date >= minDate && w.Date < maxDate).Select(s => new HistValue() { Data = s.Data, Date = s.Date, ID = s.ID }).ToList();
                case 6:
                    return db.Hist6Values.Where(w => w.Date >= minDate && w.Date < maxDate).Select(s => new HistValue() { Data = s.Data, Date = s.Date, ID = s.ID }).ToList();
                case 7:
                    return db.Hist7Values.Where(w => w.Date >= minDate && w.Date < maxDate).Select(s => new HistValue() { Data = s.Data, Date = s.Date, ID = s.ID }).ToList();
                case 8:
                    return db.Hist8Values.Where(w => w.Date >= minDate && w.Date < maxDate).Select(s => new HistValue() { Data = s.Data, Date = s.Date, ID = s.ID }).ToList();
                case 9:
                    return db.Hist9Values.Where(w => w.Date >= minDate && w.Date < maxDate).Select(s => new HistValue() { Data = s.Data, Date = s.Date, ID = s.ID }).ToList();

                default:
                    histtable = null;
                    break;
            }


            return new List<HistValue>();
        }


        public static void SaveTable(StartsContext db, int unit, List<HistValue> histValues)
        {
            switch (unit)
            {
                case 3:
                    foreach (var item in histValues)
                    {
                        db.Hist3Values.Update(new Hist3Value() { Data = item.Data, Date = item.Date, ID = item.ID });
                    }
                    break;
                case 4:
                    foreach (var item in histValues)
                    {
                        db.Hist4Values.Update(new Hist4Value() { Data = item.Data, Date = item.Date, ID = item.ID });
                    }
                    break;
                case 5:
                    foreach (var item in histValues)
                    {
                        db.Hist5Values.Update(new Hist5Value() { Data = item.Data, Date = item.Date, ID = item.ID });
                    }
                    break;
                case 6:
                    foreach (var item in histValues)
                    {
                        db.Hist6Values.Update(new Hist6Value() { Data = item.Data, Date = item.Date, ID = item.ID });
                    }
                    break;
                case 7:
                    foreach (var item in histValues)
                    {
                        db.Hist7Values.Update(new Hist7Value() { Data = item.Data, Date = item.Date, ID = item.ID });
                    }
                    break;
                case 8:
                    foreach (var item in histValues)
                    {
                        db.Hist8Values.Update(new Hist8Value() { Data = item.Data, Date = item.Date, ID = item.ID });
                    }
                    break;
                case 9:
                    foreach (var item in histValues)
                    {
                        db.Hist9Values.Update(new Hist9Value() { Data = item.Data, Date = item.Date, ID = item.ID });
                    }
                    break;
                default:
                    break;
            }
        }

        //public static HistValue GetTableById(StartsContext db, int unit, int id)
        //{
        //    object histtable = new object();
        //    switch (unit)
        //    {
        //        case 3:
        //            Hist3Value s3 = db.Hist3Values.FirstOrDefault(f => f.ID == id);
        //            return new HistValue() { Data = s3.Data, Date = s3.Date, ID = s3.ID };
        //        case 4:
        //            Hist4Value s4 = db.Hist4Values.FirstOrDefault(f => f.ID == id);
        //            return new HistValue() { Data = s4.Data, Date = s4.Date, ID = s4.ID };
        //        case 5:
        //            Hist4Value s4 = db.Hist4Values.FirstOrDefault(f => f.ID == id);
        //            return new HistValue() { Data = s4.Data, Date = s4.Date, ID = s4.ID };
        //        case 6:
        //            Hist4Value s4 = db.Hist4Values.FirstOrDefault(f => f.ID == id);
        //            return new HistValue() { Data = s4.Data, Date = s4.Date, ID = s4.ID };
        //        case 7:
        //            Hist4Value s4 = db.Hist4Values.FirstOrDefault(f => f.ID == id);
        //            return new HistValue() { Data = s4.Data, Date = s4.Date, ID = s4.ID };
        //        case 8:
        //            Hist4Value s4 = db.Hist4Values.FirstOrDefault(f => f.ID == id);
        //            return new HistValue() { Data = s4.Data, Date = s4.Date, ID = s4.ID };
        //        case 9:
        //            Hist4Value s4 = db.Hist4Values.FirstOrDefault(f => f.ID == id);
        //            return new HistValue() { Data = s4.Data, Date = s4.Date, ID = s4.ID };

        //        default:
        //            histtable = null;
        //            break;
        //    }


        //    return new HistValue();
        //}
    }
}
