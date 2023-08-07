using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PUDPS.Models.ViewModels;
using WebApi.Models;

namespace WeoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShbmController : ControllerBase
    {
        private readonly StartsContext db;

        public ShbmController(StartsContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Get(string date)
        {
            DateTime _date = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.RoundtripKind);

            DateTime firstDay = _date;
            DateTime lastDay = firstDay.AddMonths(1);

            var kens = db.Kens.Where(w => w.Datetime >= firstDay && w.Datetime < lastDay).ToList().GroupBy(g => g.Datetime);
            var signals = db.Signals.Where(w => w.AddInfo.Contains("shbm")).ToList();
            int id_shbm5A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 5).ID;
            int id_shbm5B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 5).ID;
            int id_shbm6A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 6).ID;
            int id_shbm6B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 6).ID;
            int id_shbm7A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 7).ID;
            int id_shbm7B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 7).ID;
            int id_shbm8A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 8).ID;
            int id_shbm8B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 8).ID;
            int id_shbm9A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 9).ID;
            int id_shbm9B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 9).ID;
            List<ViewModelShbm> viewModelShbms = new List<ViewModelShbm>();

            foreach (var item in kens)
            {
                ViewModelShbm modelshbm = new ViewModelShbm() { Datetime = item.Key };
                foreach (var ken in item)
                {
                    modelshbm.Shbm5ATime = GetTime(item, id_shbm5A).Time;
                    modelshbm.Shbm5BTime = GetTime(item, id_shbm5B).Time;
                    modelshbm.Shbm5APower = GetTime(item, id_shbm5A).Amperage;
                    modelshbm.Shbm5BPower = GetTime(item, id_shbm5B).Amperage;
                    modelshbm.Shbm5Downtime = Math.Round(GetTime(item, id_shbm5A).Downtime + GetTime(item, id_shbm5B).Downtime, 1);
                    modelshbm.Shbm5Effect = Math.Round((GetTime(item, id_shbm5A).Downtime * modelshbm.Shbm5APower + GetTime(item, id_shbm5B).Downtime * modelshbm.Shbm5BPower) * 1000, 1);

                    modelshbm.Shbm6ATime = GetTime(item, id_shbm6A).Time;
                    modelshbm.Shbm6BTime = GetTime(item, id_shbm6B).Time;
                    modelshbm.Shbm6APower = GetTime(item, id_shbm6A).Amperage;
                    modelshbm.Shbm6BPower = GetTime(item, id_shbm6B).Amperage;
                    modelshbm.Shbm6Downtime = Math.Round(GetTime(item, id_shbm6A).Downtime + GetTime(item, id_shbm6B).Downtime, 1);
                    modelshbm.Shbm6Effect = Math.Round((GetTime(item, id_shbm6A).Downtime * modelshbm.Shbm6APower + GetTime(item, id_shbm6B).Downtime * modelshbm.Shbm6BPower) * 1000, 1);

                    modelshbm.Shbm7ATime = GetTime(item, id_shbm7A).Time;
                    modelshbm.Shbm7BTime = GetTime(item, id_shbm7B).Time;
                    modelshbm.Shbm7APower = GetTime(item, id_shbm7A).Amperage;
                    modelshbm.Shbm7BPower = GetTime(item, id_shbm7B).Amperage;
                    modelshbm.Shbm7Downtime = Math.Round(GetTime(item, id_shbm7A).Downtime + GetTime(item, id_shbm7B).Downtime, 1);
                    modelshbm.Shbm7Effect = Math.Round((GetTime(item, id_shbm7A).Downtime * modelshbm.Shbm7APower + GetTime(item, id_shbm7B).Downtime * modelshbm.Shbm7BPower) * 1000, 1);

                    modelshbm.Shbm8ATime = GetTime(item, id_shbm8A).Time;
                    modelshbm.Shbm8BTime = GetTime(item, id_shbm8B).Time;
                    modelshbm.Shbm8APower = GetTime(item, id_shbm8A).Amperage;
                    modelshbm.Shbm8BPower = GetTime(item, id_shbm8B).Amperage;
                    modelshbm.Shbm8Downtime = Math.Round(GetTime(item, id_shbm8A).Downtime + GetTime(item, id_shbm8B).Downtime, 1);
                    modelshbm.Shbm8Effect = Math.Round((GetTime(item, id_shbm8A).Downtime * modelshbm.Shbm8APower + GetTime(item, id_shbm8B).Downtime * modelshbm.Shbm8BPower) * 1000, 1);

                    modelshbm.Shbm9ATime = GetTime(item, id_shbm9A).Time;
                    modelshbm.Shbm9BTime = GetTime(item, id_shbm9B).Time;
                    modelshbm.Shbm9APower = GetTime(item, id_shbm9A).Amperage;
                    modelshbm.Shbm9BPower = GetTime(item, id_shbm9B).Amperage;
                    modelshbm.Shbm9Downtime = Math.Round(GetTime(item, id_shbm9A).Downtime + GetTime(item, id_shbm9B).Downtime, 1);
                    modelshbm.Shbm9Effect = Math.Round((GetTime(item, id_shbm9A).Downtime * modelshbm.Shbm9APower + GetTime(item, id_shbm9B).Downtime * modelshbm.Shbm9BPower) * 1000, 1);
                }
                viewModelShbms.Add(modelshbm);
            }
            Ken GetTime(IGrouping<DateTime, Ken> date, int id)
            {
                Ken ken = date.FirstOrDefault(f => f.SignalID == id);
                if (ken != null)
                    return ken;
                else
                    return new Ken() { Time = 0, Amperage = 0 };
            }

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                DateFormatString = "yyyy.MM.dd"
            };
            return Ok(viewModelShbms);
        }

        //[HttpGet("PivotShbm")]
        //public IActionResult GetPivot()
        //{

        //    var kens = db.Kens.ToList().GroupBy(g => g.Datetime);
        //    var signals = db.Signals.Where(w => w.AddInfo.Contains("shbm")).ToList();
        //    int id_shbm5A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 5).ID;
        //    int id_shbm5B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 5).ID;
        //    int id_shbm6A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 6).ID;
        //    int id_shbm6B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 6).ID;
        //    int id_shbm7A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 7).ID;
        //    int id_shbm7B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 7).ID;
        //    int id_shbm8A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 8).ID;
        //    int id_shbm8B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 8).ID;
        //    int id_shbm9A = signals.FirstOrDefault(s => s.AddInfo == "shbmA" && s.Unit == 9).ID;
        //    int id_shbm9B = signals.FirstOrDefault(s => s.AddInfo == "shbmB" && s.Unit == 9).ID;
        //    List<ViewModelShbm> viewModelShbms = new List<ViewModelShbm>();

        //    foreach (var item in kens)
        //    {
        //        ViewModelShbm modelshbm = new ViewModelShbm() { Id = item.Key };
        //        foreach (var ken in item)
        //        {
        //            modelshbm.Shbm5ATime = GetTime(item, id_shbm5A).Time;
        //            modelshbm.Shbm5BTime = GetTime(item, id_shbm5B).Time;
        //            modelshbm.Shbm5APower = GetTime(item, id_shbm5A).Amperage;
        //            modelshbm.Shbm5BPower = GetTime(item, id_shbm5B).Amperage;
        //            modelshbm.Shbm5Downtime = Math.Round(GetTime(item, id_shbm5A).Downtime + GetTime(item, id_shbm5B).Downtime, 1);
        //            modelshbm.Shbm5Effect = Math.Round((GetTime(item, id_shbm5A).Downtime * modelshbm.Shbm5APower + GetTime(item, id_shbm5B).Downtime * modelshbm.Shbm5BPower) * 1000, 1);

        //            modelshbm.Shbm6ATime = GetTime(item, id_shbm6A).Time;
        //            modelshbm.Shbm6BTime = GetTime(item, id_shbm6B).Time;
        //            modelshbm.Shbm6APower = GetTime(item, id_shbm6A).Amperage;
        //            modelshbm.Shbm6BPower = GetTime(item, id_shbm6B).Amperage;
        //            modelshbm.Shbm6Downtime = Math.Round(GetTime(item, id_shbm6A).Downtime + GetTime(item, id_shbm6B).Downtime, 1);
        //            modelshbm.Shbm6Effect = Math.Round((GetTime(item, id_shbm6A).Downtime * modelshbm.Shbm6APower + GetTime(item, id_shbm6B).Downtime * modelshbm.Shbm6BPower) * 1000, 1);

        //            modelshbm.Shbm7ATime = GetTime(item, id_shbm7A).Time;
        //            modelshbm.Shbm7BTime = GetTime(item, id_shbm7B).Time;
        //            modelshbm.Shbm7APower = GetTime(item, id_shbm7A).Amperage;
        //            modelshbm.Shbm7BPower = GetTime(item, id_shbm7B).Amperage;
        //            modelshbm.Shbm7Downtime = Math.Round(GetTime(item, id_shbm7A).Downtime + GetTime(item, id_shbm7B).Downtime, 1);
        //            modelshbm.Shbm7Effect = Math.Round((GetTime(item, id_shbm7A).Downtime * modelshbm.Shbm7APower + GetTime(item, id_shbm7B).Downtime * modelshbm.Shbm7BPower) * 1000, 1);

        //            modelshbm.Shbm8ATime = GetTime(item, id_shbm8A).Time;
        //            modelshbm.Shbm8BTime = GetTime(item, id_shbm8B).Time;
        //            modelshbm.Shbm8APower = GetTime(item, id_shbm8A).Amperage;
        //            modelshbm.Shbm8BPower = GetTime(item, id_shbm8B).Amperage;
        //            modelshbm.Shbm8Downtime = Math.Round(GetTime(item, id_shbm8A).Downtime + GetTime(item, id_shbm8B).Downtime, 1);
        //            modelshbm.Shbm8Effect = Math.Round((GetTime(item, id_shbm8A).Downtime * modelshbm.Shbm8APower + GetTime(item, id_shbm8B).Downtime * modelshbm.Shbm8BPower) * 1000, 1);

        //            modelshbm.Shbm9ATime = GetTime(item, id_shbm9A).Time;
        //            modelshbm.Shbm9BTime = GetTime(item, id_shbm9B).Time;
        //            modelshbm.Shbm9APower = GetTime(item, id_shbm9A).Amperage;
        //            modelshbm.Shbm9BPower = GetTime(item, id_shbm9B).Amperage;
        //            modelshbm.Shbm9Downtime = Math.Round(GetTime(item, id_shbm9A).Downtime + GetTime(item, id_shbm9B).Downtime, 1);
        //            modelshbm.Shbm9Effect = Math.Round((GetTime(item, id_shbm9A).Downtime * modelshbm.Shbm9APower + GetTime(item, id_shbm9B).Downtime * modelshbm.Shbm9BPower) * 1000, 1);
        //        }
        //        viewModelShbms.Add(modelshbm);
        //    }
        //    Ken GetTime(IGrouping<DateTime, Ken> date, int id)
        //    {
        //        Ken ken = date.FirstOrDefault(f => f.SignalID == id);
        //        if (ken != null)
        //            return ken;
        //        else
        //            return new Ken() { Time = 0, Amperage = 0 };
        //    }

        //    JsonSerializerSettings settings = new JsonSerializerSettings()
        //    {
        //        DateFormatString = "yyyy.MM.dd"
        //    };

        //    return Ok();
        //}
    }
}