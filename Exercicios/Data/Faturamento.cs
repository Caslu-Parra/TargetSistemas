using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Exercicios.Data
{
    public record Faturamento
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("data")]
        public DateTime Data { get; set; }

        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }
}
