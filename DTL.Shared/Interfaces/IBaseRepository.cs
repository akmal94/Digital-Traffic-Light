using System;
using System.Collections.Generic;
using System.Text;

namespace DTL.Shared.Interfaces
{
    public interface IBaseRepository
    {
        T Get<T>() where T : class;
    }
}
