using System.Globalization;

namespace Questao1;

class ContaBancaria
{
    public int Numero { get; }
    public string Titular { get; set; }
    public double Saldo { get; private set; }

    private const double TaxaDeSaque = 3.50;

    public ContaBancaria(int numero, string titular, double depositoInicial = 0.0)
    {
        Numero = numero;
        Titular = titular;
        Saldo = depositoInicial;
    }

    public void Deposito(double quantia)
    {
        Saldo += quantia;
    }

    public void Saque(double quantia)
    {
        Saldo -= quantia + TaxaDeSaque;
    }

    public override string ToString()
    {
        return "Conta " + Numero + ", Titular: " + Titular + ", Saldo: $ " + Saldo.ToString("F2", CultureInfo.InvariantCulture);
    }
}