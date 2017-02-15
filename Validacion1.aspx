<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Validacion1.aspx.cs" Inherits="Validate.Validacion1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("table").css('border', '1px solid black');
            $("table td").css('border', '1px solid black');
            $("td[id*='Archivo']").live("click", function () {
                $("#FRPage").attr("src", "Paint_Model.aspx?UbcArchivo=" + $(this).text());
                $("#divCont").css("top", $("#PnlResult").height() + 45);
                $("#divCont").css("display", "block");
            });
        });
    </script>
    <title>Validaciones XPDL Con normas de BPMN</title>
    <style type="text/css">
        #canvas_container {
            width: 500px;
            border: 1px solid #aaa;
        }
    </style>
</head>
<body>
    <div style="padding: 0px; z-index: 100; background: white; position: absolute; border: none; display: none; top: 250px; left: 20px; border-top: 1px solid"
        id="divCont">
        <iframe id="FRPage" width="1000px" height="1000px" style="top: 15px; left: 0px; position: absolute; background-color: white"
            scrolling="yes"></iframe>
    </div>
    <form id="form1" runat="server">
        <asp:Panel ID="PnlResult" runat="server">

        </asp:Panel>
        <br />

        <asp:FileUpload ID="UpFile" runat="server" CssClass="control" />
        <asp:Button ID="Vincular" Text="Subir Modelo" runat="server" OnClick="Vincular_Click" />
    </form>
</body>
</html>
