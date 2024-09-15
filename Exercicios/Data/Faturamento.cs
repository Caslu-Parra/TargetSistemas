using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Exercicios.Data
{
    public record Faturamento
    {
        [JsonPropertyName("id")]
        [XmlElement("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("data")]
        [XmlElement("data")]
        public DateTime Data { get; set; }

        [JsonPropertyName("valor")]
        [XmlElement("valor")]
        public decimal Valor { get; set; }
    }

    [XmlRoot("root")]
    public record FaturamentoList
    {
        [XmlElement("faturamento")]
        public Faturamento[] Faturamentos { get; set; }
    }
}
