using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rimto;

namespace RimtoTestProject
{
    [TestClass]
    public class PlayListSerializerTest
    {
        private TestContext testContextInstance { get; set; }

        [TestMethod]
        public void PlainTextSerializerTest()
        {
            const string filename = "PlainTextSerializerTest.txt";

            List<PlayListItem> list = new List<PlayListItem>
            {
                new PlayListItem("1"),
                new PlayListItem("2"),
                new PlayListItem("3")
            };

            var serializer = new PlainTextSerializer<PlayListItem>();
            serializer.Save(list, filename);
            List<PlayListItem> list2 = serializer.Restore(filename).ToList();

            CollectionAssert.Equals(list, list2);
        }
    }
}
