namespace BTB.Domain.ValueObjects
{
    public class DashboardPairVO : SimplePriceVO
    {
        public int Id { get; set; }
        public bool IsFavorite { get; set; }
    }
}
