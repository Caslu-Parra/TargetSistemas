﻿using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Exercicios.Data;

namespace Exercicios
{
    class Program
    {
        static void Main()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(Environment.NewLine, new[] {
                "#### Teste técnico realizado por Lucas Parra - lucasparra2@outlook.com ####",
                "## Selecione o exercicio desejado:",
                "- Exercicio 01 (Loop)",
                "- Exercicio 02 (Fibonacci)",
                "- Exercicio 03 (Faturamento)",
                "- Exercicio 04 (Porcentagem)",
                "- Exercicio 05 (Inversor)",
                "# Exercicio: "
                });

            while (true)
            {
                Console.Write(sb);

                switch (Console.ReadLine())
                {
                    case "01":
                    case "1":
                        Console.WriteLine(Environment.NewLine + Exercicio01());
                        break;

                    case "02":
                    case "2":
                        Console.Write(Environment.NewLine + "# Informe um número: ");

                        int num = Convert.ToInt32(Console.ReadLine() ?? "0");
                        Console.WriteLine(Exercicio02(num) ? $"O número {num} pertence à sequência de Fibonacci." : $"O número {num} NÃO pertence à sequência de Fibonacci.");
                        break;

                    case "03":
                    case "3":
                        string mRet = string.Empty;
                        Console.WriteLine(Environment.NewLine + "## Escolha a fonte de dados desejada: 1 - json ou 2 - xml");
                        Console.Write("- Fonte: ");
                        switch (Console.ReadLine())
                        {
                            case "1":
                            case "json":
                                mRet = Exercicio03("json");
                                break;
                            case "2":
                            case "xml":
                                mRet = Exercicio03("xml");
                                break;
                            default:
                                mRet = "Tratamento não implementado";
                                break;
                        }
                        Console.WriteLine(mRet);
                        break;

                    case "04":
                    case "4":
                        var faturamentos = new Dictionary<string, decimal>
                    {
                       { "SP", 67836.43m },
                       { "RJ", 36678.66m },
                       { "MG", 29229.88m },
                       { "ES", 27165.48m },
                       { "Outros", 19849.53m }
                    };
                        Console.WriteLine(Environment.NewLine + Exercicio04(faturamentos));
                        break;

                    case "05":
                    case "5":
                        Console.Write(Environment.NewLine + "Informe um texto de entrada: ");
                        Console.WriteLine(Exercicio05(Console.ReadLine()));
                        break;

                    default:
                        Console.WriteLine(Environment.NewLine + "Exercicio não encontrado...");
                        break;
                }

                Console.Write("Pressione Enter para continuar ou ESC para encerrar...");
                if (Console.ReadKey(false).Key == ConsoleKey.Escape) break;
                Console.Clear();
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
        private static string Exercicio03(string pTipoArq)
        {
            try
            {
                var sb = new StringBuilder();
                var arquivo = Path.Combine(new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, $"MOCK_DATA.{pTipoArq}");
                using (StreamReader reader = new StreamReader(arquivo))
                {
                    Faturamento[] faturamentos;
                    if (arquivo.EndsWith("json"))
                    {
                        string jsonContent = reader.ReadToEnd();
                        faturamentos = JsonSerializer.Deserialize<Faturamento[]>(jsonContent);
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(FaturamentoList));
                        faturamentos = (serializer.Deserialize(reader) as FaturamentoList).Faturamentos;
                    }

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
        private static string Exercicio04(Dictionary<string, decimal> pFaturamentos)
        {
            decimal total = pFaturamentos.Sum(f => f.Value);
            var sb = new StringBuilder($"Total: {total:C}" + Environment.NewLine);

            sb.AppendJoin(Environment.NewLine, pFaturamentos.Select(f =>
            {
                decimal percentual = (f.Value / total) * 100;
                return $"{f.Key}: {f.Value:C} ({percentual:F2}%)";
            }));
            return sb.ToString();
        }
        private static string Exercicio05(string pTexto)
        {
            var sb = new StringBuilder($"Original: {pTexto}" + Environment.NewLine);
            sb.Append("Invertido: ");
            for (int i = pTexto.Length - 1; i > -1; i--) sb.Append(pTexto[i]);
            return sb.ToString();
        }
    }
}