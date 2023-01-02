using AppBancoWinForms.Entities.Enums;
using System;
using System.IO;

namespace AppBancoWinForms.Entities
{
    class ContaInvestimento : Conta
    {
        // public PerfilInvestidor Perfil { get; set; }

        public Acoes Investir { get; set; }
        public double ValorInvestido { get; set; }
        public ContaInvestimento(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo, DateTime dataCriacao) : base(numeroConta, tipoConta, numeroCliente, saldo, dataCriacao)
        {

        }

        public void InvestirAcoes(Acoes investir, double valorInvestido)
        {
            Investir = investir;
            ValorInvestido = valorInvestido;
            Saldo += valorInvestido + 3;
        }

        //public override double CalcularValorTarifa(double valor)
        //{
        //    return 0.08 * valor;
        //}


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
