using GigaSpaces.Core;
using GigaSpaces.Core.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GigaSpaces.Core.Metadata;

namespace Xap_Playground_Test
{
    [TestFixture]
    public class PonoTests
    {
        public class Person
        {
            [SpaceID]
            public int? Ssn { get; set; }
            public String FirstName { get; set; }
            public String LastName { get; set; }
        }

        [Test]
        public void CreatePonoInSpace()
        {
            Console.WriteLine("Connecting to data grid...");

            ISpaceProxy spaceProxy = new SpaceProxyFactory("myDataGrid").Create();


            Console.WriteLine("Write (store) a couple of entries in the data grid:");
            spaceProxy.Write(new Person { Ssn = 1, FirstName = "Vincent", LastName = "Chase" });
            spaceProxy.Write(new Person { Ssn = 2, FirstName = "Johnny", LastName = "Drama" });

            Console.WriteLine("Read (retrieve) an entry from the grid by its id:");
            Person person1 = spaceProxy.ReadById<Person>(1);
            person1.Should().NotBeNull();
            person1.FirstName.Should().BeEquivalentTo("Vincent");
            person1.LastName.Should().BeEquivalentTo("Chase");

            Console.WriteLine("Result: " + person1);

            Console.WriteLine("Read an entry from the grid using LINQ:");
            var query = from p in spaceProxy.Query<Person>()
                        where p.FirstName == "Johnny"
                        select p;

            var spaceQuery = query.ToSpaceQuery();

            Person person2 = spaceProxy.Read<Person>(spaceQuery);
            person2.LastName.Should().BeEquivalentTo("Drama");

            Console.WriteLine("Result: " + person2);

            Console.WriteLine("Read all entries of type Person from the grid:");
            Person[] results = spaceProxy.ReadMultiple(new Person());
            results.Count().Should().Be(2);
            Console.WriteLine("Result: " + String.Join<Person>(",", results));
        }
    }
}
