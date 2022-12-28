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
using System.IO;

namespace AppBancoWinForms
{
    public partial class Form1 : Form
    {
        Cliente cliente = new Cliente();
        string pathClientes = @"c:\DadosAppBanco\clientes.csv";
        string pathContas = @"c:\DadosAppBanco\contas.csv";
        string pathTransacoes = @"c:\DadosAppBanco\trasacoes.csv";

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            //this.MinimizeBox = false;
            linklblErroLogin.Text = "";

            // Esconde abas do tabCtrlTipoConta (na aba Abrir Nova Conta de tabCtrlTelasApp)
            //tabCtrlTipoConta.Appearance = TabAppearance.FlatButtons;
            //tabCtrlTipoConta.ItemSize = new Size(0, 1);
            //tabCtrlTipoConta.SizeMode = TabSizeMode.Fixed;

            // Esconde abas do tabCtrlTelasApp (tela principal)
            //tabCtrlTelasApp.Appearance = TabAppearance.FlatButtons;
            //tabCtrlTelasApp.ItemSize = new Size(0, 1);
            //tabCtrlTelasApp.SizeMode = TabSizeMode.Fixed;
        }

        private void btEnter_Click(object sender, EventArgs e)
        {

            string numerosCPF;
            mtbCPFLogin.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            numerosCPF = mtbCPFLogin.Text;
            if (numerosCPF.Length == 11 || double.TryParse(numerosCPF, out _))
            {
                mtbCPFLogin.TextMaskFormat = MaskFormat.IncludeLiterals;
                //string path = @"c:\DadosAppBanco\clientes.csv";
                string dadosCliente = EscreverArquivosBD.RetornarDadosCliente(pathClientes, mtbCPFLogin.Text);
                if (dadosCliente != "")
                {
                    string[] aux = dadosCliente.Split(';');
                    if (aux[5] == mtbSenhaLogin.Text)
                    {
                        cliente.Codigo = int.Parse(aux[0]);
                        cliente.Cpf = aux[1];
                        cliente.Nome = aux[2];
                        cliente.Sobrenome = aux[3];
                        cliente.Perfil = (PerfilInvestidor)Enum.Parse(typeof(PerfilInvestidor), aux[4]);
                        tabCtrlTelasApp.SelectedTab = tabPage2; // ir para a página de seleção de conta
                    }
                    else
                    {
                        linklblErroLogin.Text = "Senha incorreta";
                        mtbSenhaLogin.Focus();
                    }
                }
                else
                {
                    linklblErroLogin.Text = "CPF não encontrado no cadastro";
                    mtbCPFLogin.Focus();
                }
            }
            else
            {
                mtbCPFLogin.Focus();
                linklblErroLogin.Text = "Formato do CPF incorreto";
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



                mtbCPFCadastro.TextMaskFormat = MaskFormat.IncludeLiterals;
                string cpf = mtbCPFCadastro.Text;
                string nome = tbNomeCadastro.Text;
                string sobrenome = tbSobrenomeCadastro.Text;
                string senha = mtbSenhaCadastro.Text;


                // cadastrar novo
                if (EscreverArquivosBD.ProcurarCliente(pathClientes, cpf))
                {
                    MessageBox.Show("CPF já cadastrado!", "Falha no Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Console.WriteLine("CPF já cadastrado");
                }
                else
                {
                    Cliente cliente = Cadastros.CadastrarCliente(pathClientes, cpf, nome, sobrenome, senha);
                    MessageBox.Show("Novo cadastro realizado", "Cadastro concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Console.WriteLine("NOVO CADASTRO: " + cliente.ToString());
                    tabCtrlTelasApp.SelectedTab = tabPage7; // Ir para a tela de criação de conta
                    btVoltarParaMenu.Visible = false;
                }
            }
        }

        private void btNovoCadastro_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPage6;
            mtbCPFCadastro.Focus();
        }

        private void btExtratoPoupanca_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPage8;
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

        /******************** Seleção tipo de conta a ser criada ********************/
        private void rbPoupanca_CheckedChanged(object sender, EventArgs e)
        {
            tabCtrlTipoConta.SelectedTab = tabPagePoupanca;
        }

        private void rbSalario_CheckedChanged(object sender, EventArgs e)
        {
            tabCtrlTipoConta.SelectedTab = tabPageSalario;
        }

        private void rbInvestimento_CheckedChanged(object sender, EventArgs e)
        {
            tabCtrlTipoConta.SelectedTab = tabPageInvestimento;
        }
        /**************************************************************************/
        private void btExtratoPoupanca_Click_1(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPage8;
        }

        private void cbContaSelecionada_SelectedIndexChanged(object sender, EventArgs e)
        {

            rbDepositar.Checked = false;
            rbDepositar.Enabled = true;
            rbSacar.Checked = false;
            rbSacar.Enabled = true;
            rbTransferenciaPoup.Checked = false;
            rbTransferenciaPoup.Enabled = true;
            rbDepositarSalario.Checked = false;
            rbDepositarSalario.Enabled = true;
            rbInvestir.Checked = false;
            rbInvestir.Enabled = true;

            string tipoConta = cbContaSelecionada.Text.Split('-')[1].Trim();
            lblDadosConta.Text = cbContaSelecionada.Text.Split('-')[0].Trim() + "\n" + tipoConta;
            /******************************************************************************/
            // Mudar Switch para ler enums
            /******************************************************************************/
            switch (tipoConta)
            {
                case "Conta Poupança":
                    rbTransferenciaPoup.Enabled = false;
                    rbDepositarSalario.Enabled = false;
                    //tabControl3.SelectedTab = tabPoupanca;
                    break;
                case "Conta Salário":
                    rbDepositar.Enabled = false;
                    //tabControl3.SelectedTab = tabSalario;
                    break;
                case "Conta Investimento":
                    rbInvestir.Enabled = false;
                    rbDepositarSalario.Enabled = false;
                    //tabControl3.SelectedTab = tabInvestimento;
                    break;
                default: break;
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            DateTime agora = DateTime.Now;
            string msg = "";
            if (agora.Hour < 12)
                msg = "Bom dia, ";
            else if (agora.Hour < 18)
                msg = "Boa Tarde, ";
            else
                msg = "Boa noite, ";
            msg += cliente.Nome + "! Selecione sua conta:";
            lblSelecioneConta.Text = msg;
            //lblDadosConta.Text = cliente.Nome;
            cbContaSelecionada.Items.Clear();
            //cbContaSelecionada.Items.Add("0101 - Conta Poupança");
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
                //cbContaSelecionada.Enabled = false;
                tabCtrlTelasApp.SelectedTab = tabPage7;
            }
        }

        private void btVoltar_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPage2;
        }

        private void btVoltarParaMenu_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPage2;
        }

        private void btAbrirNovaConta_Click(object sender, EventArgs e)
        {
            tabCtrlTelasApp.SelectedTab = tabPage7;
            btVoltarParaMenu.Visible = true;
        }

        private void btCadastrarNovaConta_Click(object sender, EventArgs e)
        {
            //Fazer validações
            bool requisitosOK = true;
            double saldoInicial = 0.0;
            TipoConta tConta = TipoConta.ContaPoupanca;
            if (rbPoupanca.Checked)
            {
                tConta = TipoConta.ContaPoupanca;
                
                if (tbDepInicialPoup.Text.Contains(".") || !double.TryParse(tbDepInicialPoup.Text, out saldoInicial) || saldoInicial < 500.00)
                {
                    requisitosOK = false;
                }
            } 
            else if (rbSalario.Checked)
            { // desenvolver validações
                tConta = TipoConta.ContaSalario;
            }
            else if (rbInvestimento.Checked)
            {// desenvolver validações
                tConta = TipoConta.ContaInvestimento;
            }
            if (requisitosOK)
            {
                Cadastros.CadastrarConta(pathContas, tConta, cliente.Codigo, saldoInicial, DateTime.UtcNow);
                tabCtrlTelasApp.SelectedTab = tabPage2;
            }

        }
    }
}
