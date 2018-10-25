using System;

namespace Funkmap.Payments.Core.Abstract
{
    public interface IFunkmapTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
