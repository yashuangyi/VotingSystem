'use strict'

layui.config({
    base: '/Scripts/Project/Project/' //静态资源所在路径
}).use(['form','laydate','upload','table'], function () {
	var form = layui.form
		,laydate = layui.laydate
		,table = layui.table
		,upload = layui.upload;
	
	//项目列表数据表格
	table.render({
		elem:'#table_vote',
		height:800,
		width:1200,
		url:'/Project/GetProjectList', //数据接口
		page:true ,//开启分页
		cols:[[
			{field:"Id",title:"项目编号"},
			{field:"Name",title:"项目名称"},
			{field:"Remark",title:"备注"},
			{field:"Method",title:"评审方式"},
			{field:"StartTime",title:"开始时间"},
			{field:"EndTime",title:"结束时间"},
			{field:"Status",title:"状态"},
		]]
	});
	
	//限定投票的可选日期
	var nowDate = new Date().toLocaleDateString(); //当前日期
	var startDate = laydate.render({
		elem:'#startDate',
		min:nowDate,
		max:'2059-3-17',
		done:function(value,date){
			//约束结束时间的最小值
			endDate.config.min = {
				year:date.year,
				month:date.month-1,//注意JS的月份范围用整数表示是0~11
				date:date.date
			}
		}
	});
	var endDate = laydate.render({
		elem:'#endDate',
		min:nowDate,
		max:'2059-3-17',
		done:function(value,date){
			//约束开始时间的最大值
			startDate.config.max = {
				year:date.year,
				month:date.month-1,//注意JS的月份范围用整数表示是0~11
				date:date.date
			}
		}
	});
});