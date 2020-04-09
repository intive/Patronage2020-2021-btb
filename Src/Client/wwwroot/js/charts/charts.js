
function drawCandlestick(divId, candlestickJson) {

    var candlsetickData = JSON.parse(candlestickJson);

    document.getElementById(divId).innerHTML = "";
    var chart = LightweightCharts.createChart(document.getElementById(divId), {
        width: 900,
        height: 300,
        layout: {
            backgroundColor: 'rgba(251, 251, 251, 1.0)',
            textColor: 'rgba(85, 85, 85, 0.9)',
        },
        grid: {
            vertLines: {
                color: 'rgba(230, 230, 230, 0.9)',
            },
            horzLines: {
                color: 'rgba(230, 230, 230, 0.9)',
            },
        },
        crosshair: {
            mode: LightweightCharts.CrosshairMode.Normal,
        },
        priceScale: {
            borderColor: 'rgba(85, 85, 85, 0.9)',
            autoscale: true,
            scaleMargins: {
                top: 0.15,
                bottom: 0.1,
            },
        },
        timeScale: {
            borderColor: 'rgba(85, 85, 85, 0.9)',
            visible: true,
            timeVisible: true,
            secondsVisible: false,
        },
        priceFormat: {
            formatter: (price) => {
                return parseFloat(price).toFixed(2)
            }
        },
        localization: {
            priceFormatter: (price) => {
                return parseFloat(price).toFixed(5)
            }
        },
    });

    chart.timeScale().fitContent();

    var candleSeries = chart.addCandlestickSeries({
        upColor: 'rgba(112, 168, 0, 1)',
        borderUpColor: 'rgba(112, 168, 0, 1)',
        wickUpColor: 'rgba(112, 204, 0, 1)',
        downColor: 'rgba(234, 0, 112, 1)',
        borderDownColor: 'rgba(234, 0, 112, 1)',
        wickDownColor: 'rgba(234, 0, 112, 1)',
    });

    candleSeries.setData(candlsetickData);

    return chart;
}

function drawRSI(divCandlestickId, candlestickJson, divRSIId, rsiJson) {
    drawCandlestick(divCandlestickId, candlestickJson);

    var rsiData = JSON.parse(rsiJson);

    document.getElementById(divRSIId).innerHTML = "";
    var chart = LightweightCharts.createChart(document.getElementById(divRSIId), {
        width: 900,
        height: 200,
        layout: {
            backgroundColor: 'rgba(251, 251, 251, 1.0)',
            textColor: 'rgba(85, 85, 85, 0.9)',
        },
        grid: {
            vertLines: {
                color: 'rgba(230, 230, 230, 0.9)',
            },
            horzLines: {
                color: 'rgba(230, 230, 230, 0.9)',
            },
        },
        crosshair: {
            mode: LightweightCharts.CrosshairMode.Normal,
        },
        priceScale: {
            borderColor: 'rgba(85, 85, 85, 0.9)',
            autoscale: true,
        },
        timeScale: {
            borderColor: 'rgba(85, 85, 85, 0.9)',
            timeVisible: true,
            secondsVisible: false,
        },
    });

    chart.timeScale().fitContent();

    var rsiSeries = chart.addLineSeries({
        color: 'rgba(167, 79, 167, 0.9)',
        lineStyle: 0,
        lineWidth: 2.5,
        crosshairMarkerVisible: true,
    });

    var lineSeries30 = chart.addLineSeries({
        lineStyle: 3,
        color: 'rgba(0, 0, 0, 0.9)',
        lineWidth: 0.5,
    });

    var lineSeries70 = chart.addLineSeries({
        lineStyle: 3,
        color: 'rgba(0, 0, 0, 0.9)',
        lineWidth: 0.5,
    });

    rsiSeries.setData(rsiData);

    lineSeries30.setData([
        { time: rsiData[0].time, value: 30 },
        { time: rsiData[rsiData.length - 1].time, value: 30 },

    ]);

    lineSeries70.setData([
        { time: rsiData[0].time, value: 70 },
        { time: rsiData[rsiData.length - 1].time, value: 70 },

    ]);
}

function drawSMA(divId, candlestickJson, smaJson) {
    var chart = drawCandlestick(divId, candlestickJson);

    var smaData = JSON.parse(smaJson);

    var smaSeries = chart.addLineSeries({
        color: 'rgba(167, 79, 167, 0.9)',
        lineStyle: 0,
        lineWidth: 2.5,
        crosshairMarkerVisible: true,
    });

    smaSeries.setData(smaData);
}