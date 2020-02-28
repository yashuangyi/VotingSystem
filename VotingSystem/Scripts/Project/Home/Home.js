'use strict'

layui.config({
    base:'/Scripts/Project/Home/' //静态资源所在路径
}).use([],function(){
	
});

//修改iframe的读取路径并刷新
function setSrc(path){
	//此句必须在前面
	$('#iframeMain').contentWindow.location.reload(true);
	$('#iframeMain').src = path;
}