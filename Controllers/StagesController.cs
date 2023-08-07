using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using WebApi.Models;

namespace WeoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StagesController : ControllerBase
    {
        private readonly StartsContext db;

        public StagesController(StartsContext context)
        {
            db = context;
        }

        // GET: api/GetStages
        [HttpGet]
        public ActionResult GetStages(string unit, string year = "0", string month = "0")
        {
            List<ViewStage> viewStages = new List<ViewStage>();
            if (unit == "0") return Ok(viewStages);
            if (month == "0") month = DateTime.Now.Month.ToString();
            if (year == "0") year = DateTime.Now.Year.ToString();
            int.TryParse(unit, out int u);
            int.TryParse(month, out int m);
            int.TryParse(year, out int y);
            var date = new DateTime(y, m, 1);
            var dateAddMonth = date.AddMonths(1);
            var stages = db.Stages.Where(q => q.Date >= date & q.Date < dateAddMonth & q.Unit == u & q.Number >= 4).OrderBy(o => o.Date);

            

            ViewStage viewStage =null;

            foreach (Stage item in stages)
            {
                if (item.Number == 4)
                {
                    viewStage = new ViewStage() { id = ":" };
                    viewStage.stage4 = item.Date.ToString("s");
                    viewStage.id = item.ID.ToString() + viewStage.id;
                    
                }
                if (item.Number == 5)
                {
                    if (viewStage != null)
                    {
                        viewStage.stage5 = item.Date.ToString("s");
                        viewStage.id = viewStage.id + item.ID.ToString();
                        viewStage = null;
                    }
                    else
                    {
                        viewStage = new ViewStage() { id = ":" };
                        viewStage.stage5 = item.Date.ToString("s");
                        viewStage.id = viewStage.id + item.ID.ToString();
                        viewStages.Add(viewStage);
                    }
                }
                else
                {
                    viewStages.Add(viewStage);
                }
                
            }


            return Ok(viewStages);
        }

        // GET: api/StagesLast

        [HttpGet("Last")]
        public ActionResult GetStagesLast()
        {
            //var s = db.Stages.Where(w => w.Number == 4 || w.Number == 5).GroupBy(g => g.Unit).Select(s => s.OrderByDescending(o => o.Date).FirstOrDefault()).ToList().OrderBy(b => b.Unit).ToList();
            List<Stage> s = new List<Stage>();
            for (int i = 3; i <= 9; i++)
            {
                var v = db.Stages.OrderBy(o => o.Date).LastOrDefault(l => l.Unit == i & (l.Number == 4 | l.Number == 5));
                s.Add(v);
            }


            List<Dictionary<string, string>> stages = new List<Dictionary<string, string>>();

            foreach (Stage item in s)
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();


                pairs.Add("badge", item.badge);
                pairs.Add("icon", item.icon);
                pairs.Add("text", item.text);
                pairs.Add("id", item.Unit.ToString());
                stages.Add(pairs);
            }

            return Ok(stages);
            //.Skip(Math.Max(0,db.Stages.Count()-1)));
        }
        //// GET api/Stages/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Stage>> GetStage(int id)
        //{
        //    var stage = await db.Stages.FindAsync(id);
        //    if (stage == null)
        //    {
        //        return NotFound();
        //    }
        //    return stage;
        //}


        public class ViewStage
        {
            
            public string unit { get; set; }
            public string id { get; set; }
            public string stage4 { get; set; }
            public string stage5 { get; set; }
        }
        // POST api/<StagesController>
        [HttpPost]
        public ActionResult PostStage([FromBody] ViewStage viewStage)
        {
            try
            {
                int unit = int.Parse(viewStage.unit);
                if (!String.IsNullOrEmpty(viewStage.stage4))
                {
                    Stage stage = new Stage() { Unit = unit, Date = DateTime.Parse(viewStage.stage4, null, System.Globalization.DateTimeStyles.RoundtripKind), Number = 4 };
                    db.Stages.Add(stage);
                }

                if (!String.IsNullOrEmpty(viewStage.stage5))
                {
                    Stage stage = new Stage() { Unit = unit, Date = DateTime.Parse(viewStage.stage5, null, System.Globalization.DateTimeStyles.RoundtripKind), Number = 5 };
                    db.Stages.Add(stage);
                }


                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }


        }


        // PUT api/PutStage/5
        [HttpPut]
        public IActionResult PutStage()
        {

            try
            {
                var req = HttpContext.Request.Query.ToList();
                List<Stage> stages = new List<Stage>();
                int id = int.Parse(req.FirstOrDefault(f => f.Key == "id").Value);
                foreach (var item in req)
                {
                    if (item.Key == "values[stage4]" | item.Key == "values[stage4]")
                    {
                        Stage? stage = db.Stages.FirstOrDefault(f => f.ID == id);
                        if (stage is not null)
                        {
                            stage.Date = DateTime.Parse(item.Value, null, System.Globalization.DateTimeStyles.RoundtripKind);
                        }
                    }
                }
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }



            return NoContent();
        }

        // DELETE api/DeleteStage/5
        [HttpDelete]
        public IActionResult DeleteStage(string id)
        {
            bool changes = false;
            try
            {
                var arr = id.Split(':');
                foreach (var s in arr)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        var arrId = int.Parse(s);
                        Stage? stage = db.Stages.FirstOrDefault(f => f.ID == arrId);
                        if (stage is not null)
                        {
                            db.Stages.Remove(stage);
                        }

                        changes = true;
                    }
                }

                if (changes)
                {
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

    }
}
