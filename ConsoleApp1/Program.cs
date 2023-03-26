using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please specify the path to the product file.");
            return;
        }

        string filePath = args[0];
        if (!File.Exists(filePath))
        {
            Console.WriteLine("The specified file does not exist.");
            return;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            string extension = Path.GetExtension(filePath);

            if (extension.TrimStart('.').Contains("json"))
            {
                //List<Product> products = JsonConvert.DeserializeObject<List<Product>>(json);
                var response = JsonConvert.DeserializeObject<ProductsResponse>(json);
                foreach (var product in response.Products)
                {
                    // Insert the product into your database or perform other processing
                    Console.WriteLine($"importing: Name: \"{product.Title}\"; Categories: {string.Join(", ", product.Categories)}; Twitter: @{product.Twitter}");
                }
            }

            if(extension.TrimStart('.').Contains("yaml"))
            {
                var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

                string yaml = File.ReadAllText(filePath);
                var response = deserializer.Deserialize<List<YamlProduct>>(yaml);

                foreach (var product in response)
                {
                    // Insert the product into your database or perform other processing
                    Console.WriteLine($"importing: Name: \"{product.name}\"; Categories: {string.Join(", ", product.tags)}; Twitter: @{product.twitter}");
                }
            }



            Console.WriteLine("Import completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while importing the products: " + ex.Message);
        }
    }
}

public class Product
{
    public string[] Categories { get; set; }
    public string Twitter { get; set; }
    public string Title { get; set; }
}

public class ProductsResponse
{
    public Product[] Products { get; set; }
}

public class YamlProduct
{
    public string tags { get; set; }
    public string name { get; set; }
    public string twitter { get; set; }
}

