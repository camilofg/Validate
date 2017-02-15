using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Validate.LogicLayer;

namespace Validate
{
    public partial class Validacion1 : System.Web.UI.Page
    {
        List<Archivos> ListaArchivos;

        protected void Page_Load(object sender, EventArgs e)
        {
            /*var Archivos = new[] { 
                new { Id = 1, Nombre = "XMLs/sample 1.xpdl" }
                , new { Id = 2, Nombre = "XMLs/sample 2/sample 20.xpdl" }
                , new { Id = 3, Nombre = "XMLs/sample 3.xpdl" } 
                , new { Id = 4, Nombre = "XMLs/sample 41.xpdl" } 
                , new { Id = 5, Nombre = "XMLs/sample 42.xpdl" } 
            };*/
            CargarListaArchivos();

            PaintFiles();
        }

        private void PaintFiles()
        {
            PnlResult.Controls.Clear();
            Table TabResult = new Table();
            TableRow FilaHeader = new TableRow();
            TableCell celdaID = new TableCell();
            celdaID.Text = "ID";
            FilaHeader.Cells.Add(celdaID);


            TableCell celdaNombre = new TableCell();
            celdaNombre.Text = "Nombre";

            FilaHeader.Cells.Add(celdaNombre);
            TabResult.Rows.Add(FilaHeader);
            for (int i = 0; i < ListaArchivos.Count; i++)
            {
                TableRow filaBodyPivot = new TableRow();

                TableCell celdaPivotID = new TableCell();
                celdaPivotID.Text = ListaArchivos[i].Id.ToString();
                filaBodyPivot.Cells.Add(celdaPivotID);

                TableCell celdaPivotName = new TableCell();
                celdaPivotName.Text = ListaArchivos[i].Nombre.ToString();
                celdaPivotName.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                celdaPivotName.Style.Add(HtmlTextWriterStyle.Color, "blue");
                celdaPivotName.ID = "Archivo_" + i.ToString();
                filaBodyPivot.Cells.Add(celdaPivotName);

                TabResult.Rows.Add(filaBodyPivot);
            }
            PnlResult.Controls.Add(TabResult);
        }

       
        public void CargarListaArchivos()
        {
            DirectoryInfo oDirectorio = new DirectoryInfo(Server.MapPath("XMLs/"));
            ListaArchivos = new List<Archivos>();
            int i = 1;
            foreach (FileInfo file in oDirectorio.GetFiles("*.xpdl"))
            {
                ListaArchivos.Add(new Archivos() { Nombre = file.Name, FullName = file.FullName, Id = i });
                i++;
            }
        }

        protected void Vincular_Click(object sender, EventArgs e)
        {
            if (UpFile.HasFile)
            {
                string fileName = Path.GetFileNameWithoutExtension(UpFile.FileName) + Guid.NewGuid();
                if (Path.GetExtension(UpFile.FileName).ToLower() != ".xpdl")
                {
                    return;
                }
                fileName += ".xpdl";
                DirectoryInfo dir = new DirectoryInfo("XMLs/");
                var ubcArchivo = Server.MapPath(dir + fileName);
                if (!dir.Exists)
                    dir.Create();

                if (System.IO.File.Exists(ubcArchivo))
                    System.IO.File.Delete(ubcArchivo);

                UpFile.SaveAs(ubcArchivo);
                CargarListaArchivos();
                PaintFiles();
            }
        }
    }

   

    
}