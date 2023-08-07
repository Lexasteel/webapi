using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ApeController : ControllerBase
    {


        public List<ViewModelAlarm> viewModels = new List<ViewModelAlarm>();
        public string[] states = { "Хол.сост.", "Неост.сост", "Гор.сост." };
        public int state = 0;
        public TimeSpan timeState;

        private readonly StartsContext db;

        public ApeController(StartsContext context)
        {
            db = context;
        }

        //DateTime dateB = DateTime.Parse(dateStart, null, System.Globalization.DateTimeStyles.RoundtripKind);

        [HttpGet("GetAlarms")]
        public IActionResult GetAlarms(int unit, string dateStart="0", string dateEnd="0")
        {
            if (dateStart == "0" & dateEnd == "0") return Ok(viewModels);

            //DateTime datetimeB = new DateTime();
            //DateTime datetimeE = DateTime.Now;
            List<Stage> stages = new List<Stage>();

            if (dateStart != "0")

            {
                DateTime dateS = DateTime.Parse(dateStart, null, System.Globalization.DateTimeStyles.RoundtripKind);
                var slist = db.Stages.Where(w => w.Unit == unit & w.Date <= dateS).OrderByDescending(o => o.Date).Take(4).ToList();
                foreach (var s in slist)
                {
                    if (s.Number == 5) break;
                    if (s.Number == 4) stages.Add(s);
                    if (s.Number == 3) stages.Add(s);
                    if (s.Number == 2) stages.Add(s);
                    if (s.Number == 1) stages.Add(s);
                }
            }


            if (dateEnd != "0")
            {
                DateTime dateE = DateTime.Parse(dateEnd, null, System.Globalization.DateTimeStyles.RoundtripKind);
                stages.Add(db.Stages.FirstOrDefault(w => w.Date == dateE & w.Unit == unit));
            }
            //else
            //{
            //    datetimeE = datetimeB.AddHours(6);
            //}


            List<Signal> signals_list = db.Signals.Where(w => w.Unit == unit).ToList();
            //DateTime backData = datetimeE.AddMonths(-6);

            //var ooo = db.Stages.Where(w => w.Date >= backData && w.Date <= datetimeE && w.Unit == unit).OrderByDescending(o => o.Date).ToList().GroupBy(g => g.Number);

            //var fileUploaderController = new FileUploaderController(db);

            //foreach (var item in ooo)
            //{
            //    stages.Add(item.FirstOrDefault());
            //}
            Stage stage1 = null;
            Stage stage2 = null;
            Stage stage3 = null;
            Stage stage4 = null;
            Stage stage5 = null;



            //if (dateStart != "0")
            //{
           

            stage1 = stages.FirstOrDefault(f => f.Number == 1);
            stage2 = stages.FirstOrDefault(f => f.Number == 2);
            stage3 = stages.FirstOrDefault(f => f.Number == 3);
            stage4 = stages.FirstOrDefault(f => f.Number == 4);
            stage5 = stages.FirstOrDefault(f => f.Number == 5);

            //}

            //if (dateEnd != "0")
            //{

            //}

            Normativ normativ = new Normativ();
            float value = 0;


            if (stage1 != null)
            {
                viewModels.Add(new ViewModelAlarm(0)
                {
                    BeginTime = stage1.Date,
                    EndTime = stage2.Date,
                    Etap = 1,
                    Time = 30,
                    NormaStr = "30 мин",
                    Name = stage1.Title,

                });
            }
            if (stage2 != null)
            {
                viewModels.Add(new ViewModelAlarm(0)
                {
                    BeginTime = stage2.Date,
                    EndTime = stage2.Date.AddMinutes(10),
                    Etap = 2,
                    Time = 10,
                    NormaStr = 10 + " мин",
                    Name = new Stage() { Number = 6 }.Title

                });

                Signal cvdId = db.Signals.FirstOrDefault(f => f.Unit == unit && f.AddInfo == "time");
                //HistValue ttt = db.HistValues.FirstOrDefault(f => f.Date == stage3.Date);
                // Tables tables = new Tables();
                HistValue ttt = Tables.GetTable(db, unit, stage3.Date, stage3.Date.AddMinutes(1)).FirstOrDefault(f => f.Date == stage3.Date);//  db.HistValues.FirstOrDefault(f => f.Date == stage3.Date);
                value = ttt.JsonData[cvdId.ID.ToString()].ToObject<float>();


                //value = db.SeriesDatas.FirstOrDefault(f => f.SignalId == cvdId.ID && f.Date == stage3.Date.Value.Date).DictValues[stage3.Date.Value.TimeOfDay];

                normativ = db.Normativs.FirstOrDefault(f => f.CategoryID == 0 && f.Value == 2 && f.Units.Contains(unit.ToString()) && f.MinValue <= value && f.MaxValue > value);
                viewModels.Add(new ViewModelAlarm(0)
                {
                    BeginTime = stage2.Date.AddMinutes(10),
                    EndTime = stage3.Date,
                    Etap = 2,
                    Time = normativ.Time,
                    NormaStr = normativ.Time + " мин",
                    Name = stage2.Title + " (" + states[int.Parse(normativ.ValUnits)] + ") " + Math.Round(value, 0) + " " + cvdId.EngUnits,

                });
                state = int.Parse(normativ.ValUnits);
            }
            if (stage3 != null)
            {
                normativ = db.Normativs.FirstOrDefault(f => f.CategoryID == 0 && f.Value == 3 && f.Units.Contains(unit.ToString()) && f.MinValue <= value && f.MaxValue > value);
                viewModels.Add(new ViewModelAlarm(0)
                {
                    BeginTime = stage3.Date,
                    EndTime = stage4.Date,
                    Etap = 3,
                    Time = normativ.Time,
                    NormaStr = normativ.Time + " мин",
                    Name = stage3.Title,

                });
                timeState = stage3.Date.TimeOfDay;
            }
            if (stage4 != null)
            {
                normativ = db.Normativs.FirstOrDefault(f => f.CategoryID == 0 && f.Value == 4 && f.Units.Contains(unit.ToString()) && f.MinValue <= value && f.MaxValue > value);
                viewModels.Add(new ViewModelAlarm(0)
                {
                    BeginTime = stage4.Date,
                    EndTime = stage4.Date.AddMinutes(normativ.Time),
                    Etap = 4,
                    Time = normativ.Time,
                    NormaStr = normativ.Time + " мин",
                    Name = stage4.Title,
                });
                stage4.EndDate = stage4.Date.AddMinutes(normativ.Time);
            }

            if (stage5 != null)
            {
                viewModels.Add(new ViewModelAlarm(0)
                {
                    BeginTime = stage5.Date,
                    EndTime = stage5.Date,
                    Etap = 5,
                    Time = 0,
                    NormaStr = "",
                    Name = stage5.Title,
                });
            }


            if (stage5 != null && stage4 != null)
                if (stage4.EndDate > stage5.Date)
                {

                }
            if (stage4 != null)
            {

                if (unit == 3 || unit == 4)
                {
                    stage1 = new Stage() { Number = 1, Date = stage4.Date.AddHours(-3) };
                }

                if (stage1 != null)
                {
                    CalcCriterions(stage1.Date, stage4.EndDate, signals_list, unit);
                }

                
            }
            if (stage5 != null)
            {
                DateTime dateTimeStartStage5 = stage5.Date.AddMinutes(-30);
                if (stage4 != null)
                {

                    if (stage4.EndDate > dateTimeStartStage5)
                    {
                        dateTimeStartStage5 = stage4.EndDate;
                    }
                }
                CalcCriterions(dateTimeStartStage5, stage5.Date.AddMinutes(30), signals_list, unit);
            }



            //var serializerSettings = new System.Text.Json.JsonSerializerOptions();
            //serializerSettings.PropertyNameCaseInsensitive = false;
            return Ok(viewModels.OrderBy(o=>o.BeginTime));
        }

        void CalcCriterions(DateTime start, DateTime end, List<Signal> signals, int unit)
        {
            var query = (from signal in signals
                         where signal.CategoryID > 0
                         where signal.Unit == unit
                         join category in db.Categories on signal.CategoryID equals category.ID
                         join _normativ in db.Normativs on signal.CategoryID equals _normativ.CategoryID
                         select signal).ToList();

            //List<HistValue> histValues = db.HistValues.Where(w => w.Date >= start && w.Date <= end).OrderBy(o => o.Date).ToList();
            //Tables tables = new Tables();
            List<HistValue> histValues = Tables.GetTable(db, unit, start, end).OrderBy(o => o.Date).ToList();
            var categories = db.Categories.Select(s => s.ID).Where(w => w > 0).ToList();
            Parallel.ForEach<int>(categories, (category) =>
            {
                if (category == 22)
                {
                    if (unit != 3 && unit != 4)
                        Criterion22(unit, signals, category, histValues);
                }
                else
                {
                    // if (category==8)
                    CriterionSpeed(unit, category, signals.Where(w => w.CategoryID.Value == category), histValues);
                }
            });
        }
        private void CriterionSpeed(int unit, int category, IEnumerable<Signal> signals, List<HistValue> histValues)
        {
            //string s = $"No historical data unit {unit}";
            if (histValues.Count == 0) return; 
            DateTime end = histValues.LastOrDefault().Date;

            Parallel.ForEach<Signal>(signals, (signal, state_loop) =>
             {
                 HistValue firsthistvalue = histValues.FirstOrDefault();
                 if (firsthistvalue.JsonData[signal.ID.ToString()] == null)
                 {
                     state_loop.Break();
                 }
                 float prev = firsthistvalue.JsonData[signal.ID.ToString()].ToObject<float>(); ;

                 bool exit = false;
                 bool compare = false;
                 List<float> avgSpeed = new List<float>();
                 List<float> avgSpeedNorma = new List<float>();
                 float maxValue = 0;
                 ViewModelAlarm vms = new ViewModelAlarm(category);
                 Normativ normativ = null;
                 vms = null;
                 // HistValue histValue = new HistValue();

                 foreach (HistValue histValue in histValues)
                 {
                     //DateTime maxDate = series.Max(f => f.Date);
                     //TimeSpan maxTime = signal.SeriesDatas.FirstOrDefault(f => f.Date == maxDate).DictValues.Max(m => m.Key);
                     var currentValue = histValue.JsonData[signal.ID.ToString()].ToObject<float>();
                     //foreach (var dictValue in seriesData.JsonData)
                     //{
                     //Это заглушка для отладки!!!!!!!!!!!!!!!!

                     //if (seriesData.Date.Add(dictValue.Key) < start) continue;

                     if (exit) break;

                     if (category == 10)
                     {
                         if (state == 1 && histValue.Date.TimeOfDay < timeState)
                         {
                             normativ = signal.Category.Normativs.FirstOrDefault(f => f.State == state);
                         }
                         else
                         {
                             normativ = signal.Category.Normativs.FirstOrDefault(f => f.State != state && f.Units.Contains(unit.ToString()));
                         }
                     }
                     else
                     {
                         normativ = signal.Category.Normativs.FirstOrDefault(f => f.MinValue < currentValue && f.MaxValue > currentValue && f.Units.Contains(unit.ToString()));
                     }

                     if (normativ == null)
                     {
                         prev = currentValue;
                         continue;
                     }

                     if (normativ.Speed)
                     {
                         compare = (currentValue - prev) > normativ.Value;
                     }
                     else
                     {
                         compare = Math.Abs(currentValue) > Math.Abs(normativ.Value);
                     }


                     if (compare && vms == null)
                     {

                         vms = new ViewModelAlarm(category)
                         {
                             BeginTime = histValue.Date,
                             EndTime = histValue.Date,
                             Speed = normativ.Speed,
                             Unit = unit
                         };
                         var series = signals.Where(w => w.ID == signal.ID).Select(s => new { valueField = s.ID.ToString(), name = s.Desc, axis = "", type = "spline" }).ToList();
                         //var series = signals.Where(w => w.ID == signal.ID).Select(s => new { id = s.ID, valueField = "value", name = s.Desc, axis = "", type = "spline" }).ToList();
                         if (normativ.Speed)
                         {
                             series.Add(new { valueField = "norm", name = "Норматив", axis = "normativ", type = "line" });
                             series.Add(new { valueField = "spd", name = "Скорость", axis = "normativ", type = "spline" });
                         }

                         vms.AddInfo.Add(JsonConvert.SerializeObject(series));
                         prev = currentValue;
                         continue;
                     }
                     if (compare && vms != null)
                     {
                         vms.EndTime = histValue.Date;
                         avgSpeed.Add(currentValue - Convert.ToSingle(prev));
                         avgSpeedNorma.Add(normativ.Value);

                         if (Math.Abs(currentValue) > Math.Abs(maxValue)) maxValue = currentValue;
                     }


                     if (vms != null && (!compare || histValue.Date.Equals(end)))
                     {
                         if (vms.EndTime.Subtract(vms.BeginTime).TotalMinutes > normativ.Time)
                         {
                             if (normativ.Speed)
                             {
                                 vms.DevPoints = Math.Round(avgSpeed.Average(), 1) + normativ.EngUnits;
                                 vms.NormaStr = +Math.Round(avgSpeedNorma.Average(), 1) + normativ.EngUnits;
                                 vms.Name = signal.Desc + " (" + Math.Round(currentValue, 0) + signal.EngUnits + " - " + Math.Round(Convert.ToSingle(prev), 0) + signal.EngUnits + ")";
                             }
                             else
                             {
                                 vms.Name = signal.Desc + " (" + Math.Round(maxValue, 1) + signal.EngUnits + ")";
                                 vms.NormaStr = normativ.Value + normativ.EngUnits;
                             }


                             vms.Time = normativ.Time;

                             vms.AddInfo.Add(JsonConvert.SerializeObject(CreateConstLines(new DateTime[] { vms.BeginTime, vms.EndTime })));
                             vms.AddInfo.Add(JsonConvert.SerializeObject(CreateConstLinesValue(normativ)));
                             viewModels.Add(vms);
                         }
                         vms = new ViewModelAlarm(category);
                         vms = null;

                     }
                     prev = currentValue;
                 }
             });
        }




        List<JObject> CreateConstLines(DateTime[] dateTimes)
        {
            List<JObject> jObjects = new List<JObject>();
            foreach (DateTime item in dateTimes)
            {
                JObject jobj = new JObject();
                JObject label = new JObject();
                label.Add("text", item.ToString("HH:mm"));
                jobj.Add("label", label);
                jobj.Add("value", item);
                jobj.Add("color", "#8c8cff");
                jObjects.Add(jobj);
            }
            return jObjects;
        }

        List<JObject> CreateConstLinesValue(Normativ normativ)
        {
            List<JObject> jObjects = new List<JObject>();

            JObject jobj = new JObject();
            JObject label = new JObject();
            label.Add("text", normativ.Value + normativ.EngUnits);
            jobj.Add("label", label);
            jobj.Add("value", normativ.Value);
            jobj.Add("color", "#8c8cff");
            jObjects.Add(jobj);

            return jObjects;
        }
        [HttpGet("Chart")]
        public IActionResult Chart(string dB, string dE, string dict)
        {
            DateTime start = DateTime.Parse(dB).AddMinutes(-30);
            DateTime end = DateTime.Parse(dE).AddMinutes(30);
            if (start > end)
            {
                return Ok();
            }
            var jsonSeries = JsonConvert.DeserializeObject<List<Series>>(dict);
            List<int> idList = new List<int>();
            foreach (var item in jsonSeries)
            {
                //int n = item.id;
                var isNumeric = int.TryParse(item.valueField, out int n);
                if (n > 0) idList.Add(n);
            }
            List<Signal> signals = db.Signals.Where(x => idList.Contains(x.ID)).ToList();

            // List<HistValue> histValues = db.HistValues.Where(w => w.Date >= start && w.Date <= end).ToList();
            //Tables tables = new Tables();
            List<HistValue> histValues = Tables.GetTable(db, signals.FirstOrDefault().Unit, start, end).ToList();


            //var query = (from __signal in signals
            //             join series in db.SeriesDatas.Where(w => w.Date >= start.Date && w.Date <= end.Date).OrderBy(o => o.Date) on __signal.ID equals series.SignalId
            //             join normativ in db.Normativs on __signal.CategoryID equals normativ.CategoryID
            //             join category in db.Categories on __signal.CategoryID equals category.ID
            //             select new { series, normativ }).ToList();

            //List<ChartAlarm> js = new List<ChartAlarm>();
            List<JObject> js = new List<JObject>();
            float prev = 0;

            //Dictionary<DateTime, float> dictSeries = new Dictionary<DateTime, float>();

            foreach (Signal signal in signals)
            {
                var query = (from s in signals
                                 //join series in db.SeriesDatas.Where(w => w.Date >= start.Date && w.Date <= end.Date).OrderBy(o => o.Date) on __signal.ID equals series.SignalId
                             join normativ in db.Normativs on s.CategoryID equals normativ.CategoryID
                             join category in db.Categories on s.CategoryID equals category.ID
                             select new { s, normativ }).ToList();
            }
            foreach (HistValue histValue in histValues)
            {
                JObject jobj = new JObject();
                jobj.Add("date", histValue.Date);
                foreach (Signal signal in signals)
                {
                    float currentValue = histValue.JsonData[signal.ID.ToString()].ToObject<float>();
                    var norm = signal.Category.Normativs.FirstOrDefault(f => f.MinValue < currentValue && f.MaxValue > currentValue);
                    if (norm == null) { continue; };
                    //if (prev == null) { prev = currentValue; }
                    // ChartAlarm chartAlarm = new ChartAlarm() { Date = histValue.Date, Id = signal.ID, Value = currentValue };


                    jobj.Add(signal.ID.ToString(), histValue.JsonData[signal.ID.ToString()]);



                    if (norm.Speed)
                    {
                        jobj.Add("norm", norm.Value);
                        if (prev == 0)
                        {
                            prev = currentValue;
                        }
                        float spd = currentValue - prev;
                        if (spd < 0) spd = 0;
                        jobj.Add("spd", spd);
                        prev = currentValue;
                    }

                }

                js.Add(jobj);
                //js.Add(chartAlarm);
                //if (seriesData.Date.Add(dictValue.Key) > end)
                //{
                //    break;
                //}
            }




            var serializerSettings = new System.Text.Json.JsonSerializerOptions();
            serializerSettings.PropertyNameCaseInsensitive = true;
            return Ok(js);
        }

        void Criterion22(int unit, List<Signal> Signals, int category, List<HistValue> histValues)
        {
            //Task t = Task.Run(() =>
            //{

            ViewModelAlarm vms = new ViewModelAlarm(category);
            //Normativ normativ = db.Normativs.FirstOrDefault(w => w.CategoryID == cat);
            float prev = 0;

            Signal currentA = Signals.Find(f => f.AddInfo == "currentPenA");
            Signal flowA = Signals.Find(f => f.AddInfo == "flowPenA");
            Signal zdvANO = Signals.Find(f => f.AddInfo == "12" && f.Unit == unit);
            Signal zdvANC = Signals.Find(f => f.AddInfo == "13" && f.Unit == unit);

            Signal currentB = Signals.Find(f => f.AddInfo == "currentPenB");
            Signal flowB = Signals.Find(f => f.AddInfo == "flowPenB");
            Signal zdvBNO = Signals.Find(f => f.AddInfo == "22" && f.Unit == unit);
            Signal zdvBNC = Signals.Find(f => f.AddInfo == "23" && f.Unit == unit);



            Calc(zdvANO, zdvANC, flowA, "Незакрытие рециркуляции ПЭН-" + flowA.Unit + "A", currentA);
            Calc(zdvBNO, zdvBNC, flowB, "Незакрытие рециркуляции ПЭН-" + flowB.Unit + "Б", currentB);

            void Calc(Signal zdvNO, Signal zdvNC, Signal flow, string description, Signal current)
            {
                List<Signal> signals = new List<Signal>();
                signals.Add(flow);
                signals.Add(current);



                foreach (HistValue histValue in histValues)
                {
                    float currentValue = histValue.JsonData[flow.ID.ToString()].ToObject<float>();
                    //if (exit) break;
                    //if (flow.SeriesDatas[i].Date.Add(dictValue.Key) < start) continue;
                    //DateTime dateTime = flow.SeriesDatas[i].Date.Add(dictValue.Key);
                    //float _flow = dictValue.Value;
                    if (currentValue > flow.Category.Normativs.FirstOrDefault().Value && histValue.JsonData[zdvNC.ID.ToString()].ToObject<int>() == 1 && histValue.JsonData[zdvNO.ID.ToString()].ToObject<int>() == 0)
                    {

                        if (String.IsNullOrEmpty(vms.Name))
                        {
                            vms = new ViewModelAlarm(category)
                            {
                                BeginTime = histValue.Date,
                                EndTime = histValue.Date,
                                Name = description + " (" + Math.Round(currentValue, 0) + "т/ч",
                                Unit = flow.Unit

                            };
                        }
                        else
                        {
                            vms.EndTime = histValue.Date;
                            prev = currentValue;
                        }
                        continue;
                    }
                    //else
                    // {
                    if (vms.EndTime.Subtract(vms.BeginTime).TotalMinutes > flow.Category.Normativs.FirstOrDefault().Time)
                    {
                        var series = signals.Select(s => new { valueField = s.ID.ToString(), name = s.Desc, axis = "", type = "spline" }).ToList();
                        //series.Add(new { valueField = "norm", name = "Норматив", axis = "normativ", type = "line" });
                        //series.Add(new { valueField = "spd", name = "Скорость", axis = "normativ", type = "spline" });
                        //vms.DevPoints = Math.Round(avgSpeed.Average(), 1) + normativ.EngUnits;
                        //vms.Name += " - " + Math.Round(prev, 0) + signal.EngUnits + ")";
                        //vms.NormaStr = +Math.Round(avgSpeedNorma.Average(), 1) + normativ.EngUnits;
                        //vms.Norma = normativ.Norma;
                        vms.AddInfo.Add(JsonConvert.SerializeObject(series));
                        vms.AddInfo.Add(JsonConvert.SerializeObject(CreateConstLines(new DateTime[] { vms.BeginTime, vms.EndTime })));
                        vms.AddInfo.Add(JsonConvert.SerializeObject(CreateConstLinesValue(flow.Category.Normativs.FirstOrDefault())));

                        vms.Name += " - " + Math.Round(prev, 0) + "т/ч)";
                        vms.Time = 0;
                        viewModels.Add(vms);
                    }
                    vms = new ViewModelAlarm(category);
                    //if (dateTime > end) exit = true;
                    //}

                }
            }

            //});
            //await Task.WhenAll();

        }
        class Values
        {
            public DateTime date { get; set; }
            public DateTime endDate { get; set; }
            public int unit { get; set; }
            public DateTime oldDate { get; set; }
            public DateTime oldEndDate { get; set; }
        }

        //[HttpPost]
        //public ActionResult InsertStage(string values)
        //{

        //    var servalues = JsonConvert.DeserializeObject<Values>(values);

        //    if (servalues.date > new DateTime())
        //    {
        //        Stage stage = new Stage();
        //        stage.Date = servalues.date;
        //        stage.Number = 4;
        //        stage.Unit = servalues.unit;
        //        db.Stages.Add(stage);
        //        db.SaveChanges();
        //    }
        //    if (servalues.endDate > new DateTime())
        //    {
        //        Stage stage = new Stage();
        //        stage.Date = servalues.endDate;
        //        stage.Number = 5;
        //        stage.Unit = servalues.unit;
        //        db.Stages.Add(stage);
        //        db.SaveChanges();
        //    }

        //    return new OkResult();

        //}
        //[HttpPut("{id}")]
        //public ActionResult UpdateStage(string values)
        //{
        //    var servalues = JsonConvert.DeserializeObject<Values>(values);
        //    if (servalues.date > new DateTime())
        //    {
        //        Stage stage = db.Stages.FirstOrDefault(f => f.Date == servalues.oldDate && f.Unit == servalues.unit && f.Number == 4);
        //        stage.Date = servalues.date;

        //        db.SaveChanges();
        //    }
        //    if (servalues.endDate > new DateTime())
        //    {
        //        Stage stage = db.Stages.FirstOrDefault(f => f.Date == servalues.oldEndDate && f.Unit == servalues.unit && f.Number == 5);
        //        if (stage == null)
        //        {
        //            stage = new Stage() { Unit = servalues.unit, Number = 5 };
        //            db.Stages.Add(stage);

        //        }
        //        stage.Date = servalues.endDate;

        //        db.SaveChanges();
        //    }
        //    return new OkResult();
        //}
        //[HttpDelete("{id}")]
        //public ActionResult DeleteStage(string values)
        //{
        //    var servalues = JsonConvert.DeserializeObject<Values>(values);
        //    return new OkResult();
        //}
    }
}