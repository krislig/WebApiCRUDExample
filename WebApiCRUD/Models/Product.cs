using Newtonsoft.Json;
using System;

namespace WebApiCRUD.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("price")]
        public Decimal Price { get; set; }
    }
}
