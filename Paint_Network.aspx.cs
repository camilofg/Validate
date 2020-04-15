using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Validate.LogicLayer;

namespace Validate
{
    public partial class Paint_Network : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HdnOds.Value = Request.QueryString["Ods"];
        }

        [WebMethod]
        public static object RetrieveXml(string ods)
        {
            var wrapper = new WrapperObject {ListOds = ListOds(), ListRelations = ListRelations(ods)};
            return wrapper;
        }

        private static List<Relacion> ListRelations(string ods)
        {
            using (StreamReader r = new StreamReader(@"C: \Users\camus\source\repos\Validate\XMLs\relations_v3.json"))
            {
                string json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<RootObject_0>(json).relaciones.ToList();
                if (ods != "")
                    items = items.Where(x => x.origen.ToString() == ods).ToList();
                return items;
            }
        }

        private static RootObject ListOds()
        {
            using (StreamReader r = new StreamReader(@"C: \Users\camus\source\repos\Validate\XMLs\ods_2.json"))
            {
                string json = r.ReadToEnd();
                RootObject items = JsonConvert.DeserializeObject<RootObject>(json);
                return items;
            }
        }
    }
}