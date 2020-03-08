'use strict'

layui.config({
    base: '/Scripts/Project/Project/' //静态资源所在路径
}).use(['form','laydate','upload','table'], function () {
	var form = layui.form
		,laydate = layui.laydate
		,table = layui.table
		,upload = layui.upload;

	//上传文件功能
	upload.render({
		elem:'#btn_selectFile',
		url:'/Project/UploadProject',
		auto:false ,//不自动上传
		accept:'file',
		acceptMime:'file/xlsx,file/xlx',
		bindAction:'#btn_uploadFile', //触发上传的按钮
		before:function(){
			layer.load();
		},
		done:function(res){
			layer.closeAll('loading');
			$('#btn_selectFile').html("<i class=''layui-icon layui-icon-upload-drag'></i>重新选择");
			if(res.code === 200){
				layer.msg(res.msg);
				$('#projectFile').html("<i class='layui-icon layui-icon-file'></i> <a href='" + res.filePath + "'>" + res.fileName + "<a/>");
				$('#filePath').val(res.filePath);
			}else{
				layer.msg(res.msg);
				$('#projectFile').html("");
				$('#filePath').val("");
			}
		},
	});

	//项目列表数据表格
	table.render({
		elem:'#table_vote',
		height:600,
        width: 1500,
		url:'/Project/GetProjectList', //数据接口
		page:true ,//开启分页
		cols:[[
			//{field:"Id",title:"项目编号"},
			{field:"Name",title:"项目名称"},
			{field:"Remark",title:"备注"},
			{field:"Method",title:"评审方式"},
			{field:"ContentNum",title:"内容数"},
			{field:"StartTime",title:"开始时间"},
			{field:"EndTime",title:"结束时间"},
			{field:"Status",title:"状态",templet:'#statusbar'},
			{fixed:'right',align:'center',toolbar:'#toolbar',width:200}
		]]
	});

	//监听"新增投票"按钮
	window.btn_addVote = function(){
		$('#projectId').val(0);
		$('#status').val("未启动");
		$('#contentNum').val(0);
		layer.open({
			type:1, //页面层
			title:"新建投票项目",
			area:['600px','700px'],
			btn:['保存','取消'],
			btnAlin:'c', //按钮居中
			content:$('#div_addVote'),
			success:function(layero,index){// 弹出layer后的回调函数,参数分别为当前层DOM对象以及当前层索引
				// 解决按回车键重复弹窗问题
				$('focus').blur();
				// 为当前DOM对象添加form标志
				layero.addClass('layui-form');
				// 将保存按钮赋予submit属性
				layero.find('.layui-layer-btn0').attr({
					'lay-filter':'btn_saveAdd',
					'lay-submit':''
				});
				// 表单验证
				form.verify();
				// 刷新渲染(否则开关按钮不会显示)
				form.render();
			},
			yes:function(index,layero){// 确认按钮回调函数,参数分别为当前层索引以及当前层DOM对象
				form.on('submit(btn_saveAdd)',function(data){//data按name获取
					if(data.field.ExpertCount<=0){
						layer.msg("评委数目必须大于0!");
						return false;
					}
					$.ajax({
						type: 'post',
						url:'/Project/AddProject',
						dataType:'json',
						data:data.field,
						success:function(res){
							if(res.code === 200){
								layer.alert("新建成功!",function(index){
									window.location.reload();
								});
							}
							else if(res.code === 402){
								layer.alert("当前已有进行中的投票项目!",function(index){
									window.location.reload();
								});
							}else{
								layer.alert(res.msg);
							}
						}
					});
				});
			}
		});
	}

	//监听表格工具栏
	table.on('tool(table_vote)',function(obj){
		var data = obj.data;
		if(obj.event === 'edit'){
			$('#projectId').val(data.Id);
			$('#projectName').val(data.Name);
			$('#projectRemark').val(data.Remark);
			var select = document.getElementsByName("method");
			for(let i=0;i<select.length;i++){
				if(select[i].value === data.Method){
					select[i].checked = true;
					break;
				}
			}
			$('#contentNum').val(data.ContentNum);
			$('#expertCount').val(data.ExpertCount);
			$('#startTime').val(data.StartTime);
			$('#endTime').val(data.EndTime);
			$('#status').val(data.Status);
			$('#filePath').val(data.FilePath);
			$('#projectFile').html("<i class='layui-icon layui-icon-file'></i> <a href='"+data.FilePath+"'>项目文件.xlsx<a/>");
			layer.open({
				type:1, //页面层
				title:"修改投票项目",
				area:['600px','700px'],
				btn:['保存','取消'],
				btnAlin:'c', //按钮居中
				content:$('#div_addVote'),
				success:function(layero,index){// 弹出layer后的回调函数,参数分别为当前层DOM对象以及当前层索引
					// 解决按回车键重复弹窗问题
					$('focus').blur();
					// 为当前DOM对象添加form标志
					layero.addClass('layui-form');
					// 将保存按钮赋予submit属性
					layero.find('.layui-layer-btn0').attr({
						'lay-filter':'btn_saveEdit',
						'lay-submit':''
					});
					// 表单验证
					form.verify();
					// 刷新渲染(否则开关按钮不会显示)
					form.render();
				},
				yes:function(index,layero){// 确认按钮回调函数,参数分别为当前层索引以及当前层DOM对象
					form.on('submit(btn_saveAdd)',function(data){//data按name获取
						$.ajax({
							type: 'post',
							url:'/Project/EditProject',
							dataType:'json',
							data:data.field,
							success:function(res){
								if(res.code === 200){
									layer.alert("修改成功!",function(index){
										window.location.reload();
									});
								}else{
									layer.alert(res.msg);
								}
							}
						});
					});
				}
			});
		}
		else if(obj.event === 'start'){
			layer.confirm('确认启动该投票项目?',function(index){
				$.getJSON('/Project/StartProject',{projectId:data.Id},function(res){
					if(res.code === 200){
						layer.alert("启动成功!",function success(){
							window.location.reload();
						});
					}
					else{
						layer.alert("启动失败!");
					}
				});
			})
		}
		else if(obj.event === 'stop'){
			layer.confirm('确认停止该投票项目?',function(index){
				$.getJSON('/Project/IsFinished',{projectId:data.Id},function(res){
					if(res.code === 200){
						layer.confirm("该项目共有"+res.total+"名评委.现还有"+res.rest+"位评委未投票,请问是否强制停止投票项目?",function(){
							$.getJSON('/Project/StopProject',{projectId:data.Id},function(res){
								if(res.code === 200){
									layer.alert("停止成功!",function success(){
										window.location.reload();
									});
								}
								else{
									layer.alert("停止失败!");
								}
							});
						});
					}
				});
			})
		}
		else if(obj.event === 'del'){
			layer.confirm('该操作将删除该项目下所属评审内容,确认删除该投票项目?',function(){
				$.getJSON('/Project/DelProject',{projectId:data.Id},function(res){
					if(res.code === 200){
						layer.alert("删除成功!",function success(){
							window.location.reload();
						});
					}
					else{
						layer.alert("删除失败!");
					}
				});
			})
		}
	});

	//限定投票的可选日期
	var nowDate = new Date().toLocaleDateString(); //当前日期
	var startDate = laydate.render({
		elem:'#startTime',
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
		elem:'#endTime',
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