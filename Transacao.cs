using System;

class Transacao
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public string Status { get; set; }

    public Transacao(int id, decimal valor, DateTime data, string status)
    {
        Id = id;
        Valor = valor;
        Data = data;
        Status = status;
    }

    public override string ToString()
    {
        return "Id " + Id + " | R$ " + Valor.ToString("N2") + " | " + Data.ToString("dd/MM/yyyy") + " | " + Status;
    }
}
