using AppBancoWinForms.Entities.Enums;
using System;

namespace AppBancoWinForms.Entities
{
    internal class ContaSalario : Conta
    {
        public ContaSalario(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo, DateTime dataCriacao) : base(numeroConta, tipoConta, numeroCliente, saldo, dataCriacao)
        {

        }
    }
}
