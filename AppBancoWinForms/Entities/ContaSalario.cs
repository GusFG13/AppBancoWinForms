using AppBancoWinForms.Entities.Enums;
using System;

namespace AppBancoWinForms.Entities
{
    internal class ContaSalario : Conta
    {
        public Holerite Holerite { get; set; }

        public ContaSalario(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo, DateTime dataCriacao, Holerite holerite)
            : base(numeroConta, tipoConta, numeroCliente, saldo, dataCriacao)
        {
            Holerite = holerite;
        }

        //public override double CalcularValorTarifa(double valor)
        //{
        //    return 0.03 * valor;
        //}
        public override string ToString()
        {
            return NumeroConta
                + ";" + TipoConta.ToString()
                + ";" + NumeroCliente
                + ";" + Saldo.ToString("F2")
                + ";" + DataCriacao.ToString("yyyy-MM-ddTHH:mm:ssZ")
                + ";" + Holerite.Cnpj
                + ";" + Holerite.NomeFontePagadora;
        }
    }
}
