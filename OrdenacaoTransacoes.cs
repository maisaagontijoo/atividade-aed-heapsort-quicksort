class OrdenacaoTransacoes
{
    // ---------------------------------------------------------------
    // INSERTION SORT
    // ---------------------------------------------------------------

    public static void insertion(Transacao[] array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            Transacao tmp = array[i];
            int j = i - 1;

            while (j >= 0 && array[j].Valor > tmp.Valor)
            {
                array[j + 1] = array[j];
                j--;
            }
            array[j + 1] = tmp;
        }
    }

    // ---------------------------------------------------------------
    // MERGE SORT
    // ---------------------------------------------------------------

    public static void mergesort(Transacao[] array, int esq, int dir)
    {
        if (esq < dir)
        {
            int meio = (esq + dir) / 2;
            mergesort(array, esq, meio);
            mergesort(array, meio + 1, dir);
            intercalar(array, esq, meio, dir);
        }
    }

    static void intercalar(Transacao[] array, int esq, int meio, int dir)
    {
        int nL = meio - esq + 1;
        int nR = dir - meio;
        Transacao[] L = new Transacao[nL];
        Transacao[] R = new Transacao[nR];

        for (int k = 0; k < nL; k++) L[k] = array[esq + k];
        for (int k = 0; k < nR; k++) R[k] = array[meio + 1 + k];

        int iL = 0, iR = 0, i = esq;
        while (iL < nL && iR < nR)
        {
            if (L[iL].Valor <= R[iR].Valor)
                array[i++] = L[iL++];
            else
                array[i++] = R[iR++];
        }

        while (iL < nL) array[i++] = L[iL++];
        while (iR < nR) array[i++] = R[iR++];
    }

    // ---------------------------------------------------------------
    // QUICKSORT (pivo = elemento do meio, como nos slides)
    // ---------------------------------------------------------------

    public static void quicksort(Transacao[] array, int esq, int dir)
    {
        int i = esq, j = dir;
        decimal pivo = array[(esq + dir) / 2].Valor;

        while (i <= j)
        {
            while (array[i].Valor < pivo) i++;
            while (array[j].Valor > pivo) j--;

            if (i <= j)
            {
                Transacao temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }
        }

        if (esq < j)
            quicksort(array, esq, j);
        if (i < dir)
            quicksort(array, i, dir);
    }

    public static bool estaOrdenado(Transacao[] array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i - 1].Valor > array[i].Valor)
                return false;
        }
        return true;
    }
}
