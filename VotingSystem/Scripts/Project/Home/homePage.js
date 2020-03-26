//快速导航按钮事件
function linkBtn(url) {
    window.location.href = url;
}

//echarts饼图，需要注意先让DOM元素加载出来
$(document).ready(function(){
    var voteChart = echarts.init($('#echarts_vote').get(0));
var voteData = [];

var option = {
    tooltip: {
        trigger: 'item',
        formatter: '{a} <br/>{b}: {c} ({d}%)'
    },
    legend: {
        orient: 'vertical',
        left: 10,
        data: ['已评审', '未评审']
    },
    series: [
        {
            name: '进行中项目——评审情况',
            type: 'pie',
            radius: ['50%', '70%'],
            avoidLabelOverlap: false,
            label: {
                show: false,
                position: 'center'
            },
            emphasis: {
                label: {
                    show: true,
                    fontSize: '30',
                    fontWeight: 'bold'
                }
            },
            labelLine: {
                show: false
            },
            data: voteData
        }
    ]
};

$.get("/Home/GetEcharts", null,
    function (res) {
        if(res.code === 200){
            console.log(res.data);
            for(let i=0;i<res.count;i++){
                voteData.push(res.data[i]);
            }
            console.log(voteData);
            voteChart.setOption(option);
        }
    },
    "json"
);
});
