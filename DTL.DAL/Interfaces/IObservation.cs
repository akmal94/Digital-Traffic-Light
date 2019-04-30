using Insight.Database;
using System;
using System.Collections.Generic;
using System.Data;

namespace DTL.DAL.Interfaces
{
    public interface IObservation : IDisposable
    {
        [Sql("DeleteFirstDigit", CommandType.StoredProcedure)]
        void DeleteFirstSector(int SequenceId, int SectorNumber);
        [Sql("DeleteSecondDigit", CommandType.StoredProcedure)]
        void DeleteSecondSector(int SequenceId, int SectorNumber);

        [Sql("GetFirstDigit", CommandType.StoredProcedure)]
        IEnumerable<int> GetFirstSector(int SequenceId);
        [Sql("GetSecondDigit", CommandType.StoredProcedure)]
        IEnumerable<int> GetSecondSector(int SequenceId);
    }
}
