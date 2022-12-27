using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBancoWinForms.Entities;

namespace AppBancoWinForms.Utils
{
    internal class Cadastros
    {
        public static Cliente CadastrarCliente(string path, string cpf, string nome, string sobrenome, string senha)
        {
            int novoCodigo = 1;
            // Descobrir próximo código para cadastro de novo cliente
            if (File.Exists(path))
            {
                novoCodigo = int.Parse(EscreverArquivosBD.LerUltimaLinha(path).Split(';')[0]) + 1;
            }
            Cliente novoCliente = new Cliente(novoCodigo, cpf, nome, sobrenome, senha);

            EscreverArquivosBD.EscreverNovoItem(path, novoCliente.ToString());
            return novoCliente;
        }


        public static int CadastrarConta(string path, string tipoConta, int numCliente, double saldo)
        {
            int novoCodigo = 1;
            // Descobrir próximo código para cadastro de novo cliente
            if (File.Exists(path))
            {
                novoCodigo = int.Parse(EscreverArquivosBD.LerUltimaLinha(path).Split(';')[0]) + 1;
            }

            string dados = novoCodigo + ";" + tipoConta + ";" + numCliente + ";" + saldo;
            EscreverArquivosBD.EscreverNovoItem(path, dados);
            return novoCodigo;
        }

    }
}
