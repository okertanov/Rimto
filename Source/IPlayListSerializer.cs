using System;
using System.Collections.Generic;

namespace Rimto
{
    public interface IPlayListSerializer<T>
        where T : class
    {
        void Save(IEnumerable<T> list, object metadata);
        IEnumerable<T> Restore(object metadata);
    }
}
