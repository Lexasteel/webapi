using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Threading;
using System.Xml.Linq;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KensController : ControllerBase
    {
        private readonly StartsContext db;

        public KensController(StartsContext context)
        {
            db = context;
        }
        [HttpGet]
        private IActionResult Get(string date)
        {
            DateTime start = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime stop = start.AddMonths(1);

            //var kens = db.Kens.Where(w => w.Datetime >= firstDay && w.Datetime < lastDay).ToList().GroupBy(g => g.Datetime).OrderBy(o=>o.Key);
            //var signals = db.Signals.ToList();
            //Dictionary<int, int[]> id_list = new Dictionary<int, int[]>();
            //for (int i = 5; i <= 9; i++)
            //{
            //    id_list[i] = new int[] { signals.FirstOrDefault(s => s.AddInfo == "kenA" && s.Unit == i).ID, signals.FirstOrDefault(s => s.AddInfo == "kenB" && s.Unit == i).ID, signals.FirstOrDefault(s => s.AddInfo == "kenV" && s.Unit == i).ID, signals.FirstOrDefault(s => s.AddInfo == "generator" && s.Unit == i).ID };
            //}


            //List<Dictionary<string,object>> models = new List<Dictionary<string,object>>();


            //foreach (var item in kens)
            //{
            //    Dictionary<string,object> pair= new Dictionary<string, object>();
            //    pair.Add("date" , item.Key.ToString("s"));

            //    //JObject j = new JObject();
            //    //j.Add("datetime", item.Key);
            //    for (int b = 5; b <= 9; b++)
            //    {
            //        float denom = 0;
            //        float num = 0;
            //        for (int i = 0; i < 3; i++)
            //        {
            //            float time = GetTime(item, id_list[b][i]).Time;
            //            float amp = GetTime(item, id_list[b][i]).Amperage;
            //            //j.Add("Time" + (b).ToString() + i.ToString(), time);
            //            //j.Add("Amp" + (b).ToString() + i.ToString(), amp);

            //            pair.Add("time" + (b).ToString() + i.ToString(),time);
            //            pair.Add("amp" + (b).ToString() + i.ToString(),  amp);
            //            denom += time;
            //            num += amp * time;
            //        }
            //      //  j.Add("OneTime" + b, GetTime(item, -b).Time);
            //        pair.Add("onetime" + b, GetTime(item, -b).Time);

            //      //  j.Add("Avg" + (b), 0);
            //        pair.Add("avg" + (b), 0);

            //        var name = "avg" + (b);
            //        if (denom != 0)
            //        {
            //           // j["Avg" + (b)] = Math.Round((num * 1.0 / denom), 1);
            //            pair["avg" + (b)] = Math.Round((num * 1.0 / denom), 1);


            //        }
            //        //j.Add("Eff" + (b), Math.Round(j["Avg" + (b)].ToObject<float>() * Math.Sqrt(3.0) * 6300 * 0.85 * j["OneTime" + b].ToObject<float>() / 1000.0, 1));
            //        pair.Add("eff" + (b), Math.Round(Convert.ToDouble(pair["avg" + (b)]) * Math.Sqrt(3.0) * 6300 * 0.85 * Convert.ToDouble(pair["onetime" + b]) / 1000.0, 1));

            //    }

            //    models.Add(pair);
            //}
            //Ken GetTime(IGrouping<DateTime, Ken>  date, int id)
            //{
            //    Ken ken = date.FirstOrDefault(f => f.SignalID == id);
            //    if (ken != null)
            //        return ken;
            //    else
            //        return new Ken();
            //}
            var d = db.Devices.Where(w => w.Datetime >= start && w.Datetime < stop).GroupBy(g=>g.Datetime).ToList();
            List<JObject> res = new List<JObject>();
            foreach (var item in d)
            {   
                var j = new JObject();
                foreach (var item2 in item) {
                    j.Merge(item2.JsonData);
                }
                res.Add(j);
            }
            var s = JsonConvert.SerializeObject(res, Formatting.Indented);
            return Ok(s);

        }


    }
}