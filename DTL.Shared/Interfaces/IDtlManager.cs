using DTL.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTL.Shared.Interfaces
{
    public interface IDtlManager
    {
        SequenceResponse CreateSequence();
        ResponseModel GetResponse(ObservationModel model);
        void Clear();
    }
}
