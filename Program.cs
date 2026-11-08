using System;
using System.Diagnostics;

class Program
{
    static int[] array;
    static int n;
    static long comparacoes;
    static long movimentacoes;
    static bool mostrarPassos;

    // ---------------------------------------------------------------
    // Metodos auxiliares
    // ---------------------------------------------------------------

    static void swap(int i, int j)
    {
        int temp = array[i];
        array[i] = array[j];
        array[j] = temp;
        movimentacoes += 3;
    }

    static void mostrar(int inicio, int fim)
    {
        Console.Write("[");
        for (int i = inicio; i <= fim; i++)
        {
            Console.Write(array[i]);
            if (i < fim) Console.Write(", ");
        }
        Console.WriteLine("]");
    }

    // ---------------------------------------------------------------
    // HEAPSORT (o heap usa as posicoes 1 ate n, como nos slides)
    // ---------------------------------------------------------------

    static void heapsort()
    {
        // Construcao do heap
        for (int tam = 2; tam <= n; tam++)
        {
            construir(tam);
        }

        if (mostrarPassos)
        {
            Console.Write("Heap construido: ");
            mostrar(1, n);
        }

        // Ordenacao propriamente dita
        int t = n;
        while (t > 1)
        {
            swap(1, t--);
            reconstruir(t);

            if (mostrarPassos)
            {
                Console.Write("tam = " + t + ": ");
                mostrar(1, n);
            }
        }
    }

    static void construir(int tam)
    {
        for (int i = tam; i > 1; i /= 2)
        {
            comparacoes++;
            if (array[i] > array[i / 2])
            {
                swap(i, i / 2);
            }
            else
            {
                i = 1; // para o laco
            }
        }
    }

    static void reconstruir(int tam)
    {
        int i = 1;
        while (i <= (tam / 2))
        {
            int filho = getMaiorFilho(i, tam);
            comparacoes++;
            if (array[i] < array[filho])
            {
                swap(i, filho);
                i = filho;
            }
            else
            {
                i = tam;
            }
        }
    }

    static int getMaiorFilho(int i, int tam)
    {
        int filho;
        if (2 * i == tam)
        {
            filho = 2 * i;
        }
        else
        {
            comparacoes++;
            if (array[2 * i] > array[2 * i + 1])
                filho = 2 * i;
            else
                filho = 2 * i + 1;
        }
        return filho;
    }

    // ---------------------------------------------------------------
    // QUICKSORT (pivo = elemento do meio, como nos slides)
    // ---------------------------------------------------------------

    static void quicksort(int esq, int dir)
    {
        int i = esq, j = dir, pivo = array[(esq + dir) / 2];

        while (i <= j)
        {
            comparacoes++;
            while (array[i] < pivo)
            {
                i++;
                comparacoes++;
            }

            comparacoes++;
            while (array[j] > pivo)
            {
                j--;
                comparacoes++;
            }

            if (i <= j)
            {
                swap(i, j);
                i++;
                j--;
            }
        }

        if (mostrarPassos)
        {
            Console.Write("quicksort(" + esq + ", " + dir + ") pivo = " + pivo + ": ");
            mostrar(0, n - 1);
        }

        if (esq < j)
            quicksort(esq, j);
        if (i < dir)
            quicksort(i, dir);
    }

    // ---------------------------------------------------------------
    // Chamadas dos dois algoritmos sobre uma copia do vetor
    // ---------------------------------------------------------------

    static void ordenarHeap(int[] vetor)
    {
        n = vetor.Length;
        array = new int[n + 1]; // posicao 0 nao e usada
        for (int i = 0; i < n; i++)
            array[i + 1] = vetor[i];

        comparacoes = 0;
        movimentacoes = 0;
        heapsort();
    }

    static void ordenarQuick(int[] vetor)
    {
        n = vetor.Length;
        array = new int[n];
        for (int i = 0; i < n; i++)
            array[i] = vetor[i];

        comparacoes = 0;
        movimentacoes = 0;
        if (n > 1)
            quicksort(0, n - 1);
    }

