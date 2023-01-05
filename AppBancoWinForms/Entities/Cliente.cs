using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AppBancoWinForms.Entities.Enums;
namespace AppBancoWinForms.Entities
{
    public class Cliente
    {
        public int Codigo { get; protected set; }
        public string Cpf { get; protected set; }
        public string Nome { get; protected set; }
        public string Sobrenome { get; protected set; }
        public PerfilInvestidor Perfil { get; set; }
        private string Senha { get; set; }


        public Cliente() { }

        public Cliente(int codigo, string cpf, string nome, string sobrenome, PerfilInvestidor perfil, string senha)
        {
            Codigo = codigo;
            Cpf = cpf;
            Nome = nome;
            Sobrenome = sobrenome;
            Perfil = perfil;
            Senha = senha;
        }


        public void MostrarDadosCliente()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Dados do Cliente:");
            sb.AppendLine($"Nº Cadastro: {Codigo}");
            sb.AppendLine($"Nome Completo: {Nome} {Sobrenome}");
            sb.AppendLine($"CPF: {Cpf}");
            sb.AppendLine($"Perfil de Investimentos: {Perfil.ToString()}");
            sb.Append($"Senha cadastrada: {Senha}");

            MessageBox.Show(sb.ToString(), "Info. Cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        public void EditarCadastroCliente(string path)
        {
            if (File.Exists(path))
            {
                string[] clientesCSV = File.ReadAllLines(path);
                for (int i = 0; i < clientesCSV.Length; i++)
                {
                    if (int.Parse(clientesCSV[i].Split(';')[0]) == Codigo)
                    {
                        clientesCSV[i] = ToString();
                        break;
                    }
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < clientesCSV.Length; i++)
                {
                    sb.AppendLine(clientesCSV[i]);
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(sb.ToString());
                }
            }
        }

        public override string ToString()
        {
            return Codigo + ";" + Cpf + ";" + Nome + ";" + Sobrenome + ";" + Perfil + ";" + Senha;
        }
    }
}
