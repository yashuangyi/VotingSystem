var contentNumPerPage = 10; //每页显示10个内容
var pageNumPerPage = 5; //每页显示页码数目
var page = 1; // 初始页码为1
var totalPageNum = 0; // 总页数
var totalVoteNum = 0; // 总内容数

// 初始化页面，显示第1页数据（首先要确保id值被读取，因此要加“锁”）
window.onload = function(){
    while(1){
        if($('#myScore_expertId').val() != null){
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
    $.post("/MyVoting/GetContentList", content,
        function (res) {
            if(res.code === 200){
                var data = res.list;
                var showHtml = "";
                // 生成内容html并添加到页面中
                for(var i in data){
                    showHtml += 
                    "<div class='page-hd-title'>" + data[i].Number + " " + data[i].Name + "</div>" +
                    "<div class='weui-cells weui-cells_form'>" +
                        "<div class='weui-cell'>" +
                            "<div class='weui-cell__hd'><label class='weui-label'>评分</label></div>"+
                            "<div class='weui-cell__bd weui-cell__primary'>"+
                                "<input class='weui-input' required name='" + data[i].Id + "' id='" + data[i].Id + "' type='number' readonly=''>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "</br>";
                }
                $('#myScore_content').html(showHtml);
                // 加载保存的已填项
                for(let i in data){
                    if(data[i].Result!=null){
                        var input = document.getElementById(data[i].Id);
                        input.value=data[i].Result;
                    }
                }

                // 分页处理
                totalPageNum = Math.ceil(totalVoteNum/contentNumPerPage); // 总页数
                var beginPage = (Math.ceil(pageNum/pageNumPerPage)-1)*pageNumPerPage+1; // 分页开始页码
                var endPage = (beginPage+pageNumPerPage-1<totalPageNum)?(beginPage+pageNumPerPage-1):totalPageNum; // 分页结束页码（考虑每页显示的页码数）
                if((endPage-beginPage<pageNumPerPage)&&(totalPageNum>pageNumPerPage)) {
                    beginPage = endPage-pageNumPerPage+1;
                }
                $('#myScore_pageTotal').text("共有"+totalPageNum+"页,当前第"+pageNum+"页");

                // 生成分页按钮html
                var pageHtml = "";
                for (let i = beginPage; i <= endPage; i++){
                    pageHtml += "<a href='javascript:showContentOfPage(" + i + ");'>" + i + "</a>";
                }
                $("#myScore_pager").html(pageHtml);
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
        expertId : $('#myScore_expertId').val(),
        pageNum : pageNum,
        contentNumPerPage : contentNumPerPage,
    };
    return content;
}

// 监听“上一页”按钮
$(document).on("click", "#myScore_lastPage", function() {
    if(page === 1){
        $.toast("当前已是第一页！");
    }else{
        page--;
        showContentOfPage(page);
    }
});

// 监听“下一页”按钮
$(document).on("click", "#myScore_nextPage", function() {
    if(page === totalPageNum){
        $.toast("当前已是最后一页！");
    }else{
        page++;
        showContentOfPage(page);
    }
});