'use strict'

layui.config({
    base: '/Scripts/Project/Result/' //静态资源所在路径
}).use(['form','table'], function () {
	var form = layui.form
		,table = layui.table;

	//项目列表数据表格
	table.render({
		elem:'#table_result',
		height:600,
        width: 1500,
		url:'/Result/GetResultList', //数据接口
		page:true ,//开启分页
		where:{search:$('#search_Project').val()} ,//异步数据接口参数
		cols:[[
			{field:"ProjectName",title:"项目名称"},
			{field:"Number",title:"评审编号"},
			{field:"Name",title:"评审内容"},
			{field:"Progress",title:"评审进度"},
			{field:"Method",title:"评审方式"},
			{field:"TicketsNum",title:"票数(一等奖/二等奖/三等奖/弃权)",width:300},
			{field:"Score",title:"分数",sort:"true"},
			{field:"Result",title:"结果"},
		]]
	});
	
	//加载选择框选项
	$.getJSON('/Result/ShowChoice',function(data){
		if(data.code === 200){
			$.each(data.choice, function(index,choice){
				$('#search_Project').append(new Option(choice));//往下拉菜单中添加元素
			});
			form.render();//菜单渲染,加载内容
		}
	});
	
	//监听选择框
	form.on('select(search_Project)',function(data){
		table.reload('table_result',{
			where:{search:data.value}
		});
	});
});