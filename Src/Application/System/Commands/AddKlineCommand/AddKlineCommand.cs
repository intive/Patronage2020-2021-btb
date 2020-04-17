using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.System.Commands.AddKlineCommand
{
    public class AddKlineCommand : IRequest
    {
        public string SymbolPair { get; set; }
        public decimal Volume { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
    }
}
