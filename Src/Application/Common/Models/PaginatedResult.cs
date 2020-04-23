using System.Collections.Generic;

namespace BTB.Application.Common.Models
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Result { get; set; }
        public int AllRecordsCount { get; set; }
        public int RecordsPerPage { get; set; }
    }
}
