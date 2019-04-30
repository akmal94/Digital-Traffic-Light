using Insight.Database;
using System;
using System.Data;

namespace DTL.DAL.Interfaces
{
    public interface IClear : IDisposable
    {
        [Sql("ClearData", CommandType.StoredProcedure)]
        void ClearData();
    }
}
