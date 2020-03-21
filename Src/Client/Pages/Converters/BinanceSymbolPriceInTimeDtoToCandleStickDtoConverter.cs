using BTB.Client.Pages.Dto;
using System;
using System.Collections.Generic;

namespace BTB.Client.Pages.Converters
{
    public class BinanceSymbolPriceInTimeDtoToCandleStickDtoConverter
    {
        private readonly IEnumerable<BinanceSymbolPriceInTimeDto> _klines;
        private readonly decimal _maxValueInKlines;
        private readonly decimal _minValueInKlines;
        private readonly decimal _maxValueInChart;

        public BinanceSymbolPriceInTimeDtoToCandleStickDtoConverter(IEnumerable<BinanceSymbolPriceInTimeDto> klines, decimal maxValueInKlines, decimal minValueInKlines, decimal maxValueInChart)
        {
            _klines = klines;
            _maxValueInKlines = maxValueInKlines;
            _minValueInKlines = minValueInKlines;
            _maxValueInChart = maxValueInChart;
        }

        private decimal Normalize(decimal maxValueInChart, decimal maxInValues, decimal value, decimal minInValues)
        {
            decimal maxMinusMin = maxInValues - minInValues;

            return maxMinusMin == 0 ?
                (value - minInValues) * maxValueInChart : ((value - minInValues) / (maxMinusMin)) * maxValueInChart;
        }

        public IEnumerable<CandleStickDto> ConvertList()
        {
            List<CandleStickDto> candleSticks = new List<CandleStickDto>();

            foreach (var kline in _klines)
            {
                candleSticks.Add(ConvertKline(kline, _maxValueInKlines, _minValueInKlines, _maxValueInChart));
            }

            candleSticks.Reverse();

            return candleSticks;
        }

        private CandleStickDto ConvertKline(BinanceSymbolPriceInTimeDto kline, decimal maxValueInKlines, decimal minValueInKlines, decimal maxValueInChart)
        {
            var closePriceNormalized = Normalize(maxValueInChart, maxValueInKlines, kline.ClosePrice, minValueInKlines);
            var openPriceNormalized = Normalize(maxValueInChart, maxValueInKlines, kline.OpenPrice, minValueInKlines);
            var higestPriceNormalized = Normalize(maxValueInChart, maxValueInKlines, kline.HighestPrice, minValueInKlines);
            var lowestPriceNormalized = Normalize(maxValueInChart, maxValueInKlines, kline.LowestPrice, minValueInKlines);

            return new CandleStickDto()
            {
                ClosePrice = Convert.ToDouble(closePriceNormalized),
                OpenPrice = Convert.ToDouble(openPriceNormalized),
                HighestPrice = Convert.ToDouble(higestPriceNormalized),
                LowestPrice = Convert.ToDouble(lowestPriceNormalized),
                OpenTime = kline.OpenTime.ToString()
            };
        }
    }
}
