using System.ComponentModel.DataAnnotations;
using BTB.Client.Pages.Enums;

namespace BTB.Client.Pages.Dto
{
    public class PaginationDto
    {
        [Required]
        public int Page { get; set; }
        [Required]
        public PaginationQuantity Quantity { get; set; }
    }
}
