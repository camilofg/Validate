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
           
        }

        [WebMethod]
        public static object RetrieveXml()
        {
            var wrapper = new WrapperObject {ListOds = ListOds(), ListRelations = ListRelations()};
            return wrapper;
        }

        private static RootObject_0 ListRelations()
        {
            using (StreamReader r = new StreamReader(@"C: \Users\camus\source\repos\Validate\XMLs\relations_.json"))
            {
                string json = r.ReadToEnd();
                RootObject_0 items = JsonConvert.DeserializeObject<RootObject_0>(json);
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