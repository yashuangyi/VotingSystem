window.onload = function(){
    if($('#expert_id').val()!==""){
        $.ajax({
            type: "post",
            url: "/VotingHome/ReadState",
            data: {expertId:$('#expert_id').val()},
            dataType: "json",
            success: function (res) {
                if(res.code === 200){
                    $('#expert_name').val(res.expertName);
                    $('#expert_isVote').val(res.expertIsVote);
                }else{
                    $.alert("账号异常，请联系管理员！");
                    location.href = "/Login/ExpertLogin";
                }
            }
        });
    }else{
        $.alert("请重新登录！");
        location.href = "/Login/ExpertLogin";
    }
};

$(document).on("click", "#btn_startVote", function() {
    if($('#expert_isVote').val() === "未投票"){
        $.ajax({
            type: "post",
            url: "/VotingHome/GetProjectDetail",
            data: {expertId:$('#expert_id').val()},
            dataType: "json",
            success: function (res) {
                if(res.code === 200){
                    $.alert("项目名称:"+res.project.Name+"<br>备注:"+res.project.Remark+"<br>评审方式:"+res.project.Method+"<br>截止时间:"+res.project.EndTime, "项目信息");
                }
            }
        });
    }else{
        $.alert("您已投过票了！", "提示");
    }
});

$(document).on("click", "#btn_viewResult", function() {
    if($('#expert_isVote').val() === "已投票"){
        $.ajax({
            type: "post",
            url: "/VotingHome/GetProjectDetail",
            data: {expertId:$('#expert_id').val()},
            dataType: "json",
            success: function (res) {
                if(res.code === 200){
                    $.alert("项目名称:"+res.project.Name+"<br>备注:"+res.project.Remark+"<br>评审方式:"+res.project.Method+"<br>截止时间:"+res.project.EndTime, "项目信息");
                }
            }
        });
    }else{
        $.alert("请先完成投票！", "提示");
    }
});