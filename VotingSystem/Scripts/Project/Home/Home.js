'use strict'

layui.config({
    base: '/Scripts/Project/Home/' //静态资源所在路径
}).use([], function () {
});

//修改iframe的读取路径并刷新
function setSrc(path) {
    //此句必须在前面
    //document.getElementById("iframeMain").contentWindow.location.reload(true);
    $("#iframeMain").get(0).contentWindow.location.reload(true);
    var iframe = $("#iframeMain").get(0);
    iframe.src = path;
}