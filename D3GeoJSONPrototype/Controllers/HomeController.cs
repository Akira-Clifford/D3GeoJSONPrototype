using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D3GeoJSONPrototype.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MapData()
        {
            var path = System.Web.HttpContext.Current.Request.MapPath("~/");
            using (System.IO.StreamReader sr = new System.IO.StreamReader(path + "toronto.simple.json"))
            {
                var json = sr.ReadToEnd();
                var results = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(json);

                foreach (var f in results.features)
                {
                    if (f.properties.SCODE_NAME == "44")
                    {
                        f.properties.StatusColor = "#FF0000";
                    }
                    else
                    {
                        f.properties.StatusColor = "#AAA";
                    }
                }

                return Json(results, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MapSubDivisionData(int wardId)
        {
            var results = new List<SubDivisionViewModel>();

            Random rnd = new Random();

            for(int i = 0; i < rnd.Next(1, 60); i++)
            {
                var applicants = rnd.Next(0, 100);
                results.Add(new SubDivisionViewModel
                {
                    Applicants = rnd.Next(0, 100),
                    Hired = rnd.Next(0, 10),
                    Interviewed = rnd.Next(0, applicants),
                    Predictive = rnd.Next(0, 20),
                    StatusClass = this.GetStatus(rnd.Next(3)),
                    SubDivisionId = i,
                    WardId = wardId,
                });
            }    

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        private string GetStatus(int i)
        {
            string status = string.Empty;

            switch (i)
            {
                case 0:
                    status = "danger";
                    break;
                case 1:
                    status = "warning";
                    break;
                default:
                    status = "success";
                    break;
            }

            return status;
        }
    }

    public class Rootobject
    {
        public float[] bbox { get; set; }
        public string type { get; set; }
        public Feature[] features { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Properties
    {
        public string SHAPE_AREA { get; set; }
        public string NAME { get; set; }
        public int OBJECTID { get; set; }
        public string TYPE_DESC { get; set; }
        public string SHAPE_LEN { get; set; }
        public int GEO_ID { get; set; }
        public string SCODE_NAME { get; set; }
        public string LCODE_NAME { get; set; }
        public string TYPE_CODE { get; set; }
        public int CREATE_ID { get; set; }
        public string StatusColor { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public float[][][] coordinates { get; set; }
    }
    
    public class SubDivisionViewModel
    {
        public int WardId { get; set; }
        public int SubDivisionId { get; set; }
        public int Applicants { get; set; }
        public int Interviewed { get; set; }
        public int Hired { get; set; }
        public int Predictive { get; set; }
        public string StatusClass { get; set; }
    }
}