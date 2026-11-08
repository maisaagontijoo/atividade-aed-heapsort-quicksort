# Ordenação Interna: Heapsort e Quicksort (C#)

Algoritmos e Estruturas de Dados, PUC Minas Contagem, 2026/2.

Heapsort e Quicksort implementados como foi ensinado na disciplina:

- **Heapsort**: heap invertido usando as posições 1 a n do vetor, com os métodos `construir`, `reconstruir` e `getMaiorFilho`.
- **Quicksort**: pivô no elemento do meio.

O programa conta comparações e movimentações (cada `swap` conta 3 movimentações) e mede o tempo.

## Como executar

```bash
dotnet run
```

## Menu

1. **Exemplo passo a passo** com o vetor `[101, 115, 30, 63, 47, 20]`
2. **Digitar um vetor** e ver o passo a passo dele
3. **Comparativo de desempenho** com vetores aleatórios, crescentes e decrescentes (n = 1.000 até 1.000.000)
4. **Aplicação prática** com transações ordenadas por valor usando Insertion, Merge e Quick (n = 1.000 até 10.000.000) e as 10 maiores

## Integrantes

- Maísa Gontijo Oliveira, matrícula 907599
- Vinícius Gabriel Gontijo de Souza, matrícula 907459