    // ---------------------------------------------------------------
    // Opcoes do menu
    // ---------------------------------------------------------------

    static void demonstracao(int[] vetor)
    {
        mostrarPassos = true;

        Console.Write("\nVetor original: [");
        Console.WriteLine(string.Join(", ", vetor) + "]");

        Console.WriteLine("\n--- Heapsort ---");
        ordenarHeap(vetor);
        Console.Write("Resultado: ");
        mostrar(1, n);
        Console.WriteLine("Comparacoes: " + comparacoes + " | Movimentacoes: " + movimentacoes);

        Console.WriteLine("\n--- Quicksort ---");
        ordenarQuick(vetor);
        Console.Write("Resultado: ");
        mostrar(0, n - 1);
        Console.WriteLine("Comparacoes: " + comparacoes + " | Movimentacoes: " + movimentacoes);

        mostrarPassos = false;
    }

    static void vetorDigitado()
    {
        Console.Write("Quantos numeros? ");
        int tam = int.Parse(Console.ReadLine());
        int[] vetor = new int[tam];
        for (int i = 0; i < tam; i++)
        {
            Console.Write("Numero " + (i + 1) + ": ");
            vetor[i] = int.Parse(Console.ReadLine());
        }
        demonstracao(vetor);
    }

    static void comparativo()
    {
        int[] tamanhos = { 1000, 10000, 100000, 1000000 };
        string[] tipos = { "Aleatorio", "Crescente", "Decrescente" };
        Random rnd = new Random(42);
        Stopwatch relogio = new Stopwatch();

        Console.WriteLine("\nAlgoritmo  Tipo          n        Comparacoes   Movimentacoes   Tempo(ms)");

        foreach (int tam in tamanhos)
        {
            foreach (string tipo in tipos)
            {
                // gera o vetor de teste
                int[] vetor = new int[tam];
                for (int i = 0; i < tam; i++)
                {
                    if (tipo == "Crescente") vetor[i] = i;
                    else if (tipo == "Decrescente") vetor[i] = tam - i;
                    else vetor[i] = rnd.Next(0, tam * 10);
                }

                relogio.Restart();
                ordenarHeap(vetor);
                relogio.Stop();
                Console.WriteLine("Heapsort   {0,-12} {1,8} {2,15} {3,15} {4,11:F2}",
                    tipo, tam, comparacoes, movimentacoes, relogio.Elapsed.TotalMilliseconds);

                relogio.Restart();
                ordenarQuick(vetor);
                relogio.Stop();
                Console.WriteLine("Quicksort  {0,-12} {1,8} {2,15} {3,15} {4,11:F2}",
                    tipo, tam, comparacoes, movimentacoes, relogio.Elapsed.TotalMilliseconds);
            }
            Console.WriteLine();
        }
    }

    // ---------------------------------------------------------------
    // Main
    // ---------------------------------------------------------------

    static void Main(string[] args)
    {
        int opcao = -1;
        while (opcao != 0)
        {
            Console.WriteLine("\n===== HEAPSORT x QUICKSORT =====");
            Console.WriteLine("1 - Exemplo passo a passo");
            Console.WriteLine("2 - Digitar um vetor");
            Console.WriteLine("3 - Comparativo de desempenho");
            Console.WriteLine("4 - Aplicacao pratica (transacoes)");
            Console.WriteLine("0 - Sair");
            Console.Write("Opcao: ");
            opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    int[] exemplo = { 101, 115, 30, 63, 47, 20 };
                    demonstracao(exemplo);
                    break;
                case 2:
                    vetorDigitado();
                    break;
                case 3:
                    comparativo();
                    break;
                case 4:
                    AplicacaoPratica.executar();
                    break;
                case 0:
                    break;
                default:
                    Console.WriteLine("Opcao invalida.");
                    break;
            }
        }
    }
}
