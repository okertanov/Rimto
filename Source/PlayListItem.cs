using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rimto
{
    public class PlayListItem : IPlayListItem
    {
        public string Path  { get; protected set; }
        public string Name  { get; protected set; }
        public string Title { get; protected set; }
        public string Guid  { get; protected set; }

        public PlayListItem()
            : this(null, null, null)
        {
        }

        public PlayListItem(string path)
            : this(path, null, null)
        {
        }

        public PlayListItem(string path, string name)
            : this(path, name, null)
        {
        }

        public PlayListItem(string path, string name, string title)
        {
            Path  = path;
            Name  = name;
            Title = title;
            Guid  = System.Guid.NewGuid().ToString();

            // Deconstruct missing parts
            if (String.IsNullOrEmpty(Path))
            {
                Path = @"<Unknown track>";
            }

            if (String.IsNullOrEmpty(Name))
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(Path);
            }

            if (String.IsNullOrEmpty(Title))
            {
                Title = Name;
            }
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
