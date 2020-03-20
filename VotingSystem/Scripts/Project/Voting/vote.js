var contentNumPerPage = 10; //每页显示10个内容
var pageNumPerPage = 5; //每页显示页码数目
var page = 1; // 初始页码为1
var totalPageNum = 0; // 总页数
var hasVoteNum = 0; // 目前已投内容数
var totalVoteNum = 0; // 总内容数

// 初始化页面，显示第1页数据（首先要确保id值被读取，因此要加“锁”）
window.onload = function(){
    while(1){
        if($('#vote_expertId').val() != null){
            showContentOfPage(page);
            break;
        }
    }
};

// 查询指定页码的内容
function showContentOfPage(pageNum) {
    // 显示加载logo
    $.showLoading();
    var content = getContent(pageNum);
    $.post("/Voting/GetContentList", content,
        function (res) {
            if(res.code === 404){
                $.alert("您已完成了投票！");
                location.href = "/VotingHome/VotingHome";
            }else if(res.code === 200){
                var data = res.list;
                var showHtml = "";
                // 生成内容html并添加到页面中
                for(var i in data){
                    showHtml += 
                    "<div class='page-hd-title'>" + data[i].Number + " " + data[i].Name + "</div>" +
                    "<div class='weui-form' style='background-color:white;'>" +
                        "<div class='weui-form-li iblock'>" +
                            "<input class='weui-form-checkbox' onchange='changeState(this)' required name='"+data[i].Id+"' value='1' id='"+data[i].Id+1+"' type='radio'>"+
                            "<label for='" + data[i].Id + 1 + "'><i class='weui-icon-radio'></i>" +
                                "<div class='weui-form-text'><p>一等奖</p></div>"+
                            "</label>"+
                        "</div>"+
                        "<div class='weui-form-li iblock'>" +
                            "<input class='weui-form-checkbox' onchange='changeState(this)' name='"+data[i].Id+"' value='2' id='"+data[i].Id+2+"' type='radio'>"+
                            "<label for='" + data[i].Id + 2 + "'><i class='weui-icon-radio'></i>" +
                                "<div class='weui-form-text'><p>二等奖</p></div>"+
                            "</label>"+
                        "</div>"+
                        "<div class='weui-form-li iblock'>" +
                            "<input class='weui-form-checkbox' onchange='changeState(this)' name='"+data[i].Id+"' value='3' id='"+data[i].Id+3+"' type='radio'>"+
                            "<label for='" + data[i].Id + 3 + "'><i class='weui-icon-radio'></i>" +
                                "<div class='weui-form-text'><p>三等奖</p></div>"+
                            "</label>"+
                        "</div>"+
                        "<div class='weui-form-li iblock'>" +
                            "<input class='weui-form-checkbox' onchange='changeState(this)' name='"+data[i].Id+"' value='4' id='"+data[i].Id+4+"' type='radio'>"+
                            "<label for='" + data[i].Id + 4 + "'><i class='weui-icon-radio'></i>" +
                                "<div class='weui-form-text'><p>弃权</p></div>"+
                            "</label>"+
                        "</div>"+
                    "</div>" +
                    "</br>";
                }
                $('#vote_content').html(showHtml);
                // 加载保存的已填项
                for(let i in data){
                    if(data[i].Result!=null){
                        var radios = document.getElementsByName(data[i].Id);
                        for(let j=0;j<radios.length;j++){
                            if(radios[j].value == data[i].Result){
                                radios[j].checked = true;
                                break;
                            }
                        }
                    }
                }

                // 初始化悬浮栏
                hasVoteNum = res.hasVoteNum;
                totalVoteNum = res.totalVoteNum;
                var toolbarHtml = "<h4 class='f-blue' style='width:100%'>当前已评:" + hasVoteNum + "/" + totalVoteNum  + "</h4>";
                $('#vote_toolbar').html(toolbarHtml);

                // 分页处理
                totalPageNum = Math.ceil(totalVoteNum/contentNumPerPage); // 总页数
                var beginPage = (Math.ceil(pageNum/pageNumPerPage)-1)*pageNumPerPage+1; // 分页开始页码
                var endPage = (beginPage+pageNumPerPage-1<totalPageNum)?(beginPage+pageNumPerPage-1):totalPageNum; // 分页结束页码（考虑每页显示的页码数）
                if((endPage-beginPage<pageNumPerPage)&&(totalPageNum>pageNumPerPage)) {
                    beginPage = endPage-pageNumPerPage+1;
                }
                $('#vote_pageTotal').text("共有"+totalPageNum+"页,当前第"+pageNum+"页");

                // 生成分页按钮html
                var pageHtml = "";
                for (var i = beginPage; i <= endPage; i++){
                    pageHtml += "<a href='javascript:showContentOfPage(" + i + ");'>" + i + "</a>";
                }
                $("#vote_pager").html(pageHtml);
                // 取消加载logo
                $.hideLoading();
            }
        },
        "json"
    );
}

// 获取内容的对象
function getContent (pageNum) { 
    var content = {
        expertId : $('#vote_expertId').val(),
        pageNum : pageNum,
        contentNumPerPage : contentNumPerPage,
    };
    return content;
}

// 改变评分时触发
function changeState(element){
    var data = {
        contentId:element.name,
        expertId:$('#vote_expertId').val(),
        value: element.value,
    };
    $.post("/Voting/ChangeValue", data,
        function (res) {
            if(res.code === 200){
                if(res.isAdd === true){
                    hasVoteNum++;
                    var toolbarHtml = "<h4 class='f-blue' style='width:100%'>当前已评:" + hasVoteNum + "/" + totalVoteNum  + "</h4>";
                    $('#vote_toolbar').html(toolbarHtml);
                }
            }
        },
        "json"
    );
}

// 监听“上一页”按钮
$(document).on("click", "#vote_lastPage", function() {
    if(page === 1){
        $.toast("当前已是第一页！");
    }else{
        page--;
        showContentOfPage(page);
    }
});

// 监听“下一页”按钮
$(document).on("click", "#vote_nextPage", function() {
    if(page === totalPageNum){
        $.toast("当前已是最后一页！");
    }else{
        page++;
        showContentOfPage(page);
    }
});

// 监听“提交”按钮
$(document).on("click", "#vote_submit", function() {
    if(hasVoteNum!=totalVoteNum){
        $.toast("请先完成所有评分！");
    }else{
        $.alert("提交后无法修改，确认提交？",function(){
            var data = {
                expertId:$('#vote_expertId').val(),
            };
            $.post("/Voting/SubmitVote", data,
                function (res) {
                    if(res.code === 200){
                        $.alert("评分成功！请等待最终结果的通知...",function(){
                            location.href = "/VotingHome/VotingHome";
                        });
                    }
                },
                "json"
            );
        });
    }
});