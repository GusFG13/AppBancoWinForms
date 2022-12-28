using AppBancoWinForms.Entities.Enums;
using System;

namespace AppBancoWinForms.Entities
{
    class ContaInvestimento : Conta
    {
        // public PerfilInvestidor Perfil { get; set; }
        public ContaInvestimento(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo, DateTime dataCriacao) : base(numeroConta, tipoConta, numeroCliente, saldo, dataCriacao)
        {

        }
    }
}
