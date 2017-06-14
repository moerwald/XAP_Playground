namespace Xap_Playground_Test
{
    public partial class SpaceDocumentTests
    {
        static class Product
        {
            public static readonly string ClassName = "Product";
            public static class Properties
            {
                public static readonly string CatalogNumber = $"{nameof(CatalogNumber)}";
                public static readonly string Name = $"{nameof(Name)}";
                public static readonly string Price = $"{nameof(Price)}";

                public static readonly string Tags = $"{nameof(Tags)}";

                public static readonly string Category = $"{nameof(Category)}";
            }

            public static class Feature
            {
                public static readonly string ClassName = $"{nameof(Feature)}";

                public static class Properties
                {
                    public static readonly string Manufacturer = $"{nameof(Manufacturer)}";

                    public static readonly string RequiresAssembly = $"{nameof(RequiresAssembly)}";

                    public static readonly string NumberOfParts = $"{nameof(NumberOfParts)}";
                }
            }
        }
    }
}
