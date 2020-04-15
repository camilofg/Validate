<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Paint_Network.aspx.cs" Inherits="Validate.Paint_Network" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/json2.js" type="text/javascript"></script>
    <script src="Scripts/raphael.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        /*global Raphael:true*/
        (function () {
            if (Raphael.vml) {
                Raphael.el.strokeLinearGradient = function () {
                    // not supporting VML yet
                    return this; // maintain chainability
                };
            } else {
                var setAttr = function (el, attr) {
                    var key;
                    if (attr) {
                        for (key in attr) {
                            if (attr.hasOwnProperty(key)) {
                                el.setAttribute(key, attr[key]);
                            }
                        }
                    } else {
                        return document.createElementNS("http://www.w3.org/2000/svg", el);
                    }

                    return null;
                };

                var defLinearGrad = function (defId, stops) {
                    var def = setAttr("linearGradient");
                    var i, l;
                    def.id = defId;

                    for (i = 0, l = stops.length; i < l; i += 1) {
                        var stopEle = setAttr("stop");
                        var stop = stops[i];
                        setAttr(stopEle, stop);

                        def.appendChild(stopEle);
                    }

                    return def;
                };

                Raphael.el.strokeLinearGradient = function (defId, width, stops) {

                    if (stops) {
                        this.paper.defs.appendChild(defLinearGrad(defId, stops));
                    }

                    setAttr(this.node, {
                        "stroke": "url(#" + defId + ")",
                        "stroke-width": width
                    });

                    return this; // maintain chainability
                };

                Raphael.st.strokeLinearGradient = function (defId, width, stops) {
                    return this.forEach(function (el) {
                        el.strokeLinearGradient(defId, width, stops);
                    });
                };

                Raphael.fn.defineLinearGradient = function (defId, stops) {

                    this.defs.appendChild(defLinearGrad(defId, stops));
                };
            }
        }());

        $(document).ready(function () {
            var paper = new Raphael("ods_div", 1000, 1000);
            var counter = 0;
            //var path = paper.path("M20 20L190 190");

            //path.strokeLinearGradient("grad1", 5);
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
                    //alert(result.d.ListOds.ods.length);
                    $(result.d.ListOds.ods).each(function () {
                        var circle = paper.circle((parseFloat($(this)[0].coordinates.x) * 5) + 300,
                            (parseFloat($(this)[0].coordinates.y) * 5) + 250,
                            15);

                        paper.text((parseFloat($(this)[0].coordinates.x) * 5) + 300,
                            (parseFloat($(this)[0].coordinates.y) * 5) + 250,
                            $(this)[0].id).attr(
                                {
                                    "font-family": "serif",
                                    "font-style": "italic",
                                    "font-size": "12",
                                    "font-weight": "bold"
                                });
                    });
                    $(result.d.ListRelations.relaciones).each(function () {
                        if ($(this)[0].rel_principal != '#FFFFFF' && $(this)[0].rel_principal != '' && $(this)[0].rel_secundaria == '') {
                            paper.path("M" +
                                parseFloat($(this)[0].inicio.x - 400) +
                                ", " +
                                parseFloat($(this)[0].inicio.y - 150) +
                                ", L" +
                                parseFloat($(this)[0].fin.x - 400) +
                                "," +
                                parseFloat($(this)[0].fin.y - 150)).attr({
                                    stroke: $(this)[0].rel_principal,
                                    'stroke-width': 1,
                                    'arrow-end':
                                        'classic-wide-long',
                                    "fill": "90-#f00:5-#00f:95",
                                    "fill-opacity": 0.5
                                });
                        }
                        if ($(this)[0].rel_principal != '' && $(this)[0].rel_secundaria != '') {
                            if ($(this)[0].rel_principal == $(this)[0].rel_secundaria) {
                                paper.path("M" +
                                    parseFloat($(this)[0].fin.x - 400) +
                                    ", " +
                                    parseFloat($(this)[0].fin.y - 150) +
                                    ", L" +
                                    parseFloat($(this)[0].inicio.x - 400) +
                                    "," +
                                    parseFloat($(this)[0].inicio.y - 150)).attr({
                                        stroke: $(this)[0].rel_principal,
                                        'stroke-width': 1,
                                    });
                            }
                            else {
                                paper.defineLinearGradient("grad" + counter,
                                    [
                                        {
                                            "id": "s1",
                                            "offset": "0",
                                            "style": "stop-color:" + $(this)[0].rel_principal + ";stop-opacity:1;"
                                        },
                                        {
                                            "id": "s2",
                                            "offset": "1",
                                            "style": "stop-color:" + $(this)[0].rel_secundaria + ";stop-opacity:1;"
                                        }
                                    ]);
                                var path = paper.path("M" +
                                    parseFloat($(this)[0].inicio.x - 400) +
                                    ", " +
                                    parseFloat($(this)[0].inicio.y - 150) +
                                    ", L" +
                                    parseFloat($(this)[0].fin.x - 400) +
                                    "," +
                                    parseFloat($(this)[0].fin.y - 150));


                                //var path = paper.path("M20 20L190 190");

                                path.strokeLinearGradient("grad" + counter, 2);
                                counter++;
                            }
                        }
                    });
                },

                //    if ($(this)[0].valor_relacion > 0.067) {
                //        var color = ""
                //        if ($(this)[0].valor_relacion < 0.269) {
                //            color = "#B404AE";
                //        }
                //        else if ($(this)[0].valor_relacion < 0.47) {
                //            color = "#FFFF00";
                //        }
                //        else color = "#00FD7F";

                //        paper.path("M" +
                //            parseFloat($(this)[0].inicio.x) +
                //            ", " +
                //            parseFloat($(this)[0].inicio.y) +
                //            ", L" +
                //            parseFloat($(this)[0].fin.x) +
                //            "," +
                //            parseFloat($(this)[0].fin.y)).attr({
                //            stroke: color,
                //            'stroke-width': 1,
                //            'arrow-end':
                //                'classic-wide-long',
                //            "fill": "90-#f00:5-#00f:95",
                //            "fill-opacity": 0.5
                //        });
                //    }
                //});

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
