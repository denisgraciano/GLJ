var aplicationPathName = "@CAT.Models.Utils.AplicationPathName";

var url = window.location;
var path = window.location.pathname.split('/');

url = url.protocol + "//" + url.host;

if (path[1] == aplicationPathName) {
    url = url + "/" + path[1];
}

function RemoverMensagens(tipo)
{
    $.post(url + '@Url.Content("~/Home/RemoverMensagem/?session=")' +tipo);
}

function dateFormatGrid(cellvalue, options, rowObject) {
    var format = options.colModel.formatoptions.srcformat;
    var milli = cellvalue.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));
    return d.format(format);
}

//Plugin para Jquery  - Reinicializar os valores dos formularios
jQuery.fn.reset = function () {
    $(this).each(function () { this.reset(); });
}

function disableEnterKey(e) {
    var key;
    if (window.event)
        key = window.event.keyCode; //IE
    else
        key = e.which; //firefox      

    return (key != 13);
}