'use strict'

layui.config({
    base: '/Scripts/Project/Login/'
}).use(['layer'], function () {
    //监听登录按钮
    $("#login").click(function () {
        $.ajax({
            url: "/Login/Check",
            dataType: "json",
            type: "post",
            data: {
                account: $('#account').val(),
                password: $('#password').val(),
            },
            success: function (res) {
                //登录成功
                if (res.code === 200) {
                    layer.open({
                        title: '欢迎~'
                        , content: '登录成功!'
                        , end: function () {
                            location.href = "/Home/Home";
                        }
                    });
                }
                else if (res.code === 401) {
                    layer.open({
                        title: 'Fail'
                        , content: '请输入账号及密码!'
                    });
                }
                else if (res.code === 404) {
                    layer.open({
                        title: 'Fail'
                        , content: '账号或密码错误，请重新输入!'
                    });
                }
            },
        });
    });

    //监听“回车键”
    $(document).keyup(function (event) {
        if (event.keyCode === 13) {
            $("#login").click();
        }
    });
});