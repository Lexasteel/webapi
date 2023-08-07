using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PensController : ControllerBase
    {
        private readonly StartsContext db;

        public PensController(StartsContext context)
        {
            db = context;
        }
        [HttpGet("Chart")]
        public IActionResult Chart()
        {
            List<Pen> series = db.Pens.OrderBy(o => o.Datetime).ToList();
            List<JObject> jlist = new List<JObject>();
            List<Dictionary<string,object>> dList=new List<Dictionary<string,object>>();
            //MNK(nameof(Pen.Flow3A), nameof(Pen.Powr3A));
            //MNK(nameof(Pen.Flow3B), nameof(Pen.Powr3B));
            //MNK(nameof(Pen.Flow4A), nameof(Pen.Powr4A));
            //MNK(nameof(Pen.Flow4B), nameof(Pen.Powr4B));
            MNK(nameof(Pen.Flow5A), nameof(Pen.Powr5A), nameof(Pen.Time5A));
            MNK(nameof(Pen.Flow5B), nameof(Pen.Powr5B), nameof(Pen.Time5B));
            MNK(nameof(Pen.Flow6A), nameof(Pen.Powr6A), nameof(Pen.Time6A));
            MNK(nameof(Pen.Flow6B), nameof(Pen.Powr6B), nameof(Pen.Time6B));
            MNK(nameof(Pen.Flow7A), nameof(Pen.Powr7A), nameof(Pen.Time7A));
            MNK(nameof(Pen.Flow7B), nameof(Pen.Powr7B), nameof(Pen.Time7B));
            MNK(nameof(Pen.Flow8A), nameof(Pen.Powr8A), nameof(Pen.Time8A));
            MNK(nameof(Pen.Flow8B), nameof(Pen.Powr8B), nameof(Pen.Time8B));
            MNK(nameof(Pen.Flow9A), nameof(Pen.Powr9A), nameof(Pen.Time9A));
            MNK(nameof(Pen.Flow9B), nameof(Pen.Powr9B), nameof(Pen.Time9B));

            return Ok(dList);


            void MNK(string flow, string power, string time)
            {

                var _series = (from p in series
                               where (int)p.GetType().GetProperty(power).GetValue(p, null) > 0
                               select new
                               {
                                   flow = (double)(float)p.GetType().GetProperty(flow).GetValue(p, null),
                                   power = (int)p.GetType().GetProperty(power).GetValue(p, null),
                                   time = (double)(float)p.GetType().GetProperty(time).GetValue(p, null)
                               }).ToList();

                List<Matrix> list = (from p in _series select new Matrix() { x = p.flow, y = p.power / p.time }).ToList();
                double a = (list.Count * list.Sum(s => s.xy) - list.Sum(s => s.x) * list.Sum(s => s.y)) / (list.Count * list.Sum(s => s.x2) - Math.Pow(list.Sum(s => s.x), 2));
                double b = (list.Sum(s => s.y) - a * list.Sum(s => s.x)) / list.Count;
                //for (int i = 300; i < 600; i=i+50)
                //{
                //    JObject j = new JObject();


                //        j.Add(flow, i);
                //        j.Add(power, i * a + b);

                //    jlist.Add(j);
                //}

                //foreach (var item in _series)
                //{
                //JObject j = new JObject();
                double norm_min_flow = _series.Min(m => m.flow);

                double min = 350;
                if (norm_min_flow > min)
                {
                    while (norm_min_flow > min)
                    {
                        norm_min_flow = norm_min_flow - 30;
                    }
                }
                else
                {
                    while (norm_min_flow < 350)
                    {
                        norm_min_flow = norm_min_flow + 30;
                    }
                }

                double max = 540;
                
                    double norm_max_flow = _series.Max(m => m.flow);
                if (norm_max_flow>max)
                {
                    while (norm_max_flow > 540)
                    {
                        norm_max_flow = norm_max_flow - 30;
                    }
                }
                else
                {
                    while (norm_max_flow < 540)
                    {
                        norm_max_flow = norm_max_flow + 30;
                    }
                }
                


                


                //if ((double)(float)item.GetType().GetProperty(flow).GetValue(item, null) > 300)
                //{
                //    j.Add(flow, item.flow);
                //    j.Add(power, item.flow * a + b);
                //}

                //JObject j = new JObject();
                Dictionary<string, object> pair = new Dictionary<string, object>();

                //j.Add(flow, norm_min_flow);
                //j.Add(power, norm_min_flow * a + b);

                pair.Add(flow, norm_min_flow);
                pair.Add(power, norm_min_flow * a + b);


                //jlist.Add(j);
                dList.Add(pair);


                //j = new JObject();
                 pair = new Dictionary<string, object>();

                //j.Add(flow, norm_max_flow);
                //j.Add(power, norm_max_flow * a + b);

                pair.Add(flow, norm_max_flow);
                pair.Add(power, norm_max_flow * a + b);

                //jlist.Add(j);
                dList.Add(pair);

                string trend = a.ToString()+"*X";
                if (b<0)
                {
                    trend += b;
                }
                else
                {
                    trend += "+" + b.ToString();
                }

                //j = new JObject();
                pair = new Dictionary<string, object>();
                //j.Add("name", flow.Replace("Flow", "ПЭН-").Replace("B","Б"));
                //j.Add(flow.Replace("Flow", "trend"), trend);

                pair.Add("name", flow.Replace("Flow", "ПЭН-").Replace("B", "Б"));
                pair.Add("trend", trend);

               // jlist.Add(j);
                dList.Add(pair);
            }
        }


        [HttpGet("Power")]
        public IActionResult GetPenPower()
        {
            var pens = db.Pens.OrderBy(o => o.Datetime).ToList();

            //foreach (Pen item in pens)
            //{
            //    if (item.Time5A>0)
            //    {
            //        item.Powr5A = item.Time5A * 5;
            //    }
            //    if (item.Time5B > 0)
            //    {
            //        item.Powr5B = item.Time5B * 5;
            //    }
            //    if (item.Time6A > 0)
            //    {
            //        item.Powr6A = item.Time6A * 5;
            //    }
            //    if (item.Time6B > 0)
            //    {
            //        item.Powr6B = item.Time6B * 5;
            //    }
            //    if (item.Time7A > 0)
            //    {
            //        item.Powr7A = item.Time7A * 5;
            //    }
            //    if (item.Time7B > 0)
            //    {
            //        item.Powr7B = item.Time7B * 5;
            //    }
            //    if (item.Time8A > 0)
            //    {
            //        item.Powr8A = item.Time8A * 5;
            //    }
            //    if (item.Time8B > 0)
            //    {
            //        item.Powr8B = item.Time8B * 5;
            //    }
            //    if (item.Time9A > 0)
            //    {
            //        item.Powr9A = item.Time9A * 5;
            //    }
            //    if (item.Time9B > 0)
            //    {
            //        item.Powr9B = item.Time9B * 5;
            //    }

            //}
            //db.SaveChanges();
            return Ok(pens);
        }

        [HttpGet]
        public IActionResult Get(string date)
        {

            List<JObject> jObjects = new List<JObject>();
            //DateTime firstDay = DateTime.Parse(start, null, System.Globalization.DateTimeStyles.RoundtripKind);
            //DateTime lastDay = DateTime.Parse(end, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime datetime =  DateTime.Parse(date, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime nextMonth = datetime.AddMonths(1);
            //var signals = db.Signals.Where(w => w.AddInfo == "currentPenA" || w.AddInfo == "currentPenB").GroupBy(g => g.Unit);
            //for (DateTime i = new DateTime(2020, 1, 1, 0, 0, 0); i <= DateTime.Now; i = i.AddMonths(1))
            //{
            //    List<Ken> listKens = db.Kens.Where(w => w.Datetime >= i && w.Datetime < i.AddMonths(1)).ToList();
            //    if (listKens.Count == 0) { continue; }
            //    JObject pen = new JObject();
            //    pen.Add("Datetime", i.ToShortDateString());
            //    foreach (var block in signals)
            //    {
            //        foreach (var item in block)
            //        {

            //            List<Ken> listOnePen = listKens.Where(w => w.Signal_ID == item.ID).ToList();
            //            float time = listOnePen.Sum(s => s.Time);
            //            float flow = 0;
            //            foreach (var itemOnePen in listOnePen)
            //            {
            //                flow += itemOnePen.Downtime * itemOnePen.Time;
            //            }

            //            if (time > 0)
            //            {
            //                pen.Add(item.AddInfo + item.Unit + "flow", Math.Round(flow / time, 1));
            //            }
            //            else
            //            {
            //                pen.Add(item.AddInfo + item.Unit + "flow", 0);
            //            }


            //            // pen.Add(item.AddInfo + item.Unit + "time", time);

            //        }
            //    }
            //    jObjects.Add(pen);
            //}

            //var pens = db.Pens.Where(w => w.Datetime >= firstDay && w.Datetime < lastDay);



            var pen_ids = db.Signals.Where(w => w.AddInfo.Contains("currentPen")).Select(s => s.ID).ToArray();
            var query = (from pen in db.Kens
                         where pen_ids.Contains(pen.SignalID) && pen.Datetime>=datetime && pen.Datetime <nextMonth
                         join signal in db.Signals on pen.SignalID equals signal.ID
                         //select new {date=pen.Datetime, desc = signal.Desc, amp = pen.Amperage,  flow = pen.Downtime, time = pen.Time };
                         select new { pen, signal }).ToList().GroupBy(g => g.pen.Datetime).OrderBy(o=>o.Key);
            List<Pen> pens = new List<Pen>();
            foreach (var item in query)
            {
                Pen pen = new Pen() { Datetime = item.Key };
                foreach (var gitem in item)
                {
                    if (gitem.signal.Unit == 5)
                    {
                        if (gitem.signal.AddInfo.Contains("PenA"))
                        {
                            pen.Flow5A = gitem.pen.Downtime;
                            pen.Powr5A = (int)gitem.pen.Amperage;
                            pen.Time5A = gitem.pen.Time;
                        }
                        else
                        {
                            pen.Flow5B = gitem.pen.Downtime;
                            pen.Powr5B = (int)gitem.pen.Amperage;
                            pen.Time5B = gitem.pen.Time;
                        }

                    }
                    if (gitem.signal.Unit == 6)
                    {
                        if (gitem.signal.AddInfo.Contains("PenA"))
                        {
                            pen.Flow6A = gitem.pen.Downtime;
                            pen.Powr6A = (int)gitem.pen.Amperage;
                            pen.Time6A = gitem.pen.Time;
                        }
                        else
                        {
                            pen.Flow6B = gitem.pen.Downtime;
                            pen.Powr6B = (int)gitem.pen.Amperage;
                            pen.Time6B = gitem.pen.Time;
                        }

                    }
                    if (gitem.signal.Unit == 7)
                    {
                        if (gitem.signal.AddInfo.Contains("PenA"))
                        {
                            pen.Flow7A = gitem.pen.Downtime;
                            pen.Powr7A = (int)gitem.pen.Amperage;
                            pen.Time7A = gitem.pen.Time;
                        }
                        else
                        {
                            pen.Flow7B = gitem.pen.Downtime;
                            pen.Powr7B = (int)gitem.pen.Amperage;
                            pen.Time7B = gitem.pen.Time;
                        }

                    }
                    if (gitem.signal.Unit == 8)
                    {
                        if (gitem.signal.AddInfo.Contains("PenA"))
                        {
                            pen.Flow8A = gitem.pen.Downtime;
                            pen.Powr8A = (int)gitem.pen.Amperage;
                            pen.Time8A = gitem.pen.Time;
                        }
                        else
                        {
                            pen.Flow8B = gitem.pen.Downtime;
                            pen.Powr8B = (int)gitem.pen.Amperage;
                            pen.Time8B = gitem.pen.Time;
                        }

                    }
                    if (gitem.signal.Unit == 9)
                    {
                        if (gitem.signal.AddInfo.Contains("PenA"))
                        {
                            pen.Flow9A = gitem.pen.Downtime;
                            pen.Powr9A = (int)gitem.pen.Amperage;
                            pen.Time9A = gitem.pen.Time;
                        }
                        else
                        {
                            pen.Flow9B = gitem.pen.Downtime;
                            pen.Powr9B = (int)gitem.pen.Amperage;
                            pen.Time9B = gitem.pen.Time;
                        }

                    }
                }
                pens.Add(pen);

            }
            //var query_group = from q in query group q by q.date into qs select new { date = qs.Key, qs }; 
            //return Json(db.Pens.OrderBy(o=>o.Datetime).ToList());

            var serializerSettings = new System.Text.Json.JsonSerializerOptions();
            serializerSettings.PropertyNameCaseInsensitive = true;
            return Ok(pens);
        }

        public class PenPivot
        {
            public int Id { get; set; }
            public string Date { get; set; } = String.Empty;
            public string Unit { get; set; } = String.Empty;
            public string Pen { get; set; } = String.Empty;
            public double Flow { get; set; }
            public int Power { get; set; }
            public double Time { get; set; }

        }

        [HttpGet("PenPivot")]
        public IActionResult GetPivot()
        {

            List<JObject> jObjects = new List<JObject>();

            List<PenPivot> ListPenPivot = new List<PenPivot>();

            var pen_ids = db.Signals.Where(w => w.AddInfo.Contains("currentPen")).Select(s => s.ID).ToArray();
            var res = db.Kens.Where(w => pen_ids.Contains(w.SignalID)).Include("Signal");


            foreach (var item in res)
            {
                PenPivot pivot = new PenPivot();
                pivot.Id = item.ID;
                pivot.Date = item.Datetime.ToString("yyyy-MM-dd");
                pivot.Flow = Math.Round(item.Downtime,2);
                pivot.Power = (int)item.Amperage;
                pivot.Time = Math.Round(item.Time, 2);
                pivot.Unit = "БЛОК " + item.Signal.Unit.ToString();
                int index = item.Signal.Desc.IndexOf("ПЭН");
                pivot.Pen = "ПЭН-" + item.Signal.Desc.Substring(index+4, 2);
                ListPenPivot.Add(pivot);
            }

            var serializerSettings = new System.Text.Json.JsonSerializerOptions();
            serializerSettings.PropertyNameCaseInsensitive = true;
            return Ok(ListPenPivot);
        }


        [HttpPut("Power")]
        public async Task<IActionResult> Update()
        {
            string body = "";
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            var deser = JObject.Parse(body);

            int id = deser["id"]!.Value<int>();
            string values = deser["values"]!.ToString();

            Pen? pen = db.Pens.FirstOrDefault(f=>f.ID==id);
            if (pen == null)
                return StatusCode(409, "Pen not found");

            IDictionary<string, long>? valuesDict = JsonConvert.DeserializeObject<IDictionary<string,long>>(values);
            if (valuesDict is not null)
            {
                foreach (KeyValuePair<string, long> item in valuesDict)
                {
                    if (item.Key == "powr3A") pen.Powr3A = Convert.ToInt32(item.Value);
                    if (item.Key == "powr3B") pen.Powr3B = Convert.ToInt32(item.Value);
                    if (item.Key == "powr4A") pen.Powr4A = Convert.ToInt32(item.Value);
                    if (item.Key == "powr4B") pen.Powr4B = Convert.ToInt32(item.Value);
                    if (item.Key == "powr5A") pen.Powr5A = Convert.ToInt32(item.Value);
                    if (item.Key == "powr5B") pen.Powr5B = Convert.ToInt32(item.Value);
                    if (item.Key == "powr6A") pen.Powr6A = Convert.ToInt32(item.Value);
                    if (item.Key == "powr6B") pen.Powr6B = Convert.ToInt32(item.Value);
                    if (item.Key == "powr7A") pen.Powr7A = Convert.ToInt32(item.Value);
                    if (item.Key == "powr7B") pen.Powr7B = Convert.ToInt32(item.Value);
                    if (item.Key == "powr8A") pen.Powr8A = Convert.ToInt32(item.Value);
                    if (item.Key == "powr8B") pen.Powr8B = Convert.ToInt32(item.Value);
                    if (item.Key == "powr9A") pen.Powr9A = Convert.ToInt32(item.Value);
                    if (item.Key == "powr9B") pen.Powr9B = Convert.ToInt32(item.Value);
                }
                await db.SaveChangesAsync();
            }
            return Ok();
        }

        private class Matrix
        {
            public double x { get; set; }
            public double y { get; set; }
            public double xy { get { return x * y; } }
            public double x2 { get { return Math.Pow(x, 2); } }
        }

        private void PopulateModel(Pen model, IDictionary values)
        {



            string DATE = nameof(Pen.Datetime);
            // string Power = nameof(Pen.Datetime);

            if (values.Contains(DATE))
            {
                model.Datetime = Convert.ToDateTime(values[DATE]);
            }
        }
        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}