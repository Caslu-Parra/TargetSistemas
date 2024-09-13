namespace Exercicios
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#### Teste técnico realizado por Lucas Parra - lucasparra2@outlook.com ####");
            Console.WriteLine("## Para inicarmos, selecione o exercicio desejado:");

            Console.WriteLine("- Exercicio 01 (Loop)");

            Console.Write(Environment.NewLine + "Exercicio selecionado: ");
            switch (Console.ReadLine())
            {
                case "01":
                case "1":
                    Console.WriteLine(Environment.NewLine + Exercicio01());
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
    }
}