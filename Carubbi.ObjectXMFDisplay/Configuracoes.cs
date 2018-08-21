using Carubbi.ObjectXMFDisplay.Config;
using Carubbi.Utils.Persistence;
using System;
using System.IO;
using System.Windows.Forms;

namespace Carubbi.ObjectXMFDisplay
{
    /// <summary>
    /// Tela de configuração do simulador para indicar a pasta onde se encontram os arquivos .mfc que possuem as telas e as instruções de navegação entre elas
    /// <seealso />
    /// </summary>
    public partial class TelaConfiguracoes : Form
    {
        private int _contadorErro;

        /// <inheritdoc />
        public TelaConfiguracoes()
        {
            InitializeComponent();
        }

        public BaseMf Configuracoes { get; set; }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = txtPasta.Text;
            openFileDialog.ShowDialog(this);
            txtPasta.Text = openFileDialog.FileName;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Config_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO: Validar se arquivo .mfc é válido
            if (_contadorErro <= 3 && !File.Exists(txtPasta.Text))
            {
                MessageBox.Show(@"Por favor, selecione um arquivo válido de configurações do Mock MF.", @"Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                _contadorErro++;
                return;
            }

            if (_contadorErro > 3)
            {
                e.Cancel = false;
                Application.Exit();
                return;
            }

            var fInfo = new FileInfo(txtPasta.Text);
            Configuracoes.CaminhoArquivo = fInfo.DirectoryName;
            Configuracoes.TelaInicial = Path.GetFileNameWithoutExtension(txtPasta.Text);
            var baseMfSerializer =new Serializer<BaseMf>();
            if (checkSalvar.Checked)
                File.WriteAllText($"{Application.LocalUserAppDataPath}\\{Configuracoes.Nome}.xml", baseMfSerializer.XmlSerialize(Configuracoes));
        }

        private void TelaConfiguracoes_Load(object sender, EventArgs e)
        {
            Text = $@"{Text} - {Configuracoes.Nome}";
        }

       
    }
}
