using System.Globalization;
using System.Text;
using System.Text.Json;
using Exercicios.Data;

namespace Exercicios
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("#### Teste técnico realizado por Lucas Parra - lucasparra2@outlook.com ####");
            Console.WriteLine("## Para inicarmos, selecione o exercicio desejado:");

            Console.WriteLine("- Exercicio 01 (Loop)" + Environment.NewLine +
                              "- Exercicio 02 (Fibonacci)" + Environment.NewLine +
                              "- Exercicio 03 (Faturamento)" + Environment.NewLine);

            Console.Write("Exercicio selecionado: ");
            switch (Console.ReadLine())
            {
                case "01":
                case "1":
                    Console.Clear();
                    Console.WriteLine(Environment.NewLine + Exercicio01());
                    break;

                case "02":
                case "2":
                    Console.Clear();
                    Console.Write("# Informe um número: ");

                    int num = Convert.ToInt32(Console.ReadLine() ?? "0");
                    Console.WriteLine(Exercicio02(num) ? $"O número {num} pertence à sequência de Fibonacci." : $"O número {num} NÃO pertence à sequência de Fibonacci.");
                    break;

                case "03":
                case "3":
                    // Console.Clear();
                    Console.WriteLine(Exercicio03());
                    break;

                default:
                    Console.WriteLine("Exercicio não encontrado...");
                    break;
            }
        }
        private static string Exercicio01()
        {
            int indice = 13;
            int soma = 0;
            for (int k = 0; k < indice; k++)
            {
                soma += k;
            }
            return $"# Resultado de soma: {soma}";
        }
        private static bool Exercicio02(int num)
        {
            bool pertenceAoFibonacci;

            int a = 0, b = 1;

            // Se o número for 0 ou 1, ele pertence à sequência.
            if (num == 0 || num == 1) pertenceAoFibonacci = true;
            else
            {
                // Gera a sequência até encontrar um número maior ou igual ao informado
                while (b < num)
                {
                    int temp = a;
                    a = b;
                    b = temp + b;
                }
                // Verifica se o número encontrado é igual ao informado
                pertenceAoFibonacci = b == num;
            }
            return pertenceAoFibonacci;
        }
        private static string Exercicio03()
        {
            try
            {
                var sb = new StringBuilder();
                // Pega a raiz do projeto
                var path = new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
                using (StreamReader reader = new StreamReader(Path.Combine(path, "MOCK_DATA.json")))
                {
                    string jsonContent = reader.ReadToEnd();
                    var faturamentos = JsonSerializer.Deserialize<List<Faturamento>>(jsonContent);

                    // Ignora finais de semana, agrupa dias repetidos depois agrupa por Ano / Mes.
                    var mesGroup = faturamentos.Where(f => f.Data.DayOfWeek != DayOfWeek.Saturday &&
                                                           f.Data.DayOfWeek != DayOfWeek.Sunday)
                                               .GroupBy(f => f.Data.Date)
                                               .Select(g => new Faturamento
                                               {
                                                   Data = g.Key,
                                                   Valor = g.Sum(f => f.Valor)
                                               })
                                               .GroupBy(d => new { d.Data.Year, d.Data.Month });

                    foreach (var mes in mesGroup)
                    {
                        sb.AppendLine(Environment.NewLine + new string('-', 5) +
                                      $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes.Key.Month)}/{mes.Key.Year}" +
                                      new string('-', 5));

                        sb.AppendLine($"- Menor Faturamento: R$ {mes.Min(f => f.Valor):F2}");
                        sb.AppendLine($"- Maior Faturamento: R$ {mes.Max(f => f.Valor):F2}");

                        decimal media = mes.Average(f => f.Valor);
                        sb.AppendLine($"- Média Faturamento: R$ {media:F2}");

                        var diasMeta = mes.Where(f => f.Valor > media)
                                          .Select(f => f.Data.ToString("dd-ddd").TrimEnd('.'))
                                          .ToArray();

                        sb.Append($"# Dias maiores que a média mensal (Total {diasMeta.Count()}): ");
                        sb.AppendJoin(", ", diasMeta);
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex) { return ex.Message; }
        }
    }
}