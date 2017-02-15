using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace Validate.LogicLayer
{
    public class ValidateClass
    {
        public static List<objErrores> ValidarAchivo(string Archivo, Page page)
        {
            string xPath_2_1 = "//srch:Activities/srch:Activity[not(@Id=//srch:Activities/srch:Activity[@IsForCompensation='true']/@Id)][not(@Id=//srch:Activities/srch:Activity[srch:Event/srch:EndEvent]/@Id)][not(@Id=//srch:Activities/srch:Activity[srch:Event/srch:IntermediateEvent[@Trigger='Compensation']]/@Id)]";
            Validaciones ValBizagi = new Validaciones();
            ValBizagi["1"] = new Validacion() { xPath = "//srch:Activities/srch:Activity[@Name = '']//srch:TriggerResultMessage[@CatchThrow='THROW']/ancestor::srch:Activity", Style = "Style 0115" };
            //ValBizagi["2_1"] = new Validacion() { xPath = "//srch:Activities/srch:Activity[not(@Id=//srch:Activities/srch:Activity[@IsForCompensation='true']/@Id)][not(@Id=//srch:Activities/srch:Activity[srch:Event/srch:EndEvent]/@Id)][not(@Id=//srch:Activities/srch:Activity[srch:Event/srch:IntermediateEvent[@Trigger='Compensation']]/@Id)]", Style = "BPMN 0102" };
            ValBizagi["2_2"] = new Validacion() { xPath = xPath_2_1 + "[not(@Id=//srch:Transition[@From=" + xPath_2_1 + "/@Id]/@From)]", Style = "BPMN 0102" };
            ValBizagi["3"] = new Validacion() { xPath = "//srch:Activity[@Name = following::srch:Activity/@Name]", Style = "Style 0104" };
            ValBizagi["4"] = new Validacion() { xPath = "//*[@CatchThrow='THROW']/ancestor::srch:Activity[@Id!=//srch:Transition[@From=//*[@CatchThrow='THROW']/ancestor::srch:Activity/@Id]/@From]", Style = "Style 0123" };
            ValBizagi["5"] = new Validacion() { xPath = "//*[@Trigger='Message']/srch:TriggerResultMessage[not(@CatchThrow)]/ancestor::srch:Activity[not(@Id=//srch:Transition[@To=//*[@Trigger='Message']/srch:TriggerResultMessage[not(@CatchThrow)]/ancestor::srch:Activity/@Id]/@To)]", Style = "Style 0122" };

            List<objErrores> ListaErrores = new List<objErrores>();
            var doc = new XmlDocument();
            doc.Load(page.Server.MapPath(Archivo));
            var nsmg = new XmlNamespaceManager(doc.NameTable);
            nsmg.AddNamespace("srch", doc.GetElementsByTagName("Package")[0].Attributes["xmlns"].Value);
            foreach (var key in ValBizagi.Keys)
            {
                var nodosInvalidos = doc.SelectNodes(ValBizagi[key].xPath, nsmg);
                for (int i = 0; i < nodosInvalidos.Count; i++)
                {
                    ListaErrores.Add(new objErrores() { id = nodosInvalidos[i].Attributes["Id"].Value, nombre = nodosInvalidos[i].Attributes["Name"].Value, error = ValBizagi[key].Style });
                }
            }
            return ListaErrores;
        }


    }

    public class Archivos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string FullName { get; set; }
    }
    public class Validaciones : Dictionary<string, Validacion>
    {
        public Validacion this[string key]
        {
            get
            {
                return base[key];
            }
            set
            {
                if (ContainsKey(key)) this[key] = value;
                else Add(key, value);
            }
        }
    }

    public class Validacion
    {
        public string xPath { get; set; }
        public string Style { get; set; }
    }



    public class objErrores
    {
        public string nombre { get; set; }
        public string id { get; set; }
        public string error { get; set; }
    }
}