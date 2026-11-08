using System;
using System.Diagnostics;

class AplicacaoPratica
{
    static string[] status = { "Aprovada", "Pendente", "Recusada", "Estornada" };

    static Transacao[] gerarTransacoes(int n, Random rnd)
    {
        Transacao[] vetor = new Transacao[n];
        DateTime inicio = new DateTime(2026, 1, 1);

        for (int i = 0; i < n; i++)
        {
            decimal valor = rnd.Next(1, 10000001) / 100m; // de R$ 0,01 ate R$ 100.000,00
            DateTime data = inicio.AddMinutes(rnd.Next(365 * 24 * 60));
            vetor[i] = new Transacao(i + 1, valor, data, status[rnd.Next(status.Length)]);
        }
        return vetor;
    }

    static string tempo(double ms)
    {
        if (ms < 1000) return ms.ToString("F2") + " ms";
        if (ms < 60000) return (ms / 1000).ToString("F2") + " s";
        if (ms < 3600000) return (ms / 60000).ToString("F1") + " min";
        if (ms < 86400000) return (ms / 3600000).ToString("F1") + " h";
        return (ms / 86400000).ToString("F1") + " dias";
    }

    public static void executar()
    {
        int[] tamanhos = { 1000, 10000, 100000, 1000000, 10000000, 100000000 };
        double[] tInsertion = new double[tamanhos.Length];
        double[] tMerge = new double[tamanhos.Length];
        double[] tQuick = new double[tamanhos.Length];
        bool[] executou = new bool[tamanhos.Length];
        bool[] ordenou = new bool[tamanhos.Length];

        Random rnd = new Random(42);
        Stopwatch relogio = new Stopwatch();
        long memoria = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
        Transacao[] maiorVetor = null;

        // roda uma vez antes para o tempo do 1.000 nao sair maior que o real
        Transacao[] teste = gerarTransacoes(1000, new Random(1));
        OrdenacaoTransacoes.insertion((Transacao[])teste.Clone());
        OrdenacaoTransacoes.mergesort((Transacao[])teste.Clone(), 0, 999);
        OrdenacaoTransacoes.quicksort((Transacao[])teste.Clone(), 0, 999);

        Console.WriteLine("\n===== APLICACAO PRATICA: TRANSACOES =====\n");

        for (int t = 0; t < tamanhos.Length; t++)
        {
            int n = tamanhos[t];

            // cada transacao ocupa uns 100 bytes de memoria
            if ((long)n * 100 > memoria / 2)
            {
                Console.WriteLine("n = {0,11:N0}: nao executado, falta memoria", n);
                continue;
            }

            Console.Write("n = {0,11:N0}: ", n);
            Transacao[] original = gerarTransacoes(n, rnd);
            Transacao[] copia;
            ordenou[t] = true;

            // insertion so ate 100 mil, depois fica lento demais
            if (n <= 100000)
            {
                Console.Write("Insertion... ");
                copia = (Transacao[])original.Clone();
                relogio.Restart();
                OrdenacaoTransacoes.insertion(copia);
                relogio.Stop();
                tInsertion[t] = relogio.Elapsed.TotalMilliseconds;
                ordenou[t] = ordenou[t] && OrdenacaoTransacoes.estaOrdenado(copia);
            }
            else
            {
                // n^2: n 10x maior = tempo 100x maior
                double fator = (double)n / tamanhos[t - 1];
                tInsertion[t] = tInsertion[t - 1] * fator * fator;
            }

            Console.Write("Merge... ");
            copia = (Transacao[])original.Clone();
            relogio.Restart();
            OrdenacaoTransacoes.mergesort(copia, 0, n - 1);
            relogio.Stop();
            tMerge[t] = relogio.Elapsed.TotalMilliseconds;
            ordenou[t] = ordenou[t] && OrdenacaoTransacoes.estaOrdenado(copia);

            Console.Write("Quick... ");
            copia = (Transacao[])original.Clone();
            relogio.Restart();
            OrdenacaoTransacoes.quicksort(copia, 0, n - 1);
            relogio.Stop();
            tQuick[t] = relogio.Elapsed.TotalMilliseconds;
            ordenou[t] = ordenou[t] && OrdenacaoTransacoes.estaOrdenado(copia);

            Console.WriteLine(ordenou[t] ? "ok" : "ERRO");
            executou[t] = true;
            maiorVetor = copia;
        }

        Console.WriteLine("\nInsertion x Merge x Quick");
        Console.WriteLine("{0,13} {1,22} {2,12} {3,12}", "n", "Insertion", "Merge", "Quick");
        for (int t = 0; t < tamanhos.Length; t++)
        {
            if (!executou[t]) continue;
            string insertion = tempo(tInsertion[t]);
            if (tamanhos[t] > 100000) insertion = "~" + insertion + " (estimado)";
            Console.WriteLine("{0,13:N0} {1,22} {2,12} {3,12}",
                tamanhos[t], insertion, tempo(tMerge[t]), tempo(tQuick[t]));
        }

        Console.WriteLine("\nMerge x Quick");
        Console.WriteLine("{0,13} {1,12} {2,12} {3,10}", "n", "Merge", "Quick", "Ordenado");
        for (int t = 0; t < tamanhos.Length; t++)
        {
            if (!executou[t])
            {
                Console.WriteLine("{0,13:N0}  nao executado, falta memoria", tamanhos[t]);
                continue;
            }
            Console.WriteLine("{0,13:N0} {1,12} {2,12} {3,10}",
                tamanhos[t], tempo(tMerge[t]), tempo(tQuick[t]), ordenou[t] ? "sim" : "nao");
        }

        // vetor em ordem crescente: as 10 maiores sao as 10 ultimas
        if (maiorVetor != null)
        {
            Console.WriteLine("\n10 maiores transacoes (n = {0:N0})", maiorVetor.Length);
            for (int i = 1; i <= 10; i++)
                Console.WriteLine("{0,2}. {1}", i, maiorVetor[maiorVetor.Length - i]);
        }
    }
}
