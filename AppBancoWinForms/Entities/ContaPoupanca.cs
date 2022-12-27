using AppBancoWinForms.Entities.Enums;

namespace AppBancoWinForms.Entities
{
    class ContaPoupanca : Conta
    {
        public ContaPoupanca(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo) : base(numeroConta, tipoConta, numeroCliente, saldo)
        {

        }
    }
}
