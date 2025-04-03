using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Orders.Core.Entities
{
    
    public class OrderPayload
    {
        [JsonPropertyName("salesOrder")]
        public string SalesOrder { get; set; }

        [JsonPropertyName("containers")]
        public List<Container> Containers { get; set; }

        [JsonPropertyName("deliveryAddress")]
        public DeliveryAddress DeliveryAddress { get; set; }

    }


    public class Container
    {
        [JsonPropertyName("loadId")]
        public string LoadId { get; set; }

        [JsonPropertyName("containerType")]
        public string ContainerType { get; set; }

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        [JsonPropertyName("itemCode")]
        public string ItemCode { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("cartonWeight")]
        public double CartonWeight { get; set; }
    }

    public class DeliveryAddress
    {
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
    }
}
