using System;
using System.Collections.Generic;
using System.Text;

namespace BTB.Application.Common.Models
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Result { get; set; }
        public int AllRecorsCount { get; set; }
        public int RecordsPerPage { get; set; }
    }
}
