using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebApi.Models;
using System.Security.Cryptography.Xml;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly StartsContext _context;

        public DevicesController(StartsContext context)
        {
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<string>> GetDevices(string date)
        {
            if (_context.Devices == null)
            {
                return NotFound();
            }
            DateTime start = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime stop = start.AddMonths(1);
            var d = _context.Devices.Where(w => w.Datetime >= start && w.Datetime < stop).GroupBy(g => g.Datetime);
            List<JObject> res = new List<JObject>();
            foreach (var item in d)
            {
                var j = new JObject();
                foreach (var item2 in item)
                {
                    j.Merge(item2.JsonData);
                }
                res.Add(j);
            }
            var s = JsonConvert.SerializeObject(res, Formatting.Indented);

            return s;
        }

        // GET: api/Devices/Pens
        [HttpGet("Pens")]
        public async Task<ActionResult<string>> GetDevicesPens()
        {
            if (_context.Devices == null)
            {
                return NotFound();
            }
            var d = _context.Devices.Where(w => w.Datetime.Day == 1).GroupBy(g => g.Datetime);
            List<JObject> res = new List<JObject>();
            foreach (var item in d)
            {
                var j = new JObject();
                foreach (var item2 in item)
                {
                    j.Merge(item2.JsonData);
                }
                res.Add(j);
            }
            var s = JsonConvert.SerializeObject(res, Formatting.Indented);

            return s;
        }




        // GET: api/Devices/5
        [HttpGet("{id}")]
        private async Task<ActionResult<Device>> GetDevice(int id)
        {
            if (_context.Devices == null)
            {
                return NotFound();
            }
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        [HttpPut("Pens")]
        public async Task<IActionResult> Update()
        {
            string body = "";
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            var jObject = JObject.Parse(body);
            DateTime date = jObject["id"]!.Value<DateTime>();
            var key = JObject.Parse(jObject["values"]!.ToString());
            var unit = int.Parse(key.First.Path[^1..]);
            var device = _context.Devices.FirstOrDefault(f => f.Datetime == date && f.Unit == unit);
            if (device != null)
            {
                var jData = device.JsonData;
                jData.Merge(key);

                device.Data = JsonConvert.SerializeObject(jData);
                _context.SaveChanges();
            }


            return Ok();
        }


        // PUT: api/Devices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        private async Task<IActionResult> PutDevice(int id, Device device)
        {
            if (id != device.ID)
            {
                return BadRequest();
            }

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Devices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        private async Task<ActionResult<Device>> PostDevice(Device device)
        {
            if (_context.Devices == null)
            {
                return Problem("Entity set 'StartsContext.Devices'  is null.");
            }
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevice", new { id = device.ID }, device);
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        private async Task<IActionResult> DeleteDevice(int id)
        {
            if (_context.Devices == null)
            {
                return NotFound();
            }
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceExists(int id)
        {
            return (_context.Devices?.Any(e => e.ID == id)).GetValueOrDefault();
        }


        [HttpGet("recalc")]
        public IActionResult Recalc()
        {
            for (int i = 0; i < 5; i++)
            {
                if (_context.Signals.FirstOrDefault(f => f.ID == i + 1) is null)
                {
                    Signal signal = new Signal() { ID = i + 1, Unit = i + 5 };
                    _context.Signals.Add(signal);
                }
            }
            if (_context.ChangeTracker.HasChanges())
            {
                _context.SaveChanges();
            }
            List<Signal> bl = _context.Signals.Where(w => w.ID < 6).ToList();



            var date = new DateTime(2019, 07, 16);
            var kens = _context.Kens.Where(w => w.Datetime > date).Include(i => i.Signal).GroupBy(g => g.Datetime).ToList();
            var signals = _context.Signals.Where(w => w.AddInfo != "").ToList();

            List<string> result = new List<string>();
            foreach (var ken in kens)
            {
                foreach (var item in ken)
                {
                    if (item.SignalID < 0)
                    {
                        item.Signal = bl.FirstOrDefault(f => f.Unit == item.SignalID.Value * (-1));
                    }


                }


                foreach (var s in ken.GroupBy(g => g.Signal.Unit))
                {


                    JObject json = new JObject();
                    Device? d = _context.Devices.FirstOrDefault(f => f.Datetime == ken.Key && f.Unit == s.Key);
                    if (d is null)
                    {
                        d = new Device() { Datetime = ken.Key, Unit = s.Key };
                        _context.Devices.Add(d);
                    }
                    else
                    {
                        json = d.JsonData;
                    }
                    double? denom = 0.0;
                    double? num = 0.0;
                    double shbmDownTime = 0.0;
                    double shbmEff = 0;
                    foreach (var item in s)
                    {

                        if (item.SignalID < 0)
                        {
                            json["ken" + "One" + s.Key] = Math.Round(item.Time.Value, 1);
                            continue;
                        }


                        var signal = signals.FirstOrDefault(f => f.ID == item.SignalID);
                        if (signal is null) continue;


                        if (signal.AddInfo.Contains("ken"))
                        {
                            json[signal.AddInfo + "Ampr" + signal.Unit] = Math.Round(item.Amperage.Value, 1);
                            json[signal.AddInfo + "Time" + signal.Unit] = Math.Round(item.Time.Value, 1);
                            denom += item.Time;
                            num += item.Amperage * item.Time;
                            continue;
                        }
                        if (signal.AddInfo.Contains("shbm"))
                        {
                            json[signal.AddInfo + "Ampr" + signal.Unit] = Math.Round(item.Amperage.Value, 1);
                            json[signal.AddInfo + "Time" + signal.Unit] = Math.Round(item.Time.Value, 1);
                            shbmDownTime += Math.Round(item.Downtime.Value, 1);
                            shbmEff += item.Downtime.Value * item.Amperage.Value;
                            continue;
                        }


                        if (signal.AddInfo.Contains("currentPen"))
                        {
                            json[signal.AddInfo[^4..] + "Ampr" + signal.Unit] = Math.Round(item.Amperage.Value, 1);
                            json[signal.AddInfo[^4..] + "Flow" + signal.Unit] = Math.Round(item.Downtime.Value, 1);
                            json[signal.AddInfo[^4..] + "Time" + signal.Unit] = Math.Round(item.Time.Value, 1);
                            continue;
                        }
                        //  result.Add(item.SignalID.ToString());
                        //json[name + "FlowMonth"] = flow;
                        //json[name + "TimeMonth"] = time;

                    }
                    if (denom == 0.0) denom = 1.0;
                    var Avg = Math.Round((num.Value / denom.Value), 1);
                    json["ken" + "Avg" + s.Key] = Avg;
                    if (json["kenOne" + s.Key] is null)
                    {
                        json["kenOne" + s.Key] = 0;
                    }
                    var kenEff = Math.Round(Avg * Math.Sqrt(3.0) * 6300 * 0.85 * json["kenOne" + s.Key].ToObject<double>() / 1000.0, 1);
                    json["ken" + "Eff" + s.Key] = kenEff;

                    json["shbmDownTime" + s.Key] = Math.Round(shbmDownTime, 1);
                    json["shbmEff" + s.Key] = shbmEff * 1000;

                    if (ken.Key.Day == 1)
                    {
                        var pen = _context.Pens.FirstOrDefault(f => f.Datetime == ken.Key);
                        var _pens=_context.Pens;
                        var flowMin = 350;
                        var flowMax = 550;
                        var mnk = new double[2];
                        string plus = " + ";
                        switch (s.Key)
                        {
                            case 5:
                                json["PenA" + "FlowCons" + 5] = Math.Round(pen.Flow5A, 1);
                                json["PenA" + "TimeCons" + 5] = Math.Round(pen.Time5A, 1);
                                json["PenA" + "PowrCons" + 5] = pen.Powr5A;
                                
                                mnk = MNK(_pens.Where(w => w.Powr5A > 0).ToList()
                                    .Select(s => new Matrix() { x = s.Flow5A, y = s.Powr5A / s.Time5A }));
                                var jArray5 = new JArray();
                                jArray5.Add(new JObject(new JProperty("PenAFlowChart" + 5, flowMin),
                                new JProperty("PenAPowrChart" + 5, mnk[0] * flowMin + mnk[1])));
                                jArray5.Add(new JObject(new JProperty("PenAFlowChart" + 5, flowMax),
                                new JProperty("PenAPowrChart" + 5, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray5.Add(new JObject(new JProperty("name","ПЭН-5А: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));
                                json["PenB" + "FlowCons" + 5] = Math.Round(pen.Flow5B, 1);
                                json["PenB" + "TimeCons" + 5] = Math.Round(pen.Time5B, 1);
                                json["PenB" + "PowrCons" + 5] = pen.Powr5B;
                                mnk = MNK(_pens.Where(w => w.Powr5B > 0).ToList()
                                    .Select(s => new Matrix() { x = s.Flow5B, y = s.Powr5B / s.Time5B }));
                                jArray5.Add(new JObject(new JProperty("PenBFlowChart" + 5, flowMin),
                                new JProperty("PenBPowrChart" + 5, mnk[0] * flowMin + mnk[1])));
                                jArray5.Add(new JObject(new JProperty("PenBFlowChart" + 5, flowMax),
                                new JProperty("PenBPowrChart" + 5, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray5.Add(new JObject(new JProperty("name", "ПЭН-Б: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));
                                json.Add("chart", jArray5);

                                break;
                            case 6:
                                json["PenA" + "FlowCons" + 6] = Math.Round(pen.Flow6A, 1);
                                json["PenA" + "TimeCons" + 6] = Math.Round(pen.Time6A, 1);
                                json["PenA" + "PowrCons" + 6] = pen.Powr6A;
                                mnk = MNK(_pens.Where(w => w.Powr6A > 0).ToList()
                                   .Select(s => new Matrix() { x = s.Flow6A, y = s.Powr6A / s.Time6A }));
                                var jArray6 = new JArray();
                                jArray6.Add(new JObject(new JProperty("PenAFlowChart" + 6, flowMin),
                                new JProperty("PenAPowrChart" + 6, mnk[0] * flowMin + mnk[1])));
                                jArray6.Add(new JObject(new JProperty("PenAFlowChart" + 6, flowMax),
                                new JProperty("PenAPowrChart" + 6, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray6.Add(new JObject(new JProperty("name", "ПЭН-6А: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));

                                json["PenB" + "FlowCons" + 6] = Math.Round(pen.Flow6B, 1);
                                json["PenB" + "TimeCons" + 6] = Math.Round(pen.Time6B, 1);
                                json["PenB" + "PowrCons" + 6] = pen.Powr6B;
                                mnk = MNK(_pens.Where(w => w.Powr6B > 0).ToList()
                                    .Select(s => new Matrix() { x = s.Flow6B, y = s.Powr6B / s.Time6B }));
                                jArray6.Add(new JObject(new JProperty("PenBFlowChart" + 6, flowMin),
                                new JProperty("PenBPowrChart" + 6, mnk[0] * flowMin + mnk[1])));
                                jArray6.Add(new JObject(new JProperty("PenBFlowChart" + 6, flowMax),
                                new JProperty("PenBPowrChart" + 6, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray6.Add(new JObject(new JProperty("name", "ПЭН-6Б: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));
                                json.Add("chart", jArray6);
                                break;
                            case 7:
                                json["PenA" + "FlowCons" + 7] = Math.Round(pen.Flow7A, 1);
                                json["PenA" + "TimeCons" + 7] = Math.Round(pen.Time7A, 1);
                                json["PenA" + "PowrCons" + 7] = pen.Powr7A;
                                mnk = MNK(_pens.Where(w => w.Powr7A > 0).ToList()
                                   .Select(s => new Matrix() { x = s.Flow7A, y = s.Powr7A / s.Time7A }));
                                var jArray7 = new JArray();
                                jArray7.Add(new JObject(new JProperty("PenAFlowChart" + 7, flowMin),
                                new JProperty("PenAPowrChart" + 7, mnk[0] * flowMin + mnk[1])));
                                jArray7.Add(new JObject(new JProperty("PenAFlowChart" + 7, flowMax),
                                new JProperty("PenAPowrChart" + 7, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray7.Add(new JObject(new JProperty("name", "ПЭН-7А: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));

                                json["PenB" + "FlowCons" + 7] = Math.Round(pen.Flow7B, 1);
                                json["PenB" + "TimeCons" + 7] = Math.Round(pen.Time7B, 1);
                                json["PenB" + "PowrCons" + 7] = pen.Powr7B;
                                mnk = MNK(_pens.Where(w => w.Powr7B > 0).ToList()
                                    .Select(s => new Matrix() { x = s.Flow7B, y = s.Powr7B / s.Time7B }));
                                jArray7.Add(new JObject(new JProperty("PenBFlowChart" + 7, flowMin),
                                new JProperty("PenBPowrChart" + 7, mnk[0] * flowMin + mnk[1])));
                                jArray7.Add(new JObject(new JProperty("PenBFlowChart" + 7, flowMax),
                                new JProperty("PenBPowrChart" + 7, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray7.Add(new JObject(new JProperty("name", "ПЭН-7Б: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));
                                json.Add("chart", jArray7);
                                break;
                            case 8:
                                json["PenA" + "FlowCons" + 8] = Math.Round(pen.Flow8A, 1);
                                json["PenA" + "TimeCons" + 8] = Math.Round(pen.Time8A, 1);
                                json["PenA" + "PowrCons" + 8] = pen.Powr8A;
                                mnk = MNK(_pens.Where(w => w.Powr8A > 0).ToList()
                                   .Select(s => new Matrix() { x = s.Flow8A, y = s.Powr8A / s.Time8A }));
                                var jArray8 = new JArray();
                                jArray8.Add(new JObject(new JProperty("PenAFlowChart" + 8, flowMin),
                                new JProperty("PenAPowrChart" + 8, mnk[0] * flowMin + mnk[1])));
                                jArray8.Add(new JObject(new JProperty("PenAFlowChart" + 8, flowMax),
                                new JProperty("PenAPowrChart" + 8, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray8.Add(new JObject(new JProperty("name", "ПЭН-8А: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));

                                json["PenB" + "FlowCons" + 8] = Math.Round(pen.Flow8B, 1);
                                json["PenB" + "TimeCons" + 8] = Math.Round(pen.Time8B, 1);
                                json["PenB" + "PowrCons" + 8] = pen.Powr8B;
                                mnk = MNK(_pens.Where(w => w.Powr8B > 0).ToList()
                                    .Select(s => new Matrix() { x = s.Flow8B, y = s.Powr8B / s.Time8B }));
                                jArray8.Add(new JObject(new JProperty("PenBFlowChart" + 8, flowMin),
                                new JProperty("PenBPowrChart" + 8, mnk[0] * flowMin + mnk[1])));
                                jArray8.Add(new JObject(new JProperty("PenBFlowChart" + 8, flowMax),
                                new JProperty("PenBPowrChart" + 8, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray8.Add(new JObject(new JProperty("name", "ПЭН-8Б: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));
                                json.Add("chart", jArray8);
                                break;
                            case 9:
                                json["PenA" + "FlowCons" + 9] = Math.Round(pen.Flow9A, 1);
                                json["PenA" + "TimeCons" + 9] = Math.Round(pen.Time9A, 1);
                                json["PenA" + "PowrCons" + 9] = pen.Powr9A;
                                mnk = MNK(_pens.Where(w => w.Powr9A > 0).ToList()
                                   .Select(s => new Matrix() { x = s.Flow9A, y = s.Powr9A / s.Time9A }));
                                var jArray9 = new JArray();
                                jArray9.Add(new JObject(new JProperty("PenAFlowChart" + 9, flowMin),
                                new JProperty("PenAPowrChart" + 9, mnk[0] * flowMin + mnk[1])));
                                jArray9.Add(new JObject(new JProperty("PenAFlowChart" + 9, flowMax),
                                new JProperty("PenAPowrChart" + 9, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray9.Add(new JObject(new JProperty("name", "ПЭН-9А: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));

                                json["PenB" + "FlowCons" + 9] = Math.Round(pen.Flow9B, 1);
                                json["PenB" + "TimeCons" + 9] = Math.Round(pen.Time9B, 1);
                                json["PenB" + "PowrCons" + 9] = pen.Powr9B;
                                mnk = MNK(_pens.Where(w => w.Powr9B > 0).ToList()
                                    .Select(s => new Matrix() { x = s.Flow9B, y = s.Powr9B / s.Time9B }));
                                jArray9.Add(new JObject(new JProperty("PenBFlowChart" + 9, flowMin),
                                new JProperty("PenBPowrChart" + 9, mnk[0] * flowMin + mnk[1])));
                                jArray9.Add(new JObject(new JProperty("PenBFlowChart" + 9, flowMax),
                                new JProperty("PenBPowrChart" + 9, mnk[0] * flowMax + mnk[1])));
                                if (mnk[1] < 0) plus = " - ";
                                jArray9.Add(new JObject(new JProperty("name", "ПЭН-9Б: "), new JProperty("line", $"{mnk[0]}*x{plus}{mnk[1]}")));
                                json.Add("chart", jArray9);
                                break;
                            default:
                                break;
                        }

                    }

                    d.Data = JsonConvert.SerializeObject(json, Formatting.Indented);
                    _context.SaveChanges();
                }







            }
            return Ok(kens.Min(m => m.Key).ToString("s") + " - " + kens.Max(m => m.Key).ToString("s"));
        }

        private class Matrix
        {
            public double x { get; set; }
            public double y { get; set; }
            public double xy { get { return x * y; } }
            public double x2 { get { return Math.Pow(x, 2); } }
        }

        private double[] MNK(IEnumerable<Matrix> list)
        {
            ///(y) = ax + b
            ////Наклон(a) = (NΣXY — (ΣX)(ΣY)) / (NΣX2 — (ΣX)2)
            double a = (list.Count() * list.Sum(s => s.xy) - list.Sum(s => s.x) * list.Sum(s => s.y))
                / (list.Count() * list.Sum(s => s.x2) - Math.Pow(list.Sum(s => s.x), 2));
            ///Пересечение (b) = (ΣY — a(ΣX)) / N
            double b = (list.Sum(s => s.y) - a * list.Sum(s => s.x)) / list.Count();
            return new double[] { a, b };
        }
    }
}
