using AppBancoWinForms.Entities.Enums;

namespace AppBancoWinForms.Entities
{
    internal class ContaSalario : Conta
    {
        public ContaSalario(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo) : base(numeroConta, tipoConta, numeroCliente, saldo)
        {

        }
    }
}
