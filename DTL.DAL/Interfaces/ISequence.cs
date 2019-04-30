using Insight.Database;
using System;
using System.Collections.Generic;
using System.Data;

namespace DTL.DAL.Interfaces
{
    public interface ISequence : IDisposable
    {
        [Sql("GetSequenceId", CommandType.StoredProcedure)]
        int GetSequenceId(string Sequence);
        [Sql("AddSequence", CommandType.StoredProcedure)]
        void AddSequence(string Sequence);
        [Sql("GetStartNumbers", CommandType.StoredProcedure)]
        List<int> GetSequenceStartNumbers(int SequenceId);
        [Sql("AddStartNumbers", CommandType.StoredProcedure)]
        void AddSequenceStartNumber(int SequenceId, int StartNumber);
        [Sql("SequenceExist", CommandType.StoredProcedure)]
        object SequenceExist(string Sequence);
        [Sql("GetStartNumber", CommandType.StoredProcedure)]
        int GetStartNumber(string Sequence);
        [Sql("IsFinished", CommandType.StoredProcedure)]
        object IsFinished(string Sequence);
        [Sql("SetFinished", CommandType.StoredProcedure)]
        void SetFinished(string Sequence);
    }
}
