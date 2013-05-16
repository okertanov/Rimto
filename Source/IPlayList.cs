using System;
using System.Collections.Generic;

namespace Rimto
{
    public interface IPlayList
    {
        void Add(IPlayListItem item);
        void Add(IEnumerable<IPlayListItem> items);

        void Add(string item);
        void Add(IEnumerable<string> items);

        void Remove(IPlayListItem item);
        void Remove(string item);

        void Clear();

        void Save(PlayListType type, string name);
        void Save(string media);

        void Restore(PlayListType type, string name);
        void Restore(string media);
    }
}
