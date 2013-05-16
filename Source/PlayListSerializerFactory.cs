using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Rimto
{
    public class PlayListSerializerFactory<T> :
        IPlayListSerializerFactory
        where T : class
    {
        private static readonly Dictionary<PlayListType, Type> _playListSerializers =
            new Dictionary<PlayListType, Type>
        {
            { PlayListType.Sqlite,  typeof(SqliteSerializer<T>)     },
            { PlayListType.Plain,   typeof(PlainTextSerializer<T>)  },
            { PlayListType.M3U8,    typeof(M3U8Serializer<T>)       },
            { PlayListType.PLS,     typeof(PlsSerializer<T>)        },
            { PlayListType.WPL,     typeof(WplSerializer<T>)        }
        };

        public static IPlayListSerializer<T> CreateSerializerFor(PlayListType type)
        {
            if (_playListSerializers.ContainsKey(type) &&
                _playListSerializers[type] != null)
            {
                return (IPlayListSerializer<T>)Activator.CreateInstance(_playListSerializers[type]);
            }
            else
            {
                throw new System.InvalidOperationException("No Play List Serializer for " + type.ToString());
            }
        }
    }
}
