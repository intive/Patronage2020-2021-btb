using BTB.Domain.Entities;
using BTB.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Commands.SeedSampleData
{
    public static class AlertMessageTemplates
    {
        public const string SymbolPairPlaceholder = "{SymbolPair}";
        public const string ValuePlaceholder = "{Value}";
        public const string ValueTypePlaceholder = "{ValueType}";
        public const string AdditionalValuePlaceholder = "{AdditionalValue}";

        public static IEnumerable<AlertMessageTemplate> GetTemplates()
        {
            return new List<AlertMessageTemplate>()
            {
                new AlertMessageTemplate() { Type = AlertCondition.Crossing,
                    Message = $"The {ValueTypePlaceholder} of {SymbolPairPlaceholder} is crossing {ValuePlaceholder}!" },

                new AlertMessageTemplate() { Type = AlertCondition.CrossingUp,
                    Message = $"The {ValueTypePlaceholder} of {SymbolPairPlaceholder} is crossing {ValuePlaceholder} up!" },

                new AlertMessageTemplate() { Type = AlertCondition.CrossingDown,
                    Message = $"The {ValueTypePlaceholder} of {SymbolPairPlaceholder} is crossing {ValuePlaceholder} down!" },

                new AlertMessageTemplate() { Type = AlertCondition.Between,
                    Message = $"The {ValueTypePlaceholder} of {SymbolPairPlaceholder} is between {ValuePlaceholder} and {AdditionalValuePlaceholder}!" }
            };
        }

        public static string FillTemplate(Alert alert, string template)
        {
            var builder = new StringBuilder(template);
            builder
                .Replace(SymbolPairPlaceholder, alert.SymbolPair.PairName)
                .Replace(ValuePlaceholder, alert.Value.ToString())
                .Replace(ValueTypePlaceholder, alert.ValueType.ToString())
                .Replace(AdditionalValuePlaceholder, alert.AdditionalValue.ToString());

            return builder.ToString();
        }
    }
}
