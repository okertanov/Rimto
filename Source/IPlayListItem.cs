using System;

namespace Rimto
{
    public interface IPlayListItem
    {
        string Name  { get; }
        string Path  { get; }
        string Title { get; }
        string Guid  { get; }

        string ToString();
    }
}
