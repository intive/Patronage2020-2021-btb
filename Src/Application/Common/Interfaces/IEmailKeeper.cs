namespace BTB.Application.Common.Interfaces
{
    public interface IEmailKeeper
    {
        void IncrementEmailSent();
        bool CheckIfLimitHasBeenReached();
    }
}
