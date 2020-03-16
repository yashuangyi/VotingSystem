// 初始化信息
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
                    $('#project_status').val(res.projectStatus);
                }else{
                    $.alert("账号异常，请联系系统管理员！");
                    location.href = "/Login/ExpertLogin";
                }
            }
        });
    }else{
        $.alert("请重新登录！");
        location.href = "/Login/ExpertLogin";
    }
};

// 监听“开始投票”按钮
$(document).on("click", "#btn_startVote", function() {
    if($('#expert_isVote').val() === "未投票" && $('#project_status').val() === "进行中"){
        $.ajax({
            type: "post",
            url: "/VotingHome/GetProjectDetail",
            data: {expertId:$('#expert_id').val()},
            dataType: "json",
            success: function (res) {
                if(res.code === 200){
                    $.confirm("项目名称:"+res.project.Name+"<br>备注:"+res.project.Remark+"<br>评审方式:"+res.project.Method+"<br>截止时间:"+res.project.EndTime, "项目信息", function(){
                        if(res.project.Method === "评分"){
                            location.href = "/Voting/Score?expertId="+$('#expert_id').val();
                        }else{
                            location.href = "/Voting/Vote?expertId="+$('#expert_id').val();
                        }
                    }, function(){

                    });
                }
            }
        });
    }else if($('#expert_isVote').val() === "已投票"){
        $.alert("您已投过票了！", "提示");
    }else{
        $.alert("投票已结束！", "提示");
    }
});

// 监听“我的评审记录”按钮
$(document).on("click", "#btn_myVote", function() {
    if($('#expert_isVote').val() === "已投票"){
        location.href = "/MyVote/MyVote?expertId="+$('#expert_id').val();
    }else{
        $.alert("请先完成投票！", "提示");
    }
});

// 监听“查看结果”按钮
$(document).on("click", "#btn_viewResult", function() {
    if($('#expert_isVote').val() === "完成统计"){
        location.href = "/VotingResult/VotingResult?expertId="+$('#expert_id').val();
    }else{
        $.alert("请等待最终结果的统计！", "提示");
    }
});