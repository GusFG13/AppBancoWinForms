using AppBancoWinForms.Entities.Enums;
using System;

namespace AppBancoWinForms.Entities
{
    public class ContaPoupanca : Conta
    {
        public ContaPoupanca(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo, DateTime dataCriacao) : base(numeroConta, tipoConta, numeroCliente, saldo, dataCriacao)
        {

        }

        public override double CalcularValorTarifa(double valor)
        {
            return TaxasAdmin.TxContaPoupanca * valor;
        }
        public override string ToString()
        {
            return NumeroConta 
                + ";" + TipoConta.ToString() 
                + ";" + NumeroCliente 
                + ";" + Saldo.ToString("F2") 
                + ";" + DataCriacao.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
