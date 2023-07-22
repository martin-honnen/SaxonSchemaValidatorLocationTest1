using Saxon.Api;

namespace SaxonSchemaValidatorLocationTest1
{
    internal class Program
    {
        private static Processor processor;
        static void Main(string[] args)
        {
            processor = new();

            RunXsdValidation(Path.Combine(Environment.CurrentDirectory, "sample1-well-formed-invalid-schema-location.xml"), null);

            RunXsdValidation(Path.Combine(Environment.CurrentDirectory, "nesting-box-cuckoo1.xml"), null);
        }

        public static void RunXsdValidation(string instanceURI, string xsdURI)
        {
            var schemaManager = processor.SchemaManager;

            if (xsdURI != null && xsdURI != "")
            {
                using var xsdStream = File.OpenRead(xsdURI);
                schemaManager.Compile(xsdStream, new Uri(xsdURI));
            }

            var schemaValidator = processor.SchemaManager.NewSchemaValidator();

            using var inputStream = File.OpenRead(instanceURI);

            using var validationResultWriter = new StringWriter();

            var validationSerializer = processor.NewSerializer(validationResultWriter);
            validationSerializer.SetOutputProperty(Serializer.INDENT, "yes");

            schemaValidator.SetValidityReporting(validationSerializer);

            try
            {
                schemaValidator.Validate(inputStream, new Uri(instanceURI));
            }
            catch (SaxonApiException e)
            {

            }
            finally
            {
                var validationResult = validationResultWriter.ToString();
                Console.WriteLine(validationResult);
            }
        }
    }
}