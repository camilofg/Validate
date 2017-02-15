using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Validate.LogicLayer;

namespace Validate
{
    public partial class Paint_Model : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HdnXmlRoute.Value = "XMLs/" + Request.QueryString["UbcArchivo"];
            var ListaErrores = ValidateClass.ValidarAchivo(HdnXmlRoute.Value, this);
            var Diccionario = new Dictionary<string, string>();
            foreach (objErrores error in ListaErrores)
            {
                Diccionario.Add(error.id + "|" + error.error + "|" + error.nombre, error.error);
            }
            HdnErrores.Value = Serializer.JSerializerDictionary(Diccionario);
        }


        [WebMethod]
        public static object ConsultNodeGraphics(string xmlVarRoute, string errores)
        {
            var ListaErrores = Serializer.JDeserializerDictionary(errores);
            XmlDocument doc = new XmlDocument();
            Page page = new Paint_Model();
            doc.Load(page.Server.MapPath(xmlVarRoute));
            var nod = doc.GetElementsByTagName("Package");
            string ns = nod[0].Attributes["xmlns"].Value;
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("srch", ns);


            var GraficoCompleto = new CompleteGraphic();
            var listaProcesos = new List<GenericObj>();
            var listaNodosAPintar = new List<GraphicNodes>();
            var listaTransicionesAPintar = new List<TransitionNodes>();
            var listaMensajes = new List<MessagesFlows>();

            var MarcosAPintar = doc.SelectNodes("//srch:Pool", nsmgr);
            foreach (XmlNode nodo in MarcosAPintar)
            {
                if (nodo.Attributes["Name"].Value == "Main Process") { }
                else
                {
                    var Marco = new GenericObj();
                    Marco.Id = nodo.Attributes["Id"].Value;
                    Marco.Name = nodo.Attributes["Name"].Value;
                    Marco.Width = (float.Parse(nodo.SelectSingleNode(".//srch:NodeGraphicsInfo", nsmgr).Attributes["Width"].Value)).ToString();
                    Marco.Height = (float.Parse(nodo.SelectSingleNode(".//srch:NodeGraphicsInfo", nsmgr).Attributes["Height"].Value)).ToString();

                    Marco.xPosition = Math.Floor((float.Parse(nodo.SelectSingleNode(".//srch:Coordinates", nsmgr).Attributes["XCoordinate"].Value))).ToString();
                    Marco.yPosition = Math.Floor((float.Parse(nodo.SelectSingleNode(".//srch:Coordinates", nsmgr).Attributes["YCoordinate"].Value))).ToString();

                    listaProcesos.Add(Marco);
                }
            }
            GraficoCompleto.WorkProcess = listaProcesos;

            var NodosPintar = doc.SelectNodes("//srch:Activity", nsmgr);
            foreach (XmlNode nodo in NodosPintar)
            {
                var nodoGrafico = new GraphicNodes();
                nodoGrafico.Id = nodo.Attributes["Id"].Value;
                nodoGrafico.Name = nodo.Attributes["Name"].Value;
                nodoGrafico.Type = nodo.SelectSingleNode(".//srch:Event", nsmgr) != null ? "Circ" : "Rect";
                if (nodoGrafico.Type == "Circ" && (nodo.SelectNodes(".//srch:IntermediateEvent[@Trigger='Message']", nsmgr).Count > 0))
                    nodoGrafico.EventType = (nodo.SelectNodes(".//srch:TriggerResultMessage[@CatchThrow='THROW']", nsmgr) == null) || (nodo.SelectNodes(".//srch:TriggerResultMessage[@CatchThrow='THROW']", nsmgr).Count == 0) ? "Catch" : "Throw";
                else if (nodoGrafico.Type == "Circ")
                {
                    nodoGrafico.EventType = (nodo.SelectNodes(".//srch:StartEvent", nsmgr).Count == 0) ? "EndEvent" : "StartEvent";
                }
                nodoGrafico.Width = (float.Parse(nodo.SelectSingleNode(".//srch:NodeGraphicsInfo", nsmgr).Attributes["Width"].Value)).ToString();
                nodoGrafico.Height = (float.Parse(nodo.SelectSingleNode(".//srch:NodeGraphicsInfo", nsmgr).Attributes["Height"].Value)).ToString();

                nodoGrafico.xPosition = Math.Floor((float.Parse(nodo.SelectSingleNode(".//srch:Coordinates", nsmgr).Attributes["XCoordinate"].Value))).ToString();
                nodoGrafico.yPosition = nodoGrafico.Type == "Circ" ? Math.Floor((float.Parse(nodo.SelectSingleNode(".//srch:Coordinates", nsmgr).Attributes["YCoordinate"].Value)) + float.Parse(nodoGrafico.Width) / 2).ToString() : Math.Floor((float.Parse(nodo.SelectSingleNode(".//srch:Coordinates", nsmgr).Attributes["YCoordinate"].Value))).ToString();
                var ActivityFilterError =
                    (from item in ListaErrores
                    where (item.Key).ToString().Split('|')[0] == nodoGrafico.Id.ToString()
                    select item).ToList();
                string erroresNodo = string.Empty;
                string validacionNombre = string.Empty;
                if (ActivityFilterError.Count > 0) {
                    for (int i = 0; i < ActivityFilterError.Count; i++) {
                        var erroresSplit = ActivityFilterError[i].Key.ToString().Split('|');
                        validacionNombre += erroresSplit[1] + ",";
                        if (i == ActivityFilterError.Count - 1) {
                            erroresNodo = erroresSplit[0] + "|" + validacionNombre + "|" + erroresSplit[2];
                        }
                    }
                     
                }
                nodoGrafico.error = ActivityFilterError.Count > 0 ? erroresNodo : null;
                listaNodosAPintar.Add(nodoGrafico);
            }
            GraficoCompleto.ActivitiesToPaint = listaNodosAPintar;


            var TransicionesPintar = doc.SelectNodes("//srch:Transition", nsmgr);
            foreach (XmlNode nodo in TransicionesPintar)
            {
                var transicionesGrafico = new TransitionNodes();
                transicionesGrafico.Id = nodo.Attributes["Id"].Value;

                var from = nodo.Attributes["From"].Value;
                var ActivityFilterFrom =
                    GraficoCompleto.ActivitiesToPaint.Where(item => item.Id == from).ToList();
                if (ActivityFilterFrom[0].Type == "Circ")
                {
                    transicionesGrafico.FromX = (float.Parse(ActivityFilterFrom[0].xPosition) + (float.Parse(ActivityFilterFrom[0].Width) / 2)).ToString();
                    transicionesGrafico.FromY = (float.Parse(ActivityFilterFrom[0].yPosition) /*+ float.Parse(ActivityFilterFrom[0].Height) / 2*/).ToString();
                }
                else
                {
                    transicionesGrafico.FromX = (float.Parse(ActivityFilterFrom[0].xPosition) + (float.Parse(ActivityFilterFrom[0].Width))).ToString();
                    transicionesGrafico.FromY = (float.Parse(ActivityFilterFrom[0].yPosition) + float.Parse(ActivityFilterFrom[0].Height) / 2).ToString();

                }

                var to = nodo.Attributes["To"].Value;
                var ActivityFilterTo =
                    GraficoCompleto.ActivitiesToPaint.Where(item => item.Id == to).ToList();
                if (ActivityFilterTo[0].Type == "Rect")
                {
                    transicionesGrafico.ToX = (float.Parse(ActivityFilterTo[0].xPosition)).ToString();
                    transicionesGrafico.ToY = (float.Parse(ActivityFilterTo[0].yPosition) + (float.Parse(ActivityFilterTo[0].Height) / 2)).ToString();
                }
                else
                {
                    transicionesGrafico.ToX = (float.Parse(ActivityFilterTo[0].xPosition) - (float.Parse(ActivityFilterTo[0].Width) / 2)).ToString();
                    transicionesGrafico.ToY = (float.Parse(ActivityFilterTo[0].yPosition)).ToString();
                }
                listaTransicionesAPintar.Add(transicionesGrafico);
            }
            GraficoCompleto.TransitionsToPaint = listaTransicionesAPintar;

            var MensajesPintar = doc.SelectNodes("//srch:MessageFlow", nsmgr);
            foreach (XmlNode nodo in MensajesPintar)
            {
                var mensajes = new MessagesFlows();
                mensajes.Id = nodo.Attributes["Id"].Value;
                mensajes.Name = nodo.Attributes["Name"].Value;
                mensajes.Source = nodo.Attributes["Source"].Value;
                mensajes.Target = nodo.Attributes["Target"].Value;
                var nodosCoordenadas = nodo.SelectSingleNode(".//srch:ConnectorGraphicsInfo", nsmgr);
                var ActivityFilterFrom =
                    GraficoCompleto.ActivitiesToPaint.Where(item => item.Id == nodosCoordenadas.SelectNodes(".//srch:Coordinates/ancestor::srch:MessageFlow", nsmgr)[0].Attributes["Source"].Value).ToList();
                mensajes.FromX = (float.Parse(nodosCoordenadas.SelectNodes(".//srch:Coordinates", nsmgr)[0].Attributes["XCoordinate"].Value) - float.Parse(ActivityFilterFrom[0].Width) / 2).ToString();
                mensajes.FromY = nodosCoordenadas.SelectNodes(".//srch:Coordinates", nsmgr)[0].Attributes["YCoordinate"].Value;

                var ActivityFilterTo =
                    GraficoCompleto.ActivitiesToPaint.Where(item => item.Id == nodosCoordenadas.SelectNodes(".//srch:Coordinates/ancestor::srch:MessageFlow", nsmgr)[0].Attributes["Target"].Value).ToList();
                mensajes.ToX = (float.Parse(nodosCoordenadas.SelectNodes(".//srch:Coordinates", nsmgr)[1].Attributes["XCoordinate"].Value) - float.Parse(ActivityFilterFrom[0].Width) / 2).ToString();
                mensajes.ToY = nodosCoordenadas.SelectNodes(".//srch:Coordinates", nsmgr)[1].Attributes["YCoordinate"].Value;
                listaMensajes.Add(mensajes);
            }
            GraficoCompleto.Messages = listaMensajes;

            return GraficoCompleto;
            
            //return listaNodosAPintar;
        }

    }
}