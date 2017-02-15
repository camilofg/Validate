using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Validate.LogicLayer
{
    public class Serializer
    {
        public static string JSerializerDictionary(object parameters)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            StringBuilder sBuilder = new StringBuilder();
            jserializer.Serialize(parameters, sBuilder);
            return sBuilder.ToString();
        }
        //Permite deserializar un Dictionary en donde Keys es de tipo string, relativo a los campos y Value un object relativo al valor
        public static Dictionary<object, object> JDeserializerDictionary(string parameters)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            Dictionary<object, object> dictionary = jserializer.Deserialize<Dictionary<object, object>>(parameters);
            return dictionary;
        }
    }
}