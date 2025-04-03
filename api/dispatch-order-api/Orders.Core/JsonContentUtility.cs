using Orders.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Core
{
    public class JsonContentUtility
    {
        public static string ConvertJSONToCsv(OrderPayload data)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CustomerReference,LoadId,ContainerType,ItemCode,ItemQuantity,ItemWeight,Street,City,State,PostalCode,Country");

            foreach (var container in data.Containers)
            {
                foreach (var item in container.Items)
                {
                    csvBuilder.AppendLine($"{data.SalesOrder},{container.LoadId},{ConvertContainerType(container.ContainerType)},{item.ItemCode},{item.Quantity},{item.CartonWeight},{data.DeliveryAddress.Street},{data.DeliveryAddress.City},{data.DeliveryAddress.State},{data.DeliveryAddress.PostalCode},{data.DeliveryAddress.Country}");
                }
            }

            return csvBuilder.ToString();
        }

        private static string ConvertContainerType(string originalType)
        {
            return originalType switch
            {
                "20RF" => "REF20",
                "40RF" => "REF40",
                "20HC" => "HC20",
                "40HC" => "HC40",
                _ => originalType
            };
        }
    }
}
