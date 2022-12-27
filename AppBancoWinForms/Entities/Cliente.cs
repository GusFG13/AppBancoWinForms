using System;
using System.Collections.Generic;
using AppBancoWinForms.Entities.Enums;
namespace AppBancoWinForms.Entities
{
    class Cliente
    {
        public int Codigo { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public PerfilInvestidor Perfil { get; set; }
        public string Senha { get; set; }


        public Cliente() { }

        public Cliente(int codigo, string cpf, string nome, string sobrenome, string senha)
        {
            Codigo = codigo;
            Cpf = cpf;
            Nome = nome;
            Sobrenome = sobrenome;
            Perfil = (PerfilInvestidor)0;
            Senha = senha;
        }

        public void MostrarDadosCliente()
        {

        }

        public override string ToString()
        {
            return Codigo + ";" + Cpf + ";" + Nome + ";" + Sobrenome + ";" + Perfil + ";" + Senha;
        }
    }
}
