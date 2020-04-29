using System.ComponentModel.DataAnnotations;

namespace BTB.Domain.Common.Pagination
{
    public class PaginationDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Page { get; set; }
        [Required]
        [EnumDataType(typeof(PaginationQuantity))]
        public PaginationQuantity Quantity { get; set; }
    }
}
