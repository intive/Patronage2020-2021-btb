namespace BTB.Domain.Entities
{
    public class GamblePoint
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public decimal FreePoints { get; set; }
        public decimal SealedPoints { get; set; }
    }
}
