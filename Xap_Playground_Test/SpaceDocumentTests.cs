using System;
using NUnit.Framework;
using GigaSpaces.Core;
using GigaSpaces.Core.Document;
using GigaSpaces.Core.Metadata;
using FluentAssertions;

namespace Xap_Playground_Test
{
    [TestFixture]
    public class SpaceDocumentTests
    {
        private const string SpaceName = "myDataGrid";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Description("Create an object using space document. Write that object to the space, read it afterwards and check some of the properties")]
        public void CreateSpaceDocument ()
        {
            using (var spaceProxy = new SpaceProxyFactory(SpaceName).Create())
            {
                var document = CreateDocumemt();
                RegisterProductTypeAt(spaceProxy);

                spaceProxy.Write(document);

                SpaceDocument template = new SpaceDocument("Product");
                template["CatalogNumber"] = "av-9876";

                var readDocumentFromSpace = spaceProxy.Read(template);

                readDocumentFromSpace.Should().NotBeNull();
                Assert.IsTrue(Equals(readDocumentFromSpace["Name"], "Jet Propelled Pogo Stick"));

                var price = readDocumentFromSpace["Price"];
                price.Should().Be(19.99);
            }

            // Local Helpers
            SpaceDocument CreateDocumemt()
            {
                DocumentProperties properties = new DocumentProperties();

                properties["CatalogNumber"] = "av-9876";
                properties["Category"] = "Aviation";
                properties["Name"] = "Jet Propelled Pogo Stick";
                properties["Price"] = 19.99;
                properties["Tags"] = new String[4] { "New", "Cool", "Pogo", "Jet" };

                DocumentProperties p2 = new DocumentProperties();
                p2["Manufacturer"] = "Acme";
                p2["RequiresAssembly"] = true;
                p2["NumberOfParts"] = 42;
                properties["Features"] = p2;

                SpaceDocument document = new SpaceDocument("Product", properties);

                return document;
            }

            void RegisterProductTypeAt(ISpaceProxy proxy)
            {
                // Create type Document descriptor:
                SpaceTypeDescriptorBuilder typeBuilder = new SpaceTypeDescriptorBuilder("Product");
                typeBuilder.SetIdProperty("CatalogNumber");
                typeBuilder.SetRoutingProperty("Category");
                typeBuilder.AddPathIndex("Name");
                typeBuilder.AddPathIndex("Price", SpaceIndexType.Extended);
                ISpaceTypeDescriptor typeDescriptor = typeBuilder.Create();

                // Register type:
                proxy.TypeManager.RegisterTypeDescriptor(typeDescriptor);
            }
        }
    }
}
