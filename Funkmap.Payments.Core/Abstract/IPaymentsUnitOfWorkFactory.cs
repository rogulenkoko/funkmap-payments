
namespace Funkmap.Payments.Core.Abstract
{
    public interface IPaymentsUnitOfWorkFactory
    {
        IPaymentsUnitOfWork UnitOfWork();
    }
}
