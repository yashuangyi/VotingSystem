'use strict'

layui.config({
    base: '/Scripts/Project/Home/' //静态资源所在路径
}).use(['element','form'], function () {
    var form = layui.form,
    element = layui.element;

    // 初始化信息
    window.onload = function(){
        if($('#admin_id').val()!==""){
            $.ajax({
                type: "post",
                url: "/Home/ReadState",
                data: {adminId:$('#admin_id').val()},
                dataType: "json",
                success: function (res) {
                    if(res.code === 200){
                        $('#admin_name').text(res.adminName);
                        $('#admin_power').val(res.adminPower);
                        $('#admin_photo').attr("src", res.adminPhoto);
                    }else{
                        layer.msg("账号异常，请联系系统管理员！");
                        location.href = "/Login/Login";
                    }
                }
            });
        }else{
            layer.msg("请重新登录！");
            location.href = "/Login/Login";
        }
    };
});

//修改iframe的读取路径并刷新
function setSrc(path) {
    //此句必须在前面
    $("#iframeMain").get(0).contentWindow.location.reload(true);
    var iframe = $("#iframeMain").get(0);
    iframe.src = path;
}

//刷新按钮
function freshView(){
    var iframe = $("#iframeMain").get(0);
    iframe.src = iframe.src;
}