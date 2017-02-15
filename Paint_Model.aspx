<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Paint_Model.aspx.cs" Inherits="Validate.Paint_Model" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/json2.js" type="text/javascript"></script>
    <script src="Scripts/raphael.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            if ($("#HdnXmlRoute").val() == "") return;
            var paper = new Raphael(0, 0, 1000, 1000);

            var arrayParams = {}
            arrayParams.xmlVarRoute = $("#HdnXmlRoute").val();
            arrayParams.errores = $("#HdnErrores").val();
            arrayParams = JSON.stringify(arrayParams);
            $.ajax({
                type: "POST",
                url: "Paint_Model.aspx/ConsultNodeGraphics",
                data: arrayParams,
                contentType: "application/json; chartset=utf-8",
                dataType: "json",
                async: true,
                success: function (result) {
                    $(result.d).each(function () {
                        $((this).WorkProcess).each(function () {
                            paper.rect($(this)[0].xPosition, $(this)[0].yPosition, $(this)[0].Width, $(this)[0].Height);
                            var posicionX = parseFloat($(this)[0].xPosition) + parseFloat($(this)[0].Width / 2);
                            var posicionY = parseFloat($(this)[0].yPosition) + 13;
                            paper.text(posicionX, posicionY, $(this)[0].Name).attr(
{
    "font-family": "serif",
    "font-style": "italic",
    "font-size": "15"
});
                        });
                        $((this).ActivitiesToPaint).each(function () {
                            if ($(this)[0].Type == "Circ") {
                                if ($(this)[0].error != null) {
                                    var rect = paper.rect(($(this)[0].xPosition - $(this)[0].Width / 2) - 4, ($(this)[0].yPosition - $(this)[0].Width / 2) - 4, parseFloat($(this)[0].Width) + 8, parseFloat($(this)[0].Height) + 8);
                                    rect.node.setAttribute('fill', 'Red');
                                }
                                var circle = paper.circle($(this)[0].xPosition, $(this)[0].yPosition, $(this)[0].Width / 2);
                                if ($(this)[0].EventType == "Catch")
                                    circle.attr({ fill: 'url(Resources/CatchMessage.png)' })

                                if ($(this)[0].EventType == "Throw")
                                    circle.attr({ fill: 'url(Resources/ThrowMessage.png)' })

                                if ($(this)[0].EventType == "StartEvent") {
                                    circle.node.setAttribute('fill', '#F3F781');
                                    circle.node.setAttribute('stroke', '#FF4000')
                                    circle.node.setAttribute('stroke-width', '1');
                                }
                                if ($(this)[0].EventType == "EndEvent") {
                                    circle.node.setAttribute('fill', '#F8E0E0');
                                    circle.node.setAttribute('stroke', '#29220A')
                                    circle.node.setAttribute('stroke-width', '3');
                                }
                                if ($(this)[0].error != null)
                                    circle.attr({ title: 'El elemento no cumple con la validacion: ' + $(this)[0].error.split("|")[1] + ' de las normas de BPMN, favor corregirlo, el id del elemento con error es: ' + $(this)[0].error.split("|")[0] + ' y su nombre es: ' + $(this)[0].error.split("|")[2] })


                                paper.text($(this)[0].xPosition, parseFloat($(this)[0].yPosition) + parseFloat($(this)[0].Height), $(this)[0].Name).attr(
  {
      "font-family": "serif",
      "font-style": "italic",
      "font-size": "10"
  });
                            }
                            else {
                                if ($(this)[0].error != null) {
                                    var rect = paper.rect(parseFloat($(this)[0].xPosition) - 6, parseFloat($(this)[0].yPosition) - 6, parseFloat($(this)[0].Width) + 12, parseFloat($(this)[0].Height) + 12);
                                    rect.node.setAttribute('fill', 'Red');
                                }
                                var rect = paper.rect($(this)[0].xPosition, $(this)[0].yPosition, $(this)[0].Width, $(this)[0].Height);
                                var posicionX = parseFloat($(this)[0].xPosition) + parseFloat($(this)[0].Width / 2);
                                var posicionY = parseFloat($(this)[0].yPosition) + parseFloat($(this)[0].Height / 2);
                                rect.node.setAttribute('fill', '#FBEFFB');
                                if ($(this)[0].error != null)
                                    rect.attr({ title: 'El elemento no cumple con la validacion: ' + $(this)[0].error.split("|")[1] + ' de las normas de BPMN, favor corregirlo, el id del elemento con error es: ' + $(this)[0].error.split("|")[0] + ' y su nombre es: ' + $(this)[0].error.split("|")[2] })

                                paper.text(posicionX, posicionY, $(this)[0].Name).attr(
  {
      "font-family": "serif",
      "font-style": "italic",
      "font-size": "15"
  });

                            }
                        });

                        $((this).TransitionsToPaint).each(function () {
                            paper.path("M" + $(this)[0].FromX + ", " + $(this)[0].FromY + ", L" + $(this)[0].ToX + "," + $(this)[0].ToY);
                        });

                        $((this).Messages).each(function () {
                            if ($(this)[0].FromX == $(this)[0].ToX) {
                                var fromY = parseFloat($(this)[0].FromY);
                                var toY = parseFloat($(this)[0].FromY) + 10;
                                for (var y = 0; toY <= (parseFloat($(this)[0].ToY)) ; y++) {
                                    paper.path("M" + $(this)[0].FromX + ", " + fromY + ", L" + $(this)[0].ToX + "," + toY);
                                    fromY += 20;
                                    toY += 20;
                                }
                            }
                            if ($(this)[0].FromY == $(this)[0].ToY) {
                                var fromX = parseFloat($(this)[0].FromX);
                                var toX = parseFloat($(this)[0].FromX) + 10;
                                for (var x = 0; toX <= (parseFloat($(this)[0].ToX)) ; x++) {
                                    paper.path("M" + fromX + ", " + $(this)[0].FromY + ", L" + toX + "," + $(this)[0].ToY);
                                    fromX += 20;
                                    toX += 20;
                                }
                            }
                        });
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(XMLDocument.toString());
                }
            });
        });
    </script>
    <title>Validacion XPDL con normas de BPMN</title>
    <style type="text/css">
        #canvas_container {
            width: 500px;
            border: 1px solid #aaa;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="HdnXmlRoute" runat="server" />
        <asp:HiddenField ID="HdnErrores" runat="server" />
    </form>
</body>
</html>
