
namespace AppBancoWinForms.Entities
{
    internal class Holerite
    {
        public string Cnpj { get; set; }
        public string NomeFontePagadora { get; set; }

        public Holerite()
        {
        }

        public Holerite(string cnpj, string nomeFontePagadora)
        {
            Cnpj = cnpj;
            NomeFontePagadora = nomeFontePagadora;
        }
    }
}
