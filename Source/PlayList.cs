using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace Rimto
{
    [Flags]
    public enum PlaybackMode
    {
        Linear = 1,
        Looped = 2,
        Shuffle = 4,
        LoopedShuffle = Looped | Shuffle
    }

    public class PlayListEventArgs : EventArgs
    {
        public object Payload { get; private set; }
        public object Extra { get; private set; }

        public PlayListEventArgs(object payload)
        {
            Payload = payload;
            Extra = null;
        }

        public PlayListEventArgs(object payload, object extra)
        {
            Payload = payload;
            Extra = extra;
        }
    }

    public delegate void PlayListEventHandler(object sender, PlayListEventArgs ev);

    public class PlayList :
        IPlayList,
        IEnumerable<IPlayListItem>,
        INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };
        public event PlayListEventHandler Changed = delegate { };
        public event PlayListEventHandler Saved = delegate { };
        public event PlayListEventHandler Restored = delegate { };

        public PlaybackMode PlaybackMode { get; set; }

        const PlaybackMode _defaultPlaybackMode = PlaybackMode.Linear;
        const PlayListType _defaultPlayListType = PlayListType.Plain;

        private readonly PlayListType _playListType = _defaultPlayListType;
        private readonly string _playListName = System.Guid.NewGuid().ToString();
        private List<PlayListItem> _playListItems = new List<PlayListItem>();
        
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public PlayList()
        {
            PlaybackMode = _defaultPlaybackMode;
        }

        public PlayList(PlayList copy)
            : this()
        {
            _playListType = copy._playListType;
            _playListName = copy._playListName;
            _playListItems = copy._playListItems;
        }

        public PlayList(PlayListType type, string path)
            : this()
        {
            Restore(type, path);
        }

        public PlayList(IEnumerable<IPlayListItem> items)
            : this()
        {
            _playListItems = items.Cast<PlayListItem>().ToList();
        }

        public PlayList(IEnumerable<string> items)
            : this(items.Select(i => new PlayListItem(i) as IPlayListItem).ToList())
        {
        }

        public void Add(IEnumerable<IPlayListItem> items)
        {
            _playListItems.AddRange(items.Cast<PlayListItem>());

            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            Changed(this, new PlayListEventArgs(items));
        }

        public void Add(IEnumerable<string> items)
        {
            Add(items.Select(i => new PlayListItem(i) as IPlayListItem).ToList());
        }

        public void Add(IPlayListItem item)
        {
            _playListItems.Add(item as PlayListItem);

            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            Changed(this, new PlayListEventArgs(item));
        }

        public void Add(string item)
        {
            Add(new PlayListItem(item));
        }

        public void Remove(IPlayListItem item)
        {
            _playListItems.Remove(item as PlayListItem);

            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            Changed(this, new PlayListEventArgs(item));
        }

        public void Remove(string item)
        {
            _playListItems.RemoveAll(i => i.Path.Equals(item));

            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            Changed(this, new PlayListEventArgs(new PlayListItem(item)));
        }

        public void Clear()
        {
            _playListItems.Clear();

            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            Changed(this, new PlayListEventArgs(null));
        }

        public void Save(PlayListType type, string name)
        {
            var serializer = PlayListSerializerFactory<PlayListItem>.CreateSerializerFor(type);

            serializer.Save(_playListItems, name);

            Saved(this, new PlayListEventArgs(name, type));
        }

        public void Save(string media)
        {
            Save(_playListType, media);
        }

        public void Restore(PlayListType type, string name)
        {
            var serializer = PlayListSerializerFactory<PlayListItem>.CreateSerializerFor(type);

            Clear();

            _playListItems.AddRange(serializer.Restore(name));

            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            Changed(this, new PlayListEventArgs(null));
            Restored(this, new PlayListEventArgs(name, type));
        }

        public void Restore(string media)
        {
            Restore(_playListType, media);
        }

        public IEnumerator<IPlayListItem> GetEnumerator()
        {
            foreach (var item in _playListItems)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _playListItems.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return _playListItems.Count;
            }
        }
    }
}
