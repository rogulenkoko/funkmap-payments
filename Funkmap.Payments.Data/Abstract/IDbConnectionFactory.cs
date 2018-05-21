using System.Data;

namespace Funkmap.Payments.Data.Abstract
{
    public interface IDbConnectionFactory
    {

        IDbConnection Connection();

    }
}
