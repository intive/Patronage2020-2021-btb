using System.ComponentModel.DataAnnotations;

namespace BTB.Domain.Common.Pagination
{
    public class PaginationDto
    {
        [Required]
        public int Page { get; set; }
        [Required]
        public PaginationQuantity Quantity { get; set; }
    }
}
