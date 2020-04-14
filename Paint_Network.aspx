<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Paint_Network.aspx.cs" Inherits="Validate.Paint_Network" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="Scripts/json2.js" type="text/javascript"></script>
<script src="Scripts/raphael.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        var paper = new Raphael("ods_div", 1000, 1000);
        //var container = document.getElementById('body');
        //paper = new Raphael(container, config.canvas.width, config.canvas.height);
        var arrayParams = {}
        arrayParams.xmlVarRoute = $("#HdnXmlRoute").val();
        arrayParams.errores = $("#HdnErrores").val();
        arrayParams = JSON.stringify(arrayParams);
        $.ajax({
            type: "POST",
            url: "Paint_Network.aspx/RetrieveXml",
            data: arrayParams,
            contentType: "application/json; chartset=utf-8",
            dataType: "json",
            async: true,
            success: function (result) {
                //alert(result.d.ods.length);
                $(result.d.ListOds.ods).each(function () {
                    var circle = paper.circle((parseFloat($(this)[0].coordinates.x) * 5) + 700, (parseFloat($(this)[0].coordinates.y) * 5) + 400, 15);

                    paper.text((parseFloat($(this)[0].coordinates.x) * 5) + 720, (parseFloat($(this)[0].coordinates.y) * 5) + 400, $(this)[0].id).attr(
                        {
                            "font-family": "serif",
                            "font-style": "italic",
                            "font-size": "10"
                        });
                });
                $(result.d.ListRelations.relaciones).each(function () {
                    if ($(this)[0].valor_relacion > 0.067) {
                        var color = ""
                        if ($(this)[0].valor_relacion < 0.269) {
                            color = "#B404AE";
                        }
                        else if ($(this)[0].valor_relacion < 0.47) {
                            color = "#FFFF00";
                        }
                        else color = "#B40404";

                        paper.path("M" +
                            parseFloat($(this)[0].inicio.x) +
                            ", " +
                            parseFloat($(this)[0].inicio.y) +
                            ", L" +
                            parseFloat($(this)[0].fin.x) +
                            "," +
                            parseFloat($(this)[0].fin.y)).attr({
                            stroke: color,
                            'stroke-width': 2,
                            'arrow-end':
                                'classic-wide-long'
                        });
                    }
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLDocument.toString());
            }
        });
    });
</script>
    <title></title>
        </head>
        <body>
        <form id="form1" runat="server">
        <div id="ods_div">
        </div>
    </form>
</body>
</html>
