using System;
using System.Collections.Generic;

namespace Rimto
{
    public class WplSerializer<T> :
            IPlayListSerializer<T>
            where T : class
    {
        public virtual void Save(IEnumerable<T> list, object metadata)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> Restore(object metadata)
        {
            throw new NotImplementedException();
        }
    }
}
