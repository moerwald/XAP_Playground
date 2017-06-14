using System;
using NUnit.Framework;
using GigaSpaces.Core;
using GigaSpaces.Core.Document;
using GigaSpaces.Core.Metadata;
using FluentAssertions;

namespace Xap_Playground_Test
{
    [TestFixture]
    [Description("Implements exmaples of https://docs.gigaspaces.com/xap/12.1/dev-dotnet/document-api.html")]
    public partial class SpaceDocumentTests
    {
        private const string SpaceName = "myDataGrid";
        private const string SpaceDocument_Product = "Product";

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

                SpaceDocument template = new SpaceDocument(SpaceDocument_Product);
          
                template[Product.Properties.CatalogNumber] = "av-9876";

                var readDocumentFromSpace = spaceProxy.Read(template);

                readDocumentFromSpace.Should().NotBeNull();
                Assert.IsTrue(Equals(readDocumentFromSpace[Product.Properties.Name], "Jet Propelled Pogo Stick"));

                var price = readDocumentFromSpace[Product.Properties.Price];
                price.Should().Be(19.99);
            }

            // Local Helpers
            SpaceDocument CreateDocumemt()
            {
                DocumentProperties properties = new DocumentProperties();

                properties[Product.Properties.CatalogNumber] = "av-9876";
                properties[Product.Properties.Category] = "Aviation";
                properties[Product.Properties.Name] = "Jet Propelled Pogo Stick";
                properties[Product.Properties.Price] = 19.99;
                properties[Product.Properties.Tags] = new String[4] { "New", "Cool", "Pogo", "Jet" };

                DocumentProperties p2 = new DocumentProperties();
                p2[Product.Feature.Properties.Manufacturer] = "Acme";
                p2[Product.Feature.Properties.RequiresAssembly] = true;
                p2[Product.Feature.Properties.NumberOfParts] = 42;
                properties[Product.Feature.ClassName] = p2;

                SpaceDocument document = new SpaceDocument(Product.ClassName, properties);

                return document;
            }

            void RegisterProductTypeAt(ISpaceProxy proxy)
            {
                // Create type Document descriptor:
                SpaceTypeDescriptorBuilder typeBuilder = new SpaceTypeDescriptorBuilder(Product.ClassName);
                typeBuilder.SetIdProperty(Product.Properties.CatalogNumber);
                typeBuilder.SetRoutingProperty(Product.Properties.Category);
                typeBuilder.AddPathIndex(Product.Properties.Name);
                typeBuilder.AddPathIndex(Product.Properties.Price, SpaceIndexType.Extended);
                ISpaceTypeDescriptor typeDescriptor = typeBuilder.Create();

                // Register type:
                proxy.TypeManager.RegisterTypeDescriptor(typeDescriptor);
            }
        }
    }
}
