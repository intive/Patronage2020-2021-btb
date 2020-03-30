google.charts.load('current', { 'packages': ['corechart', 'line'] });
google.charts.setOnLoadCallback(drawCandlestickChart);
google.charts.setOnLoadCallback(drawRSI);
google.charts.setOnLoadCallback(drawSMA);

function drawCandlestickChart(jsonData) {
    var data = new google.visualization.DataTable(jsonData, true);

    var options = {
        legend: 'none',
        colors: ['black', 'blue'],
        bar: { groupWidth: '90%' },
        candlestick: {
            fallingColor: { strokeWidth: 0.5, fill: 'red', stroke: 'black' },
            risingColor: { strokeWidth: 0.5, fill: 'green', stroke: 'black' },
            hollowIsRising: true
        },
        chartArea: { right: '3%', bottom: 5, top: '5%', width: "90%", height: "80%" },
        hAxis: { textPosition: 'none' }
    };


    var chart = new google.visualization.CandlestickChart(document.getElementById('candlestick_chart_div'));

    chart.draw(data, options);
}

function drawRSI(jsonData) {
    var data = new google.visualization.DataTable(jsonData);

    var options = {
        legend: 'none',
        chartArea: { right: '3%', top: 10, width: "90%", height: "80%" },
        vAxis: {
            viewWindow: {
                min: 0,
                max: 100
            }
        },
        series: {
            1: { lineDashStyle: [4, 1], lineWidth: 1 },
            2: { lineDashStyle: [4, 1], lineWidth: 1 }
        }
    };

    var chart = new google.visualization.LineChart(document.getElementById('rsi_chart_div'));

    chart.draw(data, options);
}

function drawSMA(jsonData) {
    var data = new google.visualization.DataTable(jsonData, true);

    var options = {
        legend: 'none',
        colors: ['black', 'blue'],
        bar: { groupWidth: '90%' },
        candlestick: {
            fallingColor: { strokeWidth: 0.5, fill: 'red', stroke: 'black' },
            risingColor: { strokeWidth: 0.5, fill: 'green', stroke: 'black' },
            hollowIsRising: true
        },
        chartArea: { right: '3%', width: "90%", height: "80%" },
        seriesType: "candlesticks",
        series: { 1: { type: 'line' } }
    };

    var chart = new google.visualization.ComboChart(document.getElementById('sma_chart_div'));
    chart.draw(data, options);
}
