namespace Exercicios
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#### Teste técnico realizado por Lucas Parra - lucasparra2@outlook.com ####");
            Console.WriteLine("## Para inicarmos, selecione o exercicio desejado:");

            Console.WriteLine("- Exercicio 01 (Loop)" + Environment.NewLine +
                              "- Exercicio 02 (Fibonacci)" + Environment.NewLine);

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
    }
}