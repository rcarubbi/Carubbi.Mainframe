using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Carubbi.Extensions;
using Carubbi.Mainframe.Config;
using Carubbi.Mainframe.Exeptions;
using Carubbi.ObjectXMFDisplay;
using Carubbi.ServiceLocator;

namespace Carubbi.Mainframe
{

    /// <summary>
    /// Controle de Fachada utilizado como interface entre a aplicação cliente e o componente de emulação mainframe (que pode ser o MicroFocus Rumba ou o Simulador de emulação Carubbi)
    /// </summary>
    public partial class MfDisplay : UserControl, IDisposable
    {
        #region Variáveis Privadas
        private Size BASE_SIZE = new Size { Width = 480, Height = 320 };
        private const int TIMEOUT = 4000;
        private const int SLEEP = 50;
        private DateTime _startTime = DateTime.Now;
        private string _lastScreen = "";
        private MainframeConfigElement _terminalConfig = new MainframeConfigElement();
        private const string DEFAULT_MAINFRAMECONFIG = "Mainframe.config";
        #endregion

        #region Construtores

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadConfig();
        }


        /// <inheritdoc />
        public MfDisplay()
        {
            InitializeComponent();

            var resources = new ComponentResourceManager(typeof(MfDisplay));

            objXMFDisplay = IsInDesignMode ? new AxObjectXMFDisplay() : ImplementationResolver.Resolve("AxObjectXMFDisplay");

            ((ISupportInitialize)(objXMFDisplay)).BeginInit();

            SuspendLayout();
            // 
            // objXMFDisplay
            // 
            if (IsInDesignMode)
            {
                var control = ((AxObjectXMFDisplay)objXMFDisplay);
                control.Dock = DockStyle.Fill;
                control.Enabled = true;
                control.Location = new Point(0, 0);
                control.Name = "objXMFDisplay";
                control.OcxState = ((AxHost.State)(resources.GetObject("objXMFDisplay.OcxState")));
                control.Size = new Size(480, 320);
                control.TabIndex = 0;
            }
            else
            {
                objXMFDisplay.Call("CreateControl");
                objXMFDisplay.SetProperty("Dock", DockStyle.Fill);
                objXMFDisplay.SetProperty("Enabled", true);
                objXMFDisplay.SetProperty("Location", new Point(0, 0));
                objXMFDisplay.SetProperty("Name", "objXMFDisplay");
                objXMFDisplay.SetProperty("OcxState", ((AxHost.State)(resources.GetObject("objXMFDisplay.OcxState"))));
                objXMFDisplay.SetProperty("Size", new Size(480, 320));
                objXMFDisplay.SetProperty("TabIndex", 0);
                objXMFDisplay.SetProperty("EventVersion", 1);
                objXMFDisplay.SetProperty("CharacterSetID2", 32);
                objXMFDisplay.SetProperty("FontTypeFace", "Term3270");
                objXMFDisplay.SetProperty("AutoFont", true);
                objXMFDisplay.SetProperty("AutoFontMinimumWidth", 2);
                objXMFDisplay.SetProperty("CharacterCase", false);
                objXMFDisplay.SetProperty("ClipboardConfiguration", 0);
                objXMFDisplay.SetProperty("CursorBlinkRate", 1000);
                objXMFDisplay.SetProperty("ShowLightPenCursor", true);
                objXMFDisplay.SetProperty("WatermarkConfiguration", 0);
            }

            Controls.Add(objXMFDisplay as Control ?? throw new InvalidOperationException());
            ((ISupportInitialize)(objXMFDisplay)).EndInit();
            ResumeLayout(false);
        }
        #endregion

        #region Propriedades
        public bool IsInDesignMode => DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        private MainframeConfigElement TerminalConfig
        {
            get
            {
                var section = (MainframeConfigSection)ConfigurationManager.GetSection("Mainframe");

                foreach (MainframeConfigElement terminal in section.MainframeTerminals)
                {
                    if (terminal.Nome != Name) continue;
                    _terminalConfig = terminal;
                    break;
                }

                return _terminalConfig;
            }
        }

        public string ConfigName { get; set; }

        public bool HostConnected => objXMFDisplay.GetProperty<bool>("HostConnected");

        public int CursorRow
        {
            get => objXMFDisplay.GetProperty<int>("CursorRow");
            set => objXMFDisplay.SetProperty("CursorRow", value);
        }

