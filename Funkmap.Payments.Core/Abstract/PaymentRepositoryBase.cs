using System.Threading.Tasks;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IRepositoryBase
    {
        Task SaveAsync();
    }
}
