using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Rimto
{
    public class PlainTextSerializer<T> :
        IPlayListSerializer<T>
        where T : class
    {
        public virtual void Save(IEnumerable<T> list, object metadata)
        {
            string filename = metadata as string;

            if (!String.IsNullOrEmpty(filename))
            {
                string[] lines = list.Select(i => i.ToString()).ToArray<string>();

                System.IO.File.WriteAllLines(filename, lines, Encoding.UTF8);
            }
            else
            {
                throw new ArgumentException("Invalid argument.", "metadata");
            }
        }

        public virtual IEnumerable<T> Restore(object metadata)
        {
            string filename = metadata as string;

            if (!String.IsNullOrEmpty(filename))
            {
                string[] lines = System.IO.File.ReadAllLines(filename, Encoding.UTF8);

                var enumerable = lines.Select(s => (T)Activator.CreateInstance(typeof(T), new object[] { s }));

                return enumerable.AsEnumerable();
            }
            else
            {
                throw new ArgumentException("Invalid argument.", "metadata");
            }
        }
    }
}
