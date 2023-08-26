using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploaderController : ControllerBase
    {
        private readonly StartsContext db;
        public FileUploaderController(StartsContext context)
        {
            db = context;
        }

        List<string> list_info = new List<string>()
            { "kenA", "kenB", "kenV", "shbmA", "shbmB", "flowPenA", "currentPenA", "flowPenB", "currentPenB", "dehmw" };

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload(IFormFile file)
        {

            var myFile = Request.Form.Files["File"];
            int unit = int.Parse(myFile.FileName[0].ToString());
            var s = db.Signals.FirstOrDefault(w => w.Unit == unit & w.Desc == "Состояние энергоблока");
            int id = 0;
            if (s != null)
            {
                id = s.ID;
            }
            List<HistValue> listTemps = new List<HistValue>();
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var reader = new System.IO.StreamReader(myFile.OpenReadStream(), Encoding.GetEncoding("Windows-1251")))
                {
                    List<string[]> list = new List<string[]>();
                    List<Signal> signals = db.Signals.Where(w => w.Unit == unit).ToList();
                    List<string> headers = reader.ReadLine().Split(';').ToList();
                    List<string> engUnits = reader.ReadLine().Split(';').ToList();

                    while (!reader.EndOfStream)
                    {
                        string[] line = reader.ReadLine().Split(';');
                        list.Add(line);
                    }
                    string dateFormat = "dd.MM.yy HH:mm:ss";
                    List<Stage> stages = new List<Stage>();
                    foreach (string[] row in list)
                    {
                        JObject jObject = new JObject();
                        DateTime date = DateTime.ParseExact(row[0], dateFormat, CultureInfo.InvariantCulture);
                        HistValue hv = new HistValue() { Date = date };

                        for (int i = 0; i < headers.Count; i++)
                        {
                            Signal signal = signals.FirstOrDefault(f => f.Desc == headers[i]);
                            if (signal == null) continue;
                            if (signal.Type == "A")
                            {
                                jObject[signal.ID.ToString()] = float.Parse(row[i], CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                jObject[signal.ID.ToString()] = int.Parse(row[i].Substring(0, 1), System.Globalization.NumberStyles.Any);
                            }
                            if (signal.ID == id)
                            {
                                int number = int.Parse(row[i].Substring(0, 1));
                                for (int n = 1; n <= 5; n++)
                                {
                                    if (number == n)
                                    {
                                        stages.Add(new Stage() { Date = date, Number = n, Unit = unit });
                                    }
                                }
                            }
                        }
                        hv.Data = JsonConvert.SerializeObject(jObject);
                        listTemps.Add(hv);
                    }
                    if (stages.Count > 0)
                    {
                        foreach (var stage in stages)
                        {
                            var fs = db.Stages.FirstOrDefault(f => f.Date == stage.Date & f.Unit == stage.Unit);
                            if (fs == null)
                            {
                                db.Stages.Add(stage);
                            }
                        }
                        db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            DateTime min = listTemps.Min(m => m.Date).Date;
            DateTime max = new DateTime();
            switch (unit)
            {
                case 3:
                    //max = db.Hist3Values.Max(m => m.Date).Date;
                    //if (min > max)
                    //{
                    //    db.Hist3Values.AddRange(listTemps.Select(d => new Hist3Value() { Date = d.Date, Data = d.Data }));
                    //}
                    //else
                    //{
                    foreach (HistValue h in listTemps)
                    {
                        var hv = db.Hist3Values.FirstOrDefault(f => f.Date == h.Date);
                        if (hv is null)
                        {
                            db.Hist3Values.Add(new Hist3Value() { Date = h.Date, Data = h.Data });
                        }
                        else
                        {
                            hv.Data = h.Data;
                        }
                    }
                    //}
                    break;
                case 4:

                    //max = db.Hist4Values.Max(m => m.Date).Date;
                    //if (min > max)
                    //{
                    //    db.Hist4Values.AddRange(listTemps.Select(d => new Hist4Value() { Date = d.Date, Data = d.Data }));
                    //}
                    //else
                    //{
                    foreach (HistValue h in listTemps)
                    {
                        var hv = db.Hist4Values.FirstOrDefault(f => f.Date == h.Date);
                        if (hv is null)
                        {
                            db.Hist4Values.Add(new Hist4Value() { Date = h.Date, Data = h.Data });
                        }
                        else
                        {
                            hv.Data = h.Data;
                        }
                    }
                    //}
                    break;
                case 5:
                    //max = db.Hist5Values.Max(m => m.Date).Date;
                    //if (min > max)
                    //{
                    //    db.Hist5Values.AddRange(listTemps.Select(d => new Hist5Value() { Date = d.Date, Data = d.Data }));
                    //}
                    //else
                    //{
                    foreach (HistValue h in listTemps)
                    {
                        var hv = db.Hist5Values.FirstOrDefault(f => f.Date == h.Date);
                        if (hv is null)
                        {
                            db.Hist5Values.Add(new Hist5Value() { Date = h.Date, Data = h.Data });
                        }
                        else
                        {
                            hv.Data = h.Data;
                        }
                    }
                    //}
                    break;
                case 6:
                    //max = db.Hist6Values.Max(m => m.Date).Date;
                    //if (min > max)
                    //{
                    //    db.Hist6Values.AddRange(listTemps.Select(d => new Hist6Value() { Date = d.Date, Data = d.Data }));
                    //}
                    //else
                    //{
                    foreach (HistValue h in listTemps)
                    {
                        var hv = db.Hist6Values.FirstOrDefault(f => f.Date == h.Date);
                        if (hv is null)
                        {
                            db.Hist6Values.Add(new Hist6Value() { Date = h.Date, Data = h.Data });
                        }
                        else
                        {
                            hv.Data = h.Data;
                        }
                    }
                    //}
                    break;
                case 7:
                    //max = db.Hist7Values.Max(m => m.Date).Date;
                    //if (min > max)
                    //{
                    //    db.Hist7Values.AddRange(listTemps.Select(d => new Hist7Value() { Date = d.Date, Data = d.Data }));
                    //}
                    //else
                    //{
                    foreach (HistValue h in listTemps)
                    {
                        var hv = db.Hist7Values.FirstOrDefault(f => f.Date == h.Date);
                        if (hv is null)
                        {
                            db.Hist7Values.Add(new Hist7Value() { Date = h.Date, Data = h.Data });
                        }
                        else
                        {
                            hv.Data = h.Data;
                        }
                    }
                    //}
                    break;
                case 8:
                    //max = db.Hist8Values.Max(m => m.Date).Date;
                    //if (min > max)
                    //{
                    //    db.Hist8Values.AddRange(listTemps.Select(d => new Hist8Value() { Date = d.Date, Data = d.Data }));
                    //}
                    //else
                    //{
                    foreach (HistValue h in listTemps)
                    {
                        var hv = db.Hist8Values.FirstOrDefault(f => f.Date == h.Date);
                        if (hv == null)
                        {
                            db.Hist8Values.Add(new Hist8Value() { Date = h.Date, Data = h.Data });
                        }
                        else
                        {
                            hv.Data = h.Data;
                        }
                    }
                    //}
                    break;
                case 9:
                    //max = db.Hist9Values.Max(m => m.Date).Date;
                    //if (min > max)
                    //{
                    //    db.Hist9Values.AddRange(listTemps.Select(d => new Hist9Value() { Date = d.Date, Data = d.Data }));
                    //}
                    //else
                    //{
                    foreach (HistValue h in listTemps)
                    {
                        var hv = db.Hist9Values.FirstOrDefault(f => f.Date == h.Date);
                        if (hv == null)
                        {
                            db.Hist9Values.Add(new Hist9Value() { Date = h.Date, Data = h.Data });
                        }
                        else
                        {
                            hv.Data = h.Data;
                        }
                    }
                    //}
                    break;

                default:
                    break;
            }
            db.SaveChanges();
           
            List<string> result = new List<string>();
            var res = new Res() { unit = unit, dateMin = listTemps.Min(m => m.Date).ToString("s"), dateMax = listTemps.Max(m => m.Date).AddMinutes(1).ToString("s") };
            result.Add(CalcKenMill(res.unit, res.dateMin, res.dateMax));
            result.Add(CalcPen(res.unit, res.dateMin, res.dateMax));
            result.Add(CalcRou(res.unit, res.dateMin, res.dateMax));
            //result.Add(CalcMetall(res.unit, res.dateMin, res.dateMax));
            return Ok(result);

        }


        [HttpGet("Recalc")]
        public IActionResult ReCalc(int unit, string date)
        {

            date = date.Substring(0, 10);
            string minDate = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.RoundtripKind).ToString("s");

            DateTime mDate = DateTime.Now;
            string maxDate = mDate.Add(-mDate.TimeOfDay).ToString("s");
            List<string> res = new List<string>();
            int startRou = 3;
            int stopRou = 9;
            int startPen = 5;

            if (unit > 0)
            {
                startRou = unit;
                stopRou = unit;
                startPen = unit;
            }
            for (int i = startRou; i <= stopRou; i++)
            {
                res.Add(CalcRou(i, minDate, maxDate));
            }
            for (int i = startPen; i <= stopRou; i++)
            {
                res.Add(CalcKenMill(i, minDate, maxDate));
                res.Add(CalcPen(i, minDate, maxDate));
            }

            return Ok(String.Join(';', res));
        }
        private string CalcKenMill(int unit, string dateMin, string dateMax)
        {
            if (unit == 3 || unit == 4) return String.Empty;
            DateTime date_min = DateTime.Parse(dateMin, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime date_max = DateTime.Parse(dateMax, null, System.Globalization.DateTimeStyles.RoundtripKind);
           
            var signals = db.Signals.Where(w => w.Unit == unit);
            try
            {
                int id_mw = signals.FirstOrDefault(f => f.AddInfo == list_info[9]).ID;
                for (DateTime item = date_min; item < date_max; item = item.AddDays(1))
                {
                    JObject json = new JObject();
                    Device? device = db.Devices.FirstOrDefault(f => f.Datetime.Equals(item.Date) && f.Unit==unit);
                    if (device is null)
                    {
                        device = new Device() { Datetime = item.Date, Unit=unit };
                    }
                    else
                        json = device.JsonData;

                    List<HistValue> dataOfDay = Tables.GetTable(db, unit, item.Date, item.Date.AddDays(1));// data.Where(w => w.Date.Date == item.Date).ToList();

                    var idkA = signals.FirstOrDefault(f => f.AddInfo == list_info[0]).ID.ToString();
                    var idkB = signals.FirstOrDefault(f => f.AddInfo == list_info[1]).ID.ToString();
                    var idkV = signals.FirstOrDefault(f => f.AddInfo == list_info[2]).ID.ToString();

                    var dataOne = dataOfDay.Where(
                        w => (w.JsonData[idkA].ToObject<float>() > 10
                    && w.JsonData[idkB].ToObject<float>() < 10
                    && w.JsonData[idkV].ToObject<float>() < 10)
                    || (w.JsonData[idkA].ToObject<float>() < 10
                    && w.JsonData[idkB].ToObject<float>() > 10
                    && w.JsonData[idkV].ToObject<float>() < 10)
                    || (w.JsonData[idkA].ToObject<float>() < 10
                    && w.JsonData[idkB].ToObject<float>() < 10
                    && w.JsonData[idkV].ToObject<float>() > 10)
                    );

                    var TimeOne = Math.Round(dataOne.Count() / 60.0, 1);
                    json["ken" + "One" + unit] = TimeOne;
                    var denom = 0.0;
                    var num = 0.0;
                    ////////////////////KEN////////////////////////////////
                    foreach (Signal signal in signals.Where(w => w.AddInfo.Contains("ken")))
                    {
                        //ток КЭН>10 и Мощность>100
                        var data = dataOfDay.Where(w => w.JsonData[signal.ID.ToString()].ToObject<float>() > 10 &&
                        w.JsonData[id_mw.ToString()].ToObject<float>() > 100).Select(w => w.JsonData[signal.ID.ToString()].ToObject<float>());
                        var Time = Math.Round(data.Count() / 60.0, 1);
                        if (Time > 0)
                        {
                            var Ampr = Math.Round(data.Average(), 1);
                            json[signal.AddInfo + "Amp" + unit] = Ampr;
                            json[signal.AddInfo + "Time" + unit] = Time;
                            denom += Time;
                            num += Ampr * Time;
                        }
                        else
                        {
                            json[signal.AddInfo + "Amp" + unit] = 0;
                            json[signal.AddInfo + "Time" + unit] = 0;
                        }


                    }
                    var Avg = Math.Round((num / denom), 1);
                    json["ken" + "Avg" + unit] = Avg;

                    //pair.Add("eff" + (b), Math.Round(Convert.ToDouble(pair["avg" + (b)]) * Math.Sqrt(3.0) * 6300 * 0.85 * Convert.ToDouble(pair["onetime" + b]) / 1000.0, 1));
                    var Eff = Math.Round(Avg * Math.Sqrt(3.0) * 6300 * 0.85 * TimeOne / 1000.0, 1);
                    json["ken" + "Eff" + unit] = Eff;

                    /////////////////////////////ШБМ//////////////////////////////////////////
                    var shbmDT = 0.0;
                    var shbmEf = 0.0;
                    foreach (Signal signal in signals.Where(w => w.AddInfo.Contains("shbm")))
                    {
                        //ток ШБМ>0,5
                        var data = dataOfDay.Where(w => w.JsonData[signal.ID.ToString()].ToObject<float>() > 0.5
                        && w.JsonData[id_mw.ToString()].ToObject<float>() > 5)
                            .Select(w => w.JsonData[signal.ID.ToString()].ToObject<float>());
                        var Time = Math.Round(data.Count() / 60.0, 1);
                        var shbmAmpr = 0.0;
                        if (Time > 0)
                        {
                            shbmAmpr = Math.Round(data.Average(), 1);
                            json[signal.AddInfo + "Amp" + unit] = shbmAmpr;
                            json[signal.AddInfo + "Time" + unit] = Time;
                        }
                        else
                        {
                            json[signal.AddInfo + "Amp" + unit] = 0;
                            json[signal.AddInfo + "Time" + unit] = 0;
                        }

                        data = dataOfDay.Where(w => w.JsonData[signal.ID.ToString()].ToObject<float>() < 0.5
                        && w.JsonData[id_mw.ToString()].ToObject<float>() > 5)
                            .Select(w => w.JsonData[signal.ID.ToString()].ToObject<float>());
                        shbmDT += Math.Round(data.Count() / 60.0, 1);
                        shbmEf += shbmDT * Math.Round(data.Average(), 1);
                    }
                    json["shbmDT" + unit] = shbmDT;
                    json["shbmEf" + unit] = shbmEf * 1000;
                    device.Data = JsonConvert.SerializeObject(json);
                    db.Devices.Add(device);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return "Блок " + unit + ":КЭН " + ex.Message;
            }
            //if (dict_histDatas.Count > 0)
            //{
            //    ken.Amperage = (float)Math.Round(dict_histDatas.Average(s => s.Value[signal.ID]), 1);
            //    ken.Time = (float)Math.Round(dict_histDatas.Count / 60.0, 1);
            //    log.Add("ID=" + ken.ID + "; Amper=" + ken.Amperage + "; TIme=" + ken.Time);
            //    log.Add("new kens=" + new_kens.Count);


            //    JObject jsonObject = new JObject();
            //    jsonObject[signal.ID.ToString()] = new JArray() { ken.Amperage, ken.Time };




            //    jObject.Merge(jsonObject, mergeSettings);
            //}



            //Console.WriteLine(unit.ToString() + signal.ID + ": " + item.ToString());
            //foreach (var item_data in dataOfDay)
            //{
            //    if (item_data.Data == "{}")
            //    {
            //        log.Add(item_data.Date + ": " + unit);
            //        continue;
            //    }
            //    else
            //    {
            //        try
            //        {
            //            //ток КЭН>10 и Мощность>100
            //            if (item_data.JsonData[signal.ID.ToString()].ToObject<float>() > 10 && item_data.JsonData[id_mw.ToString()].ToObject<float>() > 100)
            //            {
            //                dict_histDatas[item_data.Date] = item_data.JsonData.ToObject<IDictionary<int, float>>().ToDictionary(k => k.Key, v => v.Value);
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //            log.Add(ex.Message);
            //        }

            //    }
            //}


            // log.Add(signal.ID + ": histDatas=" + dict_histDatas.Count);



            //if (dict_histDatas.Count > 0)
            //{
            //    ken.Amperage = (float)Math.Round(dict_histDatas.Average(s => s.Value[signal.ID]), 1);
            //    ken.Time = (float)Math.Round(dict_histDatas.Count / 60.0, 1);
            //    log.Add("ID=" + ken.ID + "; Amper=" + ken.Amperage + "; TIme=" + ken.Time);
            //    log.Add("new kens=" + new_kens.Count);


            //    JObject jsonObject = new JObject();
            //    jsonObject[signal.ID.ToString()] = new JArray() { ken.Amperage, ken.Time };




            //    jObject.Merge(jsonObject, mergeSettings);
            //}


            //if (signal.AddInfo.Contains("shbm"))
            //{
            //    Console.WriteLine(unit.ToString() + signal.ID + ": " + item.ToString());
            //    foreach (var item_dict_histDatas in dataOfDay)
            //    {
            //        if (item_dict_histDatas.Data == "{}")
            //        {
            //            log.Add(item_dict_histDatas.Date + ":shbm " + unit);
            //            continue;
            //        }
            //        else
            //        {
            //            try
            //            {

            //                if (item_dict_histDatas.JsonData[id_mw.ToString()].ToObject<float>() > 5)
            //                {
            //                    dict_histDatas[item_dict_histDatas.Date] = item_dict_histDatas.JsonData.ToObject<IDictionary<int, float>>().ToDictionary(k => k.Key, v => v.Value);
            //                }
            //            }
            //            catch (Exception ex)
            //            {

            //                log.Add(ex.Message);
            //            }

            //        }
            //    }
            //    // histDatas = data.Select(s => new { date = s.Date, val = JsonConvert.DeserializeObject<Dictionary<int, float>>(s.Data) }).Where(w => w.val[id_mw] > 5).ToDictionary(k => k.date, h => h.val);
            //    if (dict_histDatas.Count > 0)
            //    {
            //        if (dict_histDatas.Max(m => m.Value[signal.ID]) > 0.5)
            //        {

            //            ken.Amperage = (float)Math.Round(dict_histDatas.Where(w => w.Value[signal.ID] > 0.5).Average(m => m.Value[signal.ID]), 1);
            //            ken.Time = (float)Math.Round(dict_histDatas.Where(w => w.Value[signal.ID] > 0.5).Count() / 60.0, 1);
            //            log.Add("ID=" + ken.ID + "; Amper=" + ken.Amperage + "; TIme=" + ken.Time);
            //            log.Add("new kens=" + new_kens.Count);
            //        }
            //        else
            //        {
            //            ken.Amperage = 0;
            //            ken.Time = 0;
            //        }
            //    }

            //    ken.Downtime = (float)Math.Round(dict_histDatas.Where(w => w.Value[signal.ID] < 0.5).Count() / 60.0, 1);
            //    //log.Add(ken.Datetime.ToString());
            //    jObject.Merge(
            //          new JObject
            //             {

            //                   { signal.ID.ToString(), new JArray{ken.Amperage, ken.Time, ken.Downtime } }


            //               }, mergeSettings);

            //}

            //if (dict_histDatas.Count == 0)
            //{
            //    ken.Amperage = 0;
            //    ken.Downtime = 0;
            //    ken.Time = 0;
            //}
            //}
            //);


            //    Ken _ken = kens.FirstOrDefault(f => f.Datetime.Equals(item.Date) && f.SignalID == -unit);
            //    if (_ken == null)
            //    {
            //        _ken = new Ken() { Datetime = item.Date, SignalID = -unit };
            //        new_kens.Add(_ken);
            //    }
            //    int oneTime = 0;
            //    Console.WriteLine(unit.ToString() + "Oneken: " + item.ToString());
            //    foreach (HistValue histData in dataOfDay)
            //    {
            //        try
            //        {
            //            Dictionary<int, float> values = JsonConvert.DeserializeObject<Dictionary<int, float>>(histData.Data);

            //            if (((values[idkA] > 10.0 && values[idkB] < 10 && values[idkV] < 10) || (values[idkA] < 10.0 && values[idkB] > 10 && values[idkV] < 10) || (values[idkA] < 10.0 && values[idkB] < 10 && values[idkV] > 10)) && values[id_mw] > 100.0)
            //            {
            //                oneTime++;
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //            log.Add(ex.Message);
            //        }


            //    }
            //    _ken.Time = (float)Math.Round(oneTime / 60.0, 1);

            //    jObject.Merge(new JObject {

            //            { "oneKen"+unit , _ken.Time }

            //            },
            //        mergeSettings);


            //    if (!string.IsNullOrEmpty(jKen.Data))
            //    {
            //        JObject jObject_data = JObject.Parse(jKen.Data);
            //        jObject_data.Merge(jObject, mergeSettings);
            //        jKen.Data = jObject_data.ToString(Formatting.None);
            //    }
            //    else
            //    {
            //        jKen.Data = jObject.ToString(Formatting.None);
            //    }
            //    db.SaveChanges();
            //}

            //log.AddRange(new_kens.Select(s => s.SignalID.ToString()).ToList());
            //log.Add("new kens=" + new_kens.Count);


            //return Json(log);
            return "Блок " + unit + ": расчет КЭН, ШБМ завершен.";
        }
        private string CalcPen(int unit, string dateMin, string dateMax)
        {
            if (unit == 3 || unit == 4) return String.Empty;
            DateTime start = DateTime.Parse(dateMin, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime stop = DateTime.Parse(dateMax, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var signals = db.Signals.Where(u => u.Unit == unit);
            List<string> ids_pen = new List<string> { "flowPenA,currentPenA,12,13", "flowPenB,currentPenB,22,23" };

            for (DateTime date = start; date < stop; date = date.AddDays(1))
            {
                DateTime nextDay = date.AddDays(1);

                var device = db.Devices.FirstOrDefault(f => f.Datetime == date && f.Unit==unit);
                var json = new JObject();
                if (device == null)
                {
                    device = new Device() { Datetime = date };
                }
                else
                {
                    json = device.JsonData;
                }
                List<HistValue> data = Tables.GetTable(db, unit, date, nextDay);
                foreach (string str_id in ids_pen)
                {
                    string[] str = str_id.Split(',');
                    Signal flow_pen = signals.FirstOrDefault(f => f.AddInfo == str[0]);
                    Signal curr_pen = signals.FirstOrDefault(f => f.AddInfo == str[1]);
                    Signal NO = signals.FirstOrDefault(f => f.AddInfo == str[2]);
                    Signal NC = signals.FirstOrDefault(f => f.AddInfo == str[3]);
                    try
                    {
                        List<float> flow = new List<float>();
                        //foreach (HistValue histValue in data)
                        //{
                        //    if (histValue.JsonData[id_curr_pen.ToString()].ToObject<float>() > 10)
                        //    {
                        //        curr.Add(histValue.JsonData[id_curr_pen.ToString()].ToObject<float>());

                        //        if (histValue.JsonData[id_NO].ToObject<int>() == 1 && histValue.JsonData[id_NC].ToObject<int>() == 0)
                        //        {
                        //            flow.Add(histValue.JsonData[id_flow_pen.ToString()].ToObject<float>());
                        //        }
                        //        else
                        //        {
                        //            flow.Add((float)(histValue.JsonData[id_flow_pen.ToString()].ToObject<float>() + 170.0));
                        //        }
                        //    }
                        //}

                        var dataCurr = data.Where(w => w.JsonData[curr_pen.ID.ToString()].ToObject<float>() > 10.0)
                            .Select(s => new
                            {
                                Curr = s.JsonData[curr_pen.ID.ToString()].ToObject<float>(),
                                NO = s.JsonData[NO.ID.ToString()].ToObject<int>(),
                                NC = s.JsonData[NC.ID.ToString()].ToObject<int>(),
                                Flow = s.JsonData[flow_pen.ID.ToString()].ToObject<float>()
                            });

                        foreach (var item in dataCurr)
                        {
                            if (item.NO == 1 && item.NC == 0)
                            {
                                flow.Add(item.Flow);
                            }
                            else
                            {
                                flow.Add(item.Flow + 170);
                            }
                        }

                        //if (curr.Count() == 0)
                        //{
                        //    ken.Amperage = 0;
                        //    ken.Time = 0;
                        //    ken.Downtime = 0;
                        //    db.SaveChanges();
                        //    continue;
                        //}

                        var Ampr = Math.Round(dataCurr.Average(a => a.Curr), 1);
                        var Flow = (float)Math.Round(flow.Average(), 1);
                        var Time = (float)Math.Round(dataCurr.Count() / 60.0, 1);
                        json[curr_pen.AddInfo[^4..] + "Ampr" + unit] = Ampr;
                        json[flow_pen.AddInfo[^4..] + "Flow" + unit] = Flow;
                        json[curr_pen.AddInfo[^4..] + "Time" + unit] = Time;

                        //ken.Amperage = (float)Math.Round(histDatas.Average(s => s.Value[id_curr_pen]), 1);
                        //ken.Downtime = (float)Math.Round(histDatas.Average(s => s.Value[id_flow_pen]), 1);
                        //ken.Time = (float)Math.Round(histDatas.Count / 60.0, 1);

                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                device.Data = JsonConvert.SerializeObject(json);
                db.Devices.Add(device);
                db.SaveChanges();

            }


            //Pen pen = db.Pens.FirstOrDefault(f => f.Datetime == firstDayOfMonth);

            //if (pen == null)
            //{
            //    pen = new Pen() { Datetime = firstDayOfMonth };
            //    db.Pens.Add(pen);
            //}


            foreach (string str_id in ids_pen)
            {
                string[] str = str_id.Split(',');
                Signal curr_pen = signals.FirstOrDefault(f => f.AddInfo == str[1] && f.Unit == unit);
                var name = curr_pen.AddInfo[^4..];
                var data = db.Devices.Where(w => w.Datetime.Year == start.Year && w.Datetime.Month == start.Month && w.Unit==unit)
                    .Select(s => new
                    {

                        Flow = s.JsonData[name + unit.ToString() + "Flow"].ToObject<float>() *
                        s.JsonData[name + unit.ToString() + "Time"].ToObject<float>(),
                        Time = s.JsonData[name + unit.ToString() + "Time"].ToObject<float>()
                    }
                    );
                var time = data.Sum(s => s.Time);
                var flow = 0.0;
                if (time > 0)
                {
                    var sum = Convert.ToDouble(data.Sum(s => s.Flow) / time);
                    flow = Math.Round(sum, 4);
                }

                var p = db.Devices.FirstOrDefault(f => f.Datetime.Year == start.Year
                && f.Datetime.Month == start.Month
                && f.Datetime.Day == 1
                && f.Unit == unit);

                if (p is not null)
                {
                    var json = p.JsonData;

                    json[name + "FlowCons" + unit] = flow;
                    json[name + "TimeCons" + unit] = time*5;
                }

                //List<Ken> listOnePen = db.Kens.Where(w => w.Datetime >= firstDayOfMonth && w.Datetime < firstDayNextMonth && w.SignalID == id_curr_pen).ToList();
                //float time = listOnePen.Sum(s => s.Time);
                //float flow = 0;
                //foreach (var itemOnePen in listOnePen)
                //{
                //    flow += itemOnePen.Downtime * itemOnePen.Time;
                //}
                //if (time > 0)
                //{
                //    flow = (float)Math.Round(flow / time, 4);
                //}


                //switch (unit)
                //{
                //    case 3:
                //        if (str_id[7] == 'A')
                //        {
                //            pen.Flow3A = flow;
                //            pen.Time3A = time;
                //        }
                //        else
                //        {
                //            pen.Flow3B = flow;
                //            pen.Time3B = time;
                //        }
                //        break;
                //    case 4:
                //        if (str_id[7] == 'A')
                //        {
                //            pen.Flow4A = flow;
                //            pen.Time4A = time;
                //        }
                //        else
                //        {
                //            pen.Flow4B = flow;
                //            pen.Time4B = time;
                //        }
                //        break;
                //    case 5:
                //        if (str_id[7] == 'A')
                //        {
                //            pen.Flow5A = flow;
                //            pen.Time5A = time;
                //        }
                //        else
                //        {
                //            pen.Flow5B = flow;
                //            pen.Time5B = time;
                //        }
                //        break;
                //    case 6:
                //        if (str_id[7] == 'A')
                //        {
                //            pen.Flow6A = flow;
                //            pen.Time6A = time;
                //        }
                //        else
                //        {
                //            pen.Flow6B = flow;
                //            pen.Time6B = time;
                //        }
                //        break;
                //    case 7:
                //        if (str_id[7] == 'A')
                //        {
                //            pen.Flow7A = flow;
                //            pen.Time7A = time;
                //        }
                //        else
                //        {
                //            pen.Flow7B = flow;
                //            pen.Time7B = time;
                //        }
                //        break;
                //    case 8:
                //        if (str_id[7] == 'A')
                //        {
                //            pen.Flow8A = flow;
                //            pen.Time8A = time;
                //        }
                //        else
                //        {
                //            pen.Flow8B = flow;
                //            pen.Time8B = time;
                //        }
                //        break;
                //    case 9:
                //        if (str_id[7] == 'A')
                //        {
                //            pen.Flow9A = flow;
                //            pen.Time9A = time;
                //        }
                //        else
                //        {
                //            pen.Flow9B = flow;
                //            pen.Time9B = time;
                //        }
                //        break;
                //    default:
                //        break;
                //}

            }
            db.SaveChanges();




            return "Блок " + unit + ": расчет ПЭН завершен.";
        }
        private string CalcRou(int unit, string dateMin, string dateMax)
        {
            try
            {
                DateTime low_condition = DateTime.Parse(dateMin, null, System.Globalization.DateTimeStyles.RoundtripKind);
                DateTime high_condition = DateTime.Parse(dateMax, null, System.Globalization.DateTimeStyles.RoundtripKind);

                List<Signal> signals = db.Signals.Where(w => w.AddInfo == "rou24" || w.AddInfo == "rou140" || w.AddInfo == "generator").ToList();
                List<string> Errors = new List<string>();

                ///////////////////////////////24/13////////////////////////////////////////////////


                for (DateTime d = low_condition; d < high_condition; d = d.AddDays(1))
                {
                    DateTime nextDay = d.AddDays(1);
                    // var data = db.HistValues.Where(f => f.Date >= d && f.Date < nextDay).OrderBy(o => o.Date).ToList();
                    Rou rou = db.Rous.FirstOrDefault(f => f.Datetime == d);
                    if (rou == null)
                    {
                        rou = new Rou() { Datetime = d };
                        db.Rous.Add(rou);
                    }


                    List<HistValue> data = Tables.GetTable(db, unit, d, nextDay);
                    if (data.Count == 0)
                    {
                        return "Блок " + unit + ": нет данных" + d.ToString("s");
                    }

                    List<float> newrou = new List<float>();
                    float avRou = 0;
                    string id_gen = "";
                    if (unit >= 5 && unit <= 9)
                    {


                        id_gen = signals.FirstOrDefault(f => f.Unit == unit && f.AddInfo == "generator").ID.ToString();
                        string id = signals.FirstOrDefault(f => f.Unit == unit && f.AddInfo == "rou24").ID.ToString();
                        List<float> iEnumAvRou24 = data.Where(w => w.JsonData[id_gen].ToObject<int>() > 0).Select(s => s.JsonData[id].ToObject<float>()).ToList();

                        foreach (float itemrou in iEnumAvRou24)
                        {
                            if (itemrou >= 0)
                            {
                                newrou.Add(itemrou);
                            }
                            else
                            {
                                newrou.Add(0);
                            }
                        }
                        avRou = 0;
                        if (iEnumAvRou24.Count > 0)
                        {
                            avRou = (float)Math.Round(newrou.Average(), 2);
                        }
                        switch (unit)
                        {
                            case 3: rou.Unit3 = avRou; break;
                            case 4: rou.Unit4 = avRou; break;
                            case 5: rou.Unit5 = avRou; break;
                            case 6: rou.Unit6 = avRou; break;
                            case 7: rou.Unit7 = avRou; break;
                            case 8: rou.Unit8 = avRou; break;
                            case 9: rou.Unit9 = avRou; break;
                            default: break;
                        }
                    }
                    if (unit == 3 || unit == 7 || unit == 9)
                    {
                        ///////////////////////////140/15//////////////////////////////////////
                        string id140 = signals.FirstOrDefault(f => f.Unit == unit && f.AddInfo == "rou140").ID.ToString();
                        List<float> iEnumAvRou140 = new List<float>();
                        if (unit == 3)
                        {
                            iEnumAvRou140 = data.Select(s => s.JsonData[id140].ToObject<float>()).ToList();
                        }
                        else
                        {
                            iEnumAvRou140 = data.Where(w => w.JsonData[id_gen].ToObject<int>() > 0).Select(s => s.JsonData[id140].ToObject<float>()).ToList();
                        }


                        newrou.Clear();
                        foreach (float itemrou in iEnumAvRou140)
                        {
                            if (itemrou >= 0)
                            {
                                newrou.Add(itemrou);
                            }
                            else
                            {
                                newrou.Add(0);
                            }
                        }

                        if (iEnumAvRou140.Count > 0)
                        {
                            avRou = (float)Math.Round(newrou.Average(), 2);
                        }
                        switch (unit)
                        {
                            case 3: rou.Unit3Big = avRou; break;
                            case 7: rou.Unit7Big = avRou; break;
                            case 9: rou.Unit9Big = avRou; break;
                            default: break;
                        }
                    }

                    db.SaveChanges();
                }



                ///////////////////////////140/15//////////////////////////////////////
                ///
            }
            catch (Exception ex)
            {

                return "Блок " + unit + ":РОУ " + ex.Message;
            }
            return "Блок " + unit + ": расчет РОУ завершен.";
        }
        [HttpGet("CreateROURow")]
        public ActionResult CreateROURow()
        {
            DateTime now = DateTime.Now;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            Rou rou = db.Rous.FirstOrDefault(f => f.Datetime == d);
            if (rou == null)
            {
                rou = new Rou() { Datetime = d };
                db.Rous.Add(rou);
                db.SaveChanges();
            }

            return Ok();
        }

        [HttpGet("Metall")]
        public ActionResult CalcMetall(int unit, string dateMin, string dateMax)
        {
            List<string> res = new List<string>();
            res.Add("Блок " + unit.ToString());
            DateTime minDate = DateTime.Parse(dateMin, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime maxDate = DateTime.Parse(dateMax, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var signals = db.Signals.Where(w => w.AddInfo.Contains("metall"));
            List<HistValue> data = Tables.GetTable(db, unit, minDate, maxDate);
            foreach (var signal in signals)
            {
                if (signal.AddInfo.Contains("VD") || signal.AddInfo.Contains("ND"))
                {
                    foreach (var item in data)
                    {
                        if (item.JsonData[signal.ID.ToString()].ToObject<float>() > 565)
                        {
                            res.Add(String.Join(';', item.Date.ToString("g"), signal.Desc, item.JsonData[signal.ID.ToString()].ToObject<float>()));
                        }
                    }

                }
                if (signal.AddInfo.Contains("SH"))
                {
                    foreach (var item in data)
                    {
                        if (item.JsonData[signal.ID.ToString()].ToObject<float>() > 535)
                        {
                            res.Add(String.Join(';', item.Date.ToString("g"), signal.Desc, item.JsonData[signal.ID.ToString()].ToObject<float>()));
                        }
                    }
                }
            }
            var contentType = "text/xml";
            var content = "<content>Your content</content>";
            var bytes = Encoding.UTF8.GetBytes(content);
            var result = new FileContentResult(bytes, contentType);
            result.FileDownloadName = "Metall" + unit.ToString() + ".xml";
            return result;
        }


        [HttpGet("Edit")]
        public IActionResult GetEditStages(int unit, string date)
        {
            if (unit == 0 || unit == 3 || unit == 4)
            {
                return Ok("");
            }

            DateTime minDate = DateTime.Parse(date).Date;
            DateTime maxDate = minDate.AddHours(24);

            var histValues = Tables.GetTable(db, unit, minDate, maxDate);
            if (histValues.Count == 0) return Ok("");
            var signals = db.Signals.Where(w => w.Unit == unit);

            string id_gen = signals.FirstOrDefault(f => f.Code.Contains("BR")).ID.ToString();
            string id_dva = signals.FirstOrDefault(f => f.Code.Contains("HLB10")).ID.ToString();
            string id_dvb = signals.FirstOrDefault(f => f.Code.Contains("HLB20")).ID.ToString();
            string id_speed = signals.FirstOrDefault(f => f.Code.Contains("SPI")).ID.ToString();
            string id_press = signals.FirstOrDefault(f => f.Code.Contains("CP001")).ID.ToString();
            string id_stage = signals.FirstOrDefault(f => f.Code.Contains("Stage")).ID.ToString();

            var f = histValues.Select(s => new
            {
                Id = s.ID,
                Date = s.Date.ToString("s"),
                Generator = s.JsonData[id_gen]!.Value<int>(),
                Speed = s.JsonData[id_speed].Value<int>(),
                Dva = s.JsonData[id_dva].Value<int>(),
                Dvb = s.JsonData[id_dvb].Value<int>(),
                Press = s.JsonData[id_press].ToString(),
                Stage = s.JsonData[id_stage].Value<int>()
            });
            return Ok(f);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStagesEdit()
        {
            string body = "";
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            var ds = JObject.Parse(body);

            int id = ds["id"].Value<int>();
            string values = ds["values"].ToString();
            int unit = ds["unit"].Value<int>();
            string date = ds["date"].ToString();

            DateTime d = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.RoundtripKind);
            HistValue histValue = Tables.GetTable(db, unit, d, d.AddDays(1)).FirstOrDefault(f => f.ID == id);
            string id_stage = db.Signals.FirstOrDefault(f => f.Unit == unit & f.Code.Contains("Stage")).ID.ToString();
            //var v = histValue.Data.Replace("{", "").Replace("}", "").Split(',').Select(s => s.Split(':')).ToDictionary(sp => sp[0], sp => sp[1]);
            var deser_values = JsonConvert.DeserializeObject<JObject>(values);
            int number_stage = deser_values["stage"].ToObject<int>();

            JObject j = histValue.JsonData;
            j[id_stage] = number_stage;
            histValue.Data = JsonConvert.SerializeObject(j);

            Tables.SaveTable(db, unit, new List<HistValue>() { histValue });

            Stage? stage = db.Stages.FirstOrDefault(f => f.Date == histValue.Date && f.Unit == unit);
            if (stage == null && number_stage > 0)
            {
                stage = new Stage();
                stage.Unit = unit;
                stage.Date = histValue.Date;
                stage.Number = number_stage;
                db.Stages.Add(stage);
            }

            if (stage != null)
            {
                if (number_stage == 0)
                {
                    db.Stages.Remove(stage);
                }
                if (number_stage > 0)
                {
                    stage.Number = number_stage;
                }
            }
            db.SaveChanges();


            return Ok();
        }

        [HttpGet("CheckData")]
        public IActionResult CheckData(int unit, string date)
        {

            List<Error> response = new List<Error>();

            DateTime dateTime = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.RoundtripKind);

            DateTime now = DateTime.Now;
            now = now.Add(-now.TimeOfDay).AddSeconds(-1);
            int start = unit;
            int stop = unit;
            if (unit == 0)
            {
                start = 3;
                stop = 9;
            }
            for (int i = start; i <= stop; i++)
            {

                switch (i)
                {
                    case 3:
                        var histValues = db.Hist3Values.Where(w => w.Date >= dateTime && w.Date <= now).Select(s => new HistValue() { ID = s.ID, Date = s.Date, Data = s.Data });
                        response.AddRange(DataCheckInOneTable(dateTime, now, i, histValues));
                        break;
                    case 4:
                        histValues = db.Hist4Values.Where(w => w.Date >= dateTime && w.Date <= now).Select(s => new HistValue() { ID = s.ID, Date = s.Date, Data = s.Data });
                        response.AddRange(DataCheckInOneTable(dateTime, now, i, histValues));
                        break;
                    case 5:
                        histValues = db.Hist5Values.Where(w => w.Date >= dateTime && w.Date <= now).Select(s => new HistValue() { ID = s.ID, Date = s.Date, Data = s.Data });
                        response.AddRange(DataCheckInOneTable(dateTime, now, i, histValues));
                        break;
                    case 6:
                        histValues = db.Hist6Values.Where(w => w.Date >= dateTime && w.Date <= now).Select(s => new HistValue() { ID = s.ID, Date = s.Date, Data = s.Data });
                        response.AddRange(DataCheckInOneTable(dateTime, now, i, histValues));
                        break;
                    case 7:
                        histValues = db.Hist7Values.Where(w => w.Date >= dateTime && w.Date <= now).Select(s => new HistValue() { ID = s.ID, Date = s.Date, Data = s.Data });
                        response.AddRange(DataCheckInOneTable(dateTime, now, i, histValues));
                        break;
                    case 8:
                        histValues = db.Hist8Values.Where(w => w.Date >= dateTime && w.Date <= now).Select(s => new HistValue() { ID = s.ID, Date = s.Date, Data = s.Data });
                        response.AddRange(DataCheckInOneTable(dateTime, now, i, histValues));
                        break;
                    case 9:
                        histValues = db.Hist9Values.Where(w => w.Date >= dateTime && w.Date <= now).Select(s => new HistValue() { ID = s.ID, Date = s.Date, Data = s.Data });
                        response.AddRange(DataCheckInOneTable(dateTime, now, i, histValues));
                        break;
                }



            }


            List<Error> DataCheckInOneTable(DateTime dateTime, DateTime now, int unit, IQueryable<HistValue> histValues)
            {
                List<Error> response = new List<Error>();
                List<Signal> signals = db.Signals.Where(w => w.Unit == unit).ToList();
                int count = signals.Count;
                for (DateTime i = dateTime; i <= now; i = i.AddMinutes(1))
                {
                    Error error = new Error() { Date = i, Unit = unit.ToString() };
                    var row = histValues.FirstOrDefault(f => f.Date == i);
                    if (row == null)
                    {
                        response.Add(error);
                    }
                    else
                    {
                        if (row.JsonData.Count != count)
                        {
                            foreach (Signal signal in signals)
                            {
                                if (!row.JsonData.ContainsKey(signal.ID.ToString()))
                                {
                                    error.Signals.Add(signal.Unit.ToString() + "_" + signal.ID.ToString());
                                }
                            }
                            response.Add(error);
                        }
                    }
                }

                return response;
            }

            return Ok(response.GroupBy(g => g.Date).Select(s => String.Join('-', s.Key, "Блоки: " + String.Join(',', s.Select(u => u.Unit)) +
                ";")));
        }

        [HttpGet("ReturnStage")]
        public IActionResult ReturnStage()
        {
            var list = db.Stages.ToList();
            db.Stages.RemoveRange(db.Stages);
            foreach (var stage in list)
            {
                if (stage.Stage1 != null)
                {
                    db.Stages.Add(new Stage() { Date = stage.Stage1.Value, Unit = stage.Unit, Number = 1 });
                }
                if (stage.Stage2 != null)
                {
                    db.Stages.Add(new Stage() { Date = stage.Stage2.Value, Unit = stage.Unit, Number = 2 });
                }
                if (stage.Stage3 != null)
                {
                    db.Stages.Add(new Stage() { Date = stage.Stage3.Value, Unit = stage.Unit, Number = 3 });
                }
                if (stage.Stage4 != null)
                {
                    db.Stages.Add(new Stage() { Date = stage.Stage4.Value, Unit = stage.Unit, Number = 4 });
                }
                if (stage.Stage5 != null)
                {
                    db.Stages.Add(new Stage() { Date = stage.Stage5.Value, Unit = stage.Unit, Number = 5 });
                }

            }
            db.SaveChanges();

            return Ok();
        }

        private class Res
        {
            public int unit { get; set; }
            public string dateMin { get; set; }
            public string dateMax { get; set; }

        }
        private class Error
        {
            public DateTime Date { get; set; }
            public List<string> Signals { get; set; } = new List<string>();

            public string Unit { get; set; }
        }

        private class EditStage
        {
            public int Id { get; set; }
            public string Date { get; set; }
            public int Generator { get; set; }
            public int Speed { get; set; }
            public int Dva { get; set; }
            public int Dvb { get; set; }
            public string? Press { get; set; }
            public int Stage { get; set; }
        }

    }


}


////public void SortTable()
////{

////    Dictionary<int, int> signals = db.Signals.ToDictionary(d => d.ID, f => f.Unit);
////    List<HistValue> items = new List<HistValue>();
////    for (DateTime i = new DateTime(2020, 10, 22, 00, 00, 0); i < new DateTime(2021, 11, 01, 0, 0, 0); i = i.AddDays(1))
////    {

////        ///!!!Warning!!!!!
////        //List<HistValue> items = new List<HistValue>();
////        items = db.HistValues.Where(w => w.Date >= i.Date && w.Date < i.Date.AddDays(1)).OrderBy(o => o.Date).ToList();
////        foreach (HistValue item in items)
////        {


////            //if (item == null)
////            //{
////            //    string str = i.ToString();
////            //    db.Logs.Add(new Log(str + " No data!"));
////            //    db.Hist3Values.Add(new Hist3Value() { Date = i });
////            //    db.Hist4Values.Add(new Hist4Value() { Date = i });
////            //    db.Hist5Values.Add(new Hist5Value() { Date = i });
////            //    db.Hist6Values.Add(new Hist6Value() { Date = i });
////            //    db.Hist7Values.Add(new Hist7Value() { Date = i });
////            //    db.Hist8Values.Add(new Hist8Value() { Date = i });
////            //    db.Hist9Values.Add(new Hist9Value() { Date = i });
////            //   // db.SaveChanges();
////            //    continue;
////            //}
////            JObject j3 = new JObject();
////            JObject j4 = new JObject();
////            JObject j5 = new JObject();
////            JObject j6 = new JObject();
////            JObject j7 = new JObject();
////            JObject j8 = new JObject();
////            JObject j9 = new JObject();
////            foreach (JToken jitem in item.JsonData.Children())
////            {
////                int p = int.Parse(jitem.Path);
////                int u = signals[p];
////                //Signal signal = signals.FirstOrDefault(f => f.ID == int.Parse(jitem.Path));
////                switch (u)
////                {
////                    case 3:
////                        j3.Add(jitem);
////                        break;
////                    case 4:
////                        j4.Add(jitem);
////                        break;
////                    case 5:
////                        j5.Add(jitem);
////                        break;
////                    case 6:
////                        j6.Add(jitem);
////                        break;
////                    case 7:
////                        j7.Add(jitem);
////                        break;
////                    case 8:
////                        j8.Add(jitem);
////                        break;

////                    case 9:
////                        j9.Add(jitem);
////                        break;

////                    default:
////                        break;
////                }
////            }

////            if (j3.Count > 0)
////            {
////                db.Hist3Values.Add(new Hist3Value() { Date = item.Date, Data = j3.ToString(Formatting.None) });
////            }
////            if (j4.Count > 0)
////            {
////                db.Hist4Values.Add(new Hist4Value() { Date = item.Date, Data = j4.ToString(Formatting.None) });
////            }
////            if (j5.Count > 0)
////            {
////                db.Hist5Values.Add(new Hist5Value() { Date = item.Date, Data = j5.ToString(Formatting.None) });
////            }
////            if (j6.Count > 0)
////            {
////                db.Hist6Values.Add(new Hist6Value() { Date = item.Date, Data = j6.ToString(Formatting.None) });
////            }
////            if (j7.Count > 0)
////            {
////                db.Hist7Values.Add(new Hist7Value() { Date = item.Date, Data = j7.ToString(Formatting.None) });
////            }
////            if (j8.Count > 0)
////            {
////                db.Hist8Values.Add(new Hist8Value() { Date = item.Date, Data = j8.ToString(Formatting.None) });
////            }
////            if (j9.Count > 0)
////            {
////                db.Hist9Values.Add(new Hist9Value() { Date = item.Date, Data = j9.ToString(Formatting.None) });
////            }

////            if (i.TimeOfDay.Equals(new TimeSpan(0, 0, 0)))
////            {

////            }

////        }
////        db.SaveChanges();
////        items.Clear();

////    }
////    db.SaveChanges();

////}

////public ActionResult ConvertData()
////{
////    SortTable();
////    return new OkResult();
////}



////[HttpGet]
////public IActionResult Index()
////{
////    var user = Request.HttpContext.User.Identity.Name.Split('\\')[1].ToUpper();

////    List<string> users = new List<string>();
////    users.Add("romashkooy");
////    users.Add("KirichekAV");
////    users.Add("LavrentevPS");
////    users.Add("SaprikinAV");
////    users.Add("opc");
////    users.Add("user");
////    foreach (var userList in users)
////    {
////        if (userList.ToUpper() == user.ToUpper())
////            return View();

////    }
////    return Ok("Access denied!");
////}
