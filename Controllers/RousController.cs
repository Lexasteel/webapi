using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Globalization;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RousController : ControllerBase
    {
        private readonly StartsContext db;

        public RousController(StartsContext context) {
            db = context;
        }

        [HttpGet]
        public IActionResult Get24(string date) {


            DateTime _date = DateTime.Parse(date, null, DateTimeStyles.RoundtripKind);
            DateTime firstDay = _date;
            DateTime lastDay = firstDay.AddMonths(1);
            var rous = db.Rous.Where(w=>w.Datetime>=firstDay && w.Datetime<lastDay).OrderBy(o=>o.Datetime);
            return Ok(rous);
        }

        //[HttpPost]
        //public async Task<IActionResult> Post(string values) {
        //    var model = new Rou();
        //    var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
        //    PopulateModel(model, valuesDict);

        //    if(!TryValidateModel(model))
        //        return BadRequest(GetFullErrorMessage(ModelState));

        //    var result = db.Rous.Add(model);
        //    await db.SaveChangesAsync();

        //    return Ok(result.Entity.Datetime);
        //}

        [HttpPut]
        public async Task<IActionResult> UpdateRou() 
        {

            string body = "";
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            var deser = JObject.Parse(body);

            int id = deser["id"].Value<int>();
            string values = deser["values"].ToString();

            var model = await db.Rous.FirstOrDefaultAsync(item => item.Id == id);
            if(model == null)
                return StatusCode(409, "Rou not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await db.SaveChangesAsync();
            return Ok();
        }

        //[HttpDelete]
        //public async Task Delete(DateTime key) {
        //    var model = await db.Rous.FirstOrDefaultAsync(item => item.Datetime == key);

        //    db.Rous.Remove(model);
        //    await db.SaveChangesAsync();
        //}


        private void PopulateModel(Rou model, IDictionary values) {
            //string DATE = nameof(Rou.Datetime).ToLower();
            //string UNIT3 = nameof(Rou.Unit3).ToLower();
            //string UNIT3BIG = nameof(Rou.Unit3Big).ToLower();
            //string UNIT4 = nameof(Rou.Unit4).ToLower();
            //string UNIT5 = nameof(Rou.Unit5).ToLower();
            //string UNIT6 = nameof(Rou.Unit6).ToLower();
            //string UNIT7 = nameof(Rou.Unit7).ToLower();
            //string UNIT7BIG = nameof(Rou.Unit7Big).ToLower();
            //string UNIT8 = nameof(Rou.Unit8).ToLower();
            //string UNIT9 = nameof(Rou.Unit9).ToLower();
            //string UNIT9BIG = nameof(Rou.Unit9Big).ToLower();
            string KPD = nameof(Rou.Kpd).ToLower();
            string OUTPUT = nameof(Rou.Output).ToLower();
            string OUTPUT3 = nameof(Rou.Output3).ToLower();
            string OUTPUT7 = nameof(Rou.Output7).ToLower();
            string OUTPUT9 = nameof(Rou.Output9).ToLower();


            //if (values.Contains(DATE)) {
            //    model.Datetime = Convert.ToDateTime(values[DATE]);
            //}

            //if(values.Contains(UNIT3)) {
            //    model.Unit3 = Convert.ToSingle(values[UNIT3], CultureInfo.InvariantCulture);
            //}

            //if(values.Contains(UNIT3BIG)) {
            //    model.Unit3Big = Convert.ToSingle(values[UNIT3BIG], CultureInfo.InvariantCulture);
            //}

            //if(values.Contains(UNIT4)) {
            //    model.Unit4 = Convert.ToSingle(values[UNIT4], CultureInfo.InvariantCulture);
            //}

            //if(values.Contains(UNIT5)) {
            //    model.Unit5 = Convert.ToSingle(values[UNIT5], CultureInfo.InvariantCulture);
            //}

            //if(values.Contains(UNIT6)) {
            //    model.Unit6 = Convert.ToSingle(values[UNIT6], CultureInfo.InvariantCulture);
            //}

            //if(values.Contains(UNIT7)) {
            //    model.Unit7 = Convert.ToSingle(values[UNIT7], CultureInfo.InvariantCulture);
            //}

            //if(values.Contains(UNIT7BIG)) {
            //    model.Unit7Big = Convert.ToSingle(values[UNIT7BIG], CultureInfo.InvariantCulture);
            //}

            //if(values.Contains(UNIT8)) {
            //    model.Unit8 = Convert.ToSingle(values[UNIT8], CultureInfo.InvariantCulture);
            //}

            //if(values.Contains(UNIT9)) {
            //    model.Unit9 = Convert.ToSingle(values[UNIT9], CultureInfo.InvariantCulture);
            //}

            //if(values.Contains(UNIT9BIG)) {
            //    model.Unit9Big = Convert.ToSingle(values[UNIT9BIG], CultureInfo.InvariantCulture);
            //}

            if(values.Contains(KPD)) {
                model.Kpd = Convert.ToSingle(values[KPD], CultureInfo.InvariantCulture);
            }

            if(values.Contains(OUTPUT)) {
                model.Output = Convert.ToInt32(values[OUTPUT]);
            }
            if (values.Contains(OUTPUT3))
            {
                model.Output3 = Convert.ToInt32(values[OUTPUT3]);
            }
            if (values.Contains(OUTPUT7))
            {
                model.Output7 = Convert.ToInt32(values[OUTPUT7]);
            }
            if (values.Contains(OUTPUT9))
            {
                model.Output9 = Convert.ToInt32(values[OUTPUT9]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}