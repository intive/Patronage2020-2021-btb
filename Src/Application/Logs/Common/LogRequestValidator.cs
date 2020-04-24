using BTB.Application.Common.Interfaces;
using BTB.Application.Logs.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Logs
{
    public class LogRequestValidator : AbstractValidator<LogRequestBase>
    {
        private readonly ILogFileService _logFileService;

        public LogRequestValidator(ILogFileService logFileService)
        {
            _logFileService = logFileService;

            RuleFor(a => a.LogDate)
                .Must(f => ValidateDateFormat(f));
        }

        private bool ValidateDateFormat(string input)
        {
            string format = _logFileService.LogDateFormat;
            string pattern = new DateTime().Date.ToString(format);

            if (string.IsNullOrEmpty(input))
                return true;

            bool validated = input.Length == pattern.Length;

            if (validated)
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    if (Char.IsDigit(pattern[i]))
                    {
                        validated = Char.IsDigit(input[i]);
                    }
                    else
                    {
                        validated = Char.Equals(pattern[i], input[i]);
                    }

                    if (!validated)
                        return false;
                }
            }

            return validated;
        }
    }
}
