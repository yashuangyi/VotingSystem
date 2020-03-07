'use strict'

layui.config({
    base: '/Scripts/Project/Expert/' //静态资源所在路径
}).use(['form','table'], function () {
	var form = layui.form
		,table = layui.table;

	//项目列表数据表格
	table.render({
		elem:'#table_expert',
		height:600,
        width: 1200,
		url:'/Expert/GetExpertList', //数据接口
		page:true ,//开启分页
		where:{search:$('#searchProject').val()} ,//异步数据接口参数
		cols:[[
			{field:"ProjectName",title:"项目名称"},
			{field:"Name",title:"名字"},
			{field:"Account",title:"账号"},
			{field:"Password",title:"密码"},
			{field:"Status",title:"状态",templet:'#statusbar'},
			{fixed:'right',align:'center',toolbar:'#toolbar',width:200}
		]]
	});
	
	//加载选择框选项
	$.getJSON('/Expert/ShowChoice',function(data){
		if(data.code === 200){
			$.each(data.choice, function(index,choice){
				$('#searchProject').append(new Option(choice));//往下拉菜单中添加元素
			});
			form.render();//菜单渲染,加载内容
		}
	});
	
	//监听搜索
	form.on('submit(search-project)',function(data){
		table.reload('table_expert',{
			where:data.filed
		});
	});

	//监听表格工具栏
	table.on('tool(table_expert)',function(obj){
		var data = obj.data;
		if(obj.event === 'showCode'){
			layer.open({
				type:1 ,//页面层
				area:['300px','350px'],
				scrollbar:false,
				content:"<img src='"+data.CodePath+"' width=300px;height=200px;/>"
			});
		}
	});
});