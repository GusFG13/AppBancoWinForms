using AppBancoWinForms.Entities.Enums;

namespace AppBancoWinForms.Entities
{
    class ContaInvestimento : Conta
    {
        // public PerfilInvestidor Perfil { get; set; }
        public ContaInvestimento(int numeroConta, TipoConta tipoConta, int numeroCliente, double saldo) : base(numeroConta, tipoConta, numeroCliente, saldo)
        {

        }
    }
}
