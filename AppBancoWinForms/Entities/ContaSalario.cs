using AppBancoWinForms.Entities.Enums;
using System;

namespace AppBancoWinForms.Entities
{
    internal class ContaSalario : Conta
    {
        Holerite Holerite { get; set; }

        public ContaSalario(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo, DateTime dataCriacao, Holerite holerite)
            : base(numeroConta, tipoConta, numeroCliente, saldo, dataCriacao)
        {
            Holerite = holerite;
        }
    }
}
