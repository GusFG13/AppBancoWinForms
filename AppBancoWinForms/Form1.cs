using AppBancoWinForms.Entities;
using AppBancoWinForms.Utils;
using AppBancoWinForms.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace AppBancoWinForms
{
    public partial class Form1 : Form
    {
        Cliente cliente = new Cliente();

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            //this.MinimizeBox = false;
            //foreach (Control ctr in tabPage1.Controls)
            //{
            //    ctr.TabStop = false;
            //}
            //foreach (Control ctr in groupBox1.Controls)
            //{
            //    ctr.TabStop = false;
            //}
            linkLabel2.Text = "";
            cbContaSelecionada.Items.Add("0101 - Conta Poupança");
            cbContaSelecionada.Items.Add("0127 - Conta Investimento");
            cbContaSelecionada.Items.Add("0243 - Conta Salário");
            cbContaSelecionada.Items.Add("0574 - Conta Poupança");
            if (cbContaSelecionada.Items.Count > 0)
            {
                //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                cbContaSelecionada.SelectedIndex = 0;
            }
            else
            {
                //cbContaSelecionada.Visible = false;
                cbContaSelecionada.Enabled = false;
            }
        }

        private void btSelecionarConta_Click(object sender, EventArgs e)
        {

        }

        private void btEnter_Click(object sender, EventArgs e)
        {

            string numerosCPF;
            mtbCPFLogin.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            numerosCPF = mtbCPFLogin.Text;
            if (numerosCPF.Length == 11 || double.TryParse(numerosCPF, out _))
            {
                mtbCPFLogin.TextMaskFormat = MaskFormat.IncludeLiterals;
                string path = @"c:\DadosAppBanco\clientes.csv";
                string dadosCliente = EscreverArquivosBD.RetornarDadosCliente(path, mtbCPFLogin.Text);
                if (dadosCliente != "")
                {
                    string[] aux = dadosCliente.Split(';');
                    if (aux[5] == mtbSenhaLogin.Text) 
                    {
                        cliente.Codigo = int.Parse(aux[0]);
                        cliente.Cpf = aux[1];
                        cliente.Nome = aux[2];
                        cliente.Sobrenome = aux[3];
                        cliente.Perfil = (PerfilInvestidor)Enum.Parse(typeof(PerfilInvestidor),aux[4]);
                        tabControl1.SelectedTab = tabPage2; // ir para a página de seleção de conta
                    }
                    else
                    {
                        linkLabel2.Text = "Senha incorreta";
                        mtbSenhaLogin.Focus();
                    }
                }
                else
                {
                    linkLabel2.Text = "CPF não encontrado no cadastro";
                    mtbCPFLogin.Focus();
                }
            }
            else
            {
                mtbCPFLogin.Focus();
                linkLabel2.Text = "Formato do CPF incorreto";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                mtbSenhaCadastro.UseSystemPasswordChar = false;
                mtbSenhaCadastroRepetir.UseSystemPasswordChar = false;
            }
            else
            {
                mtbSenhaCadastro.UseSystemPasswordChar = true;
                mtbSenhaCadastroRepetir.UseSystemPasswordChar = true;
            }

        }

        private void btCadastrar_Click(object sender, EventArgs e)
        {
            bool dadosOK = true;
            string numerosCPF;
            mtbCPFCadastro.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            numerosCPF = mtbCPFCadastro.Text;
            if (numerosCPF.Length != 11 || !double.TryParse(numerosCPF, out _)) // melhorar validação
            {
                dadosOK = false;
                MessageBox.Show("Porfavor, preencha o campo CPF corretamente.", "CPF Inválido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mtbCPFCadastro.Focus();
            }
            else if (String.IsNullOrWhiteSpace(tbNomeCadastro.Text))
            {
                dadosOK = false;
                tbNomeCadastro.Focus();
            }
            else if (String.IsNullOrWhiteSpace(tbSobrenomeCadastro.Text))
            {
                dadosOK = false;
                tbSobrenomeCadastro.Focus();
            }
            else if (mtbSenhaCadastro.Text.Length < 6)
            {
                dadosOK = false;
                mtbSenhaCadastro.Focus();
                MessageBox.Show("Senha deve ter 6 números", "Senha Inválida", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (mtbSenhaCadastro.Text != mtbSenhaCadastroRepetir.Text)
                {
                    dadosOK = false;
                    mtbSenhaCadastro.Focus();
                    MessageBox.Show("Campos senhas não coincidem", "Senhas Diferentes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (dadosOK) // Gravar novo usuário no arquivo c:\DadosAppBanco\clientes.csv
            {

                string path = @"c:\DadosAppBanco\clientes.csv";
                //string path = @"c:\DadosAppBanco\contas.csv";
                //string path = @"c:\DadosAppBanco\trasacoes.csv";

                mtbCPFCadastro.TextMaskFormat = MaskFormat.IncludeLiterals;
                string cpf = mtbCPFCadastro.Text;
                string nome = tbNomeCadastro.Text;
                string sobrenome = tbSobrenomeCadastro.Text;
                string senha = mtbSenhaCadastro.Text;


                // cadastrar novo
                if (EscreverArquivosBD.ProcurarCliente(path, cpf))
                {
                    MessageBox.Show("CPF já cadastrado!", "Falha no Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Console.WriteLine("CPF já cadastrado");
                }
                else
                {
                    Cliente cliente = Cadastros.CadastrarCliente(path, cpf, nome, sobrenome, senha);
                    MessageBox.Show("Novo cadastro realizado", "Cadastro concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Console.WriteLine("NOVO CADASTRO: " + cliente.ToString());
                    tabControl1.SelectedTab = tabPage7; // Ir para a tela de criação de conta
                }
            }
        }

        private void btNovoCadastro_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage6;
            mtbCPFCadastro.Focus();
        }

        private void btExtratoPoupanca_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage8;
        }

        private void tabPage8_Enter(object sender, EventArgs e)
        {
            rb7dias.Checked = true;
        }

        private void rb7dias_CheckedChanged(object sender, EventArgs e)
        {
            if (rb7dias.Checked)
            {
                DateTime hoje = DateTime.Now;
                dtpInicio.ValueChanged -= dtpInicio_ValueChanged;
                dtpInicio.Value = hoje.AddDays(-7);
                dtpInicio.ValueChanged += dtpInicio_ValueChanged;
                dtpFim.ValueChanged -= dtpFim_ValueChanged;
                dtpFim.Value = hoje;
                dtpFim.ValueChanged += dtpFim_ValueChanged;
                tbExtrato.Text = "";
            }
        }

        private void rb15dias_CheckedChanged(object sender, EventArgs e)
        {
            if (rb15dias.Checked)
            {
                DateTime hoje = DateTime.Now;
                dtpInicio.ValueChanged -= dtpInicio_ValueChanged;
                dtpInicio.Value = hoje.AddDays(-15);
                dtpInicio.ValueChanged += dtpInicio_ValueChanged;
                dtpFim.ValueChanged -= dtpFim_ValueChanged;
                dtpFim.Value = hoje;
                dtpFim.ValueChanged += dtpFim_ValueChanged;
                tbExtrato.Text = "";
            }
        }

        private void rb30dias_CheckedChanged(object sender, EventArgs e)
        {
            if (rb30dias.Checked)
            {
                DateTime hoje = DateTime.Now;
                dtpInicio.ValueChanged -= dtpInicio_ValueChanged;
                dtpInicio.Value = hoje.AddDays(-30);
                dtpInicio.ValueChanged += dtpInicio_ValueChanged;
                dtpFim.ValueChanged -= dtpFim_ValueChanged;
                dtpFim.Value = hoje;
                dtpFim.ValueChanged += dtpFim_ValueChanged;
                tbExtrato.Text = "";
            }
        }

        private void dtpInicio_ValueChanged(object sender, EventArgs e)
        {
            rb7dias.Checked = false;
            rb15dias.Checked = false;
            rb30dias.Checked = false;
            tbExtrato.Text = "";
        }

        private void dtpFim_ValueChanged(object sender, EventArgs e)
        {
            rb7dias.Checked = false;
            rb15dias.Checked = false;
            rb30dias.Checked = false;
            tbExtrato.Text = "";
        }

        private void btGerarextrato_Click(object sender, EventArgs e)
        {
            if (dtpInicio.Value > dtpFim.Value)
            {
                MessageBox.Show("Final do período escolhido anterior ao início.\nPor favor, escolher outro período.", "Período Inválido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpInicio.Focus();
            }
            else
            {
                tbExtrato.Text = $"Extrato. Período {dtpInicio.Value.ToString("dd/MM/yyyy")} até {dtpFim.Value.ToString("dd/MM/yyyy")}";
            }
        }

        private void btExportarExtrato_Click(object sender, EventArgs e)
        {
            if (tbExtrato.Text.Length == 0)
            {
                MessageBox.Show("Por favor, gerar o extrato para o período.", "Extrato Indisponível", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Implementar função.", "Exportar Extrato", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rbPoupanca_CheckedChanged(object sender, EventArgs e)
        {
            tabControl2.SelectedTab = tabPage9;
        }

        private void rbSalario_CheckedChanged(object sender, EventArgs e)
        {
            tabControl2.SelectedTab = tabPage10;
        }

        private void rbInvestimento_CheckedChanged(object sender, EventArgs e)
        {
            tabControl2.SelectedTab = tabPage11;
        }

        private void btExtratoPoupanca_Click_1(object sender, EventArgs e)
        {

        }

        private void cbContaSelecionada_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipoConta = cbContaSelecionada.Text.Split('-')[1].Trim();
            lblDadosConta.Text = cbContaSelecionada.Text.Split('-')[0].Trim() + "\n" + tipoConta;
            switch (tipoConta)
            {
                case "Conta Poupança":
                    tabControl3.SelectedTab = tabPoupanca;
                    break;
                case "Conta Salário":
                    tabControl3.SelectedTab = tabSalario;
                    break;
                case "Conta Investimento":
                    tabControl3.SelectedTab = tabInvestimento;
                    break;
                default: break;
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            lblDadosConta.Text = cliente.Nome;
        }
    }
}