        public int CursorColumn
        {
            get => objXMFDisplay.GetProperty<int>("CursorColumn");
            set => objXMFDisplay.SetProperty("CursorColumn", value);
        }

        #endregion

        #region Métodos Privados

        public void LoadConfig()
        {
            ConfigName = Name;
            objXMFDisplay.Call("LoadConfig", ConfigName);
        }

        private static void ConfigureDisplay()
        {

        }

        void IDisposable.Dispose()
        {
            Disconnect();
        }

        private void SetStart()
        {
            _startTime = DateTime.Now;
        }

        private bool CheckTimeout(int timeout)
        {
            return _startTime.AddMilliseconds(timeout > 0 ? timeout : TIMEOUT) < DateTime.Now;
        }

        private bool CheckScreenChange()
        {
            return _lastScreen.Trim() != GetScreen().Trim();
        }

        private void SetLastScreen()
        {
            Sleep(SLEEP);
            Application.DoEvents();
            _lastScreen = GetScreen();
        }

        private void Sleep(int milisecondsTimeOut)
        {
            Thread.Sleep(milisecondsTimeOut);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Aguarda até que uma alteração ocorra na tela ou até que o tempo de espera seja atingido
        /// </summary>
        public void WaitForChanges()
        {
            SetStart();
            while (!CheckTimeout(0) && !CheckScreenChange())
            {
                Sleep(SLEEP);
                Application.DoEvents();
            }
            if (!CheckScreenChange())
            {
                throw new Exception("Timeout");
            }
            SetLastScreen();
        }

        private bool _firstConenction = true;

        /// <summary>
        /// Efetua a conexão com o terminal mainframe
        /// </summary>
        public void Connect()
        {
            LoadConfig();
            if (objXMFDisplay.GetProperty<bool>("HostConnected"))
            {
                objXMFDisplay.Call("Disconnect");
                Sleep(SLEEP);
            }

            objXMFDisplay.SetProperty("HostInterfaceConfiguration", 15);

            if (_firstConenction)
            {
                var t = new Thread(ConfigureDisplay);
                t.Start();
                _firstConenction = false;
            }
            objXMFDisplay.Call("Connect");

            SetStart();
            while (!CheckTimeout(16000) && !objXMFDisplay.GetProperty<bool>("HostConnected"))
            {
                WaitForChanges();
                Sleep(SLEEP);
                Application.DoEvents();
            }
            if (!objXMFDisplay.GetProperty<bool>("HostConnected"))
            {
                throw new Exception("Timeout");
            }
            SetLastScreen();

        }

        /// <summary>
        /// Desconecta do terminal
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (objXMFDisplay.GetProperty<bool>("HostConnected"))
                    objXMFDisplay.Call("Disconnect");
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        /// Verifica se um determinado texto se encontra em uma coordenada na tela atual
        /// </summary>
        /// <param name="text"></param>
        /// <param name="row"></param>
        /// <param name="colum"></param>
        /// <returns></returns>
        public bool CompareText(string text, int row, int colum)
        {
            return string.Equals(text, GetScreen(row, colum, text.Length), StringComparison.CurrentCultureIgnoreCase);
        }


        /// <summary>
        /// Retorna o texto da tela corrente
        /// </summary>
        /// <returns></returns>
        public string GetScreen()
        {
            return GetScreen(1, 1, 1920);
        }


        /// <summary>
        /// Retorna parte do texto na tela corrente a partir de uma coordenada e do tamanho do trecho do texto
        /// </summary>
        /// <param name="row">Coordenada no eixo Y</param>
        /// <param name="column">Coordenada no eixo X</param>
        /// <param name="length">Tamanho do trecho a ser recuperado</param>
        /// <returns>Texto encontrado</returns>
        public string GetScreen(int row, int column, int length)
        {
            return objXMFDisplay.Call<string>("GetScreen2", row, column, length).Replace("\0", " ");
        }

        /// <summary>
        /// Insere um texto a partir de uma determinada coordenada na tela corrente
        /// </summary>
        /// <param name="text">Texto a ser inserido</param>
        /// <param name="row">Coordenada no eixo Y</param>
        /// <param name="column">Coordenada no eixo X</param>
        public void PutScreen(string text, int row, int column)
        {
            objXMFDisplay.Call("PutScreen2", text, row, column);
            SetLastScreen();
        }

        /// <summary>
        /// Insere uma data na tela corrente a partir de uma coordenada
        /// </summary>
        /// <param name="data">Data a ser inserida</param>
        /// <param name="row">Coordenada no eixo Y</param>
        /// <param name="column">Coordenada no eixo X</param>
        public void PutScreen(DateTime data, int row, int column)
        {
            objXMFDisplay.Call("PutScreen2", data.Day.ToString("00"), row, column);
            objXMFDisplay.Call("PutScreen2", data.Month.ToString("00"), row, column + 5);
            objXMFDisplay.Call("PutScreen2", data.Year.ToString("0000").Substring(2, 2), row, column + 10);
            SetLastScreen();
        }

        /// <summary>
        /// Limpa um determinado espaço na tela antes de inserir um novo texto
        /// </summary>
        /// <param name="text"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="size"></param>
        public void PutScreen(string text, int row, int column, int size)
        {
            objXMFDisplay.Call("PutScreen2", new String(' ', size), row, column);
            PutScreen(text, row, column);
        }


        /// <summary>
        /// Insere um texto na tela corrente a partir do próximo espaço editável disponível
        /// </summary>
        /// <param name="text">Texto a ser inserido</param>
        public void PutScreen(string text)
        {
            objXMFDisplay.Call("SendKeys", text);
            SetLastScreen();
        }

        /// <summary>
        /// Envia uma tecla para o emulador
        /// </summary>
        /// <param name="key">Tecla a ser enviada</param>
        public void SendKey(ConsoleKey key)
        {
            SetLastScreen();
            objXMFDisplay.Call("SendKeys", key.ToComandText());
        }

        /// <summary>
        /// Verifica se um determinado texto se encontra em uma posição durante um período de tempo com base nas coordenadas configuradas em uma chave do arquivo de configurações do emulador de terminal mainframe
        /// </summary>
        /// <param name="chaveTexto">Texto a ser procurado</param>
        /// <param name="chavePosicao">Chave das coordenadas no arquivo de configurações do emulador de terminal</param>
        /// <param name="mensagemErro">Mensagem de erro que deve ser retornada na exceção caso a tela não encontre o texto procurado</param>
        public void ValidaPassoOk(string chaveTexto, string chavePosicao, string mensagemErro)
        {
            ValidaPassoOk(chaveTexto, chavePosicao, mensagemErro, 0);
        }

        /// <summary>
        /// Verifica se um determinado texto se encontra em uma posição durante um período de tempo com base nas coordenadas configuradas em uma chave do arquivo de configurações do emulador de terminal mainframe 
        /// </summary>
        /// <param name="chaveTexto">Texto a ser procurado</param>
        /// <param name="chavePosicao">Chave das coordenadas no arquivo de configurações do emulador de terminal</param>
        /// <param name="mensagemErro">Mensagem de erro que deve ser retornada na exceção caso a tela não encontre o texto procurado</param>
        /// <param name="timeout">Tempo de espera em milisegundos</param>
        public void ValidaPassoOk(string chaveTexto, string chavePosicao, string mensagemErro, int timeout)
        {
            if (!VerificaPassoAtual(chaveTexto, chavePosicao, timeout))
            {
                throw new UnexpectedScreenException(mensagemErro, GetScreen());
            }
        }

        public string GetEditable()
        {
            try
            {
                int row;
                for (row = 1; row <= 24; row++)
                {
                    int column;
                    for (column = 1; column < 80; column++)
                    {
                        PutScreen("@", row, column);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return GetScreen().Replace("\t", " ");
        }


        public bool VerificaPassoAtual(string chaveTexto, string chavePosicao, int timeout)
        {
            if (!TerminalConfig.Settings.AllKeys.Contains(chaveTexto)) return false;
            var posicao = TerminalConfig.GetPositionSetting(chavePosicao);
            SetStart();
            while (!CheckTimeout(timeout) && !CompareText(TerminalConfig.Settings[chaveTexto].Value, posicao.Y, posicao.X))
            {
                Sleep(SLEEP);
                Application.DoEvents();
            }
            return CompareText(TerminalConfig.Settings[chaveTexto].Value, posicao.Y, posicao.X);
        }
        #endregion

    }
}
