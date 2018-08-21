using Carubbi.Extensions;
using Carubbi.ObjectXMFDisplay.Config;
using Carubbi.ObjectXMFDisplay.Util;
using Carubbi.Utils.Persistence;
using Carubbi.WindowsAppHelper;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Carubbi.ObjectXMFDisplay
{
    /// <summary>
    /// Controle Simulador de Emulação de Terminal Mainframe baseado na interface do componente Microfocus Rumba OnWeb
    /// </summary>
    public partial class AxObjectXMFDisplay : UserControl, ISupportInitialize
    {
        private Size BASE_SIZE = new Size { Width = 480, Height = 320 };
        private Tela _telaAtual = new Tela();
        private Point _cursorPosition = new Point { X = 0, Y = 0 };
        private BaseMf _configuracoes = new BaseMf();

        public AxObjectXMFDisplay()
        {
            InitializeComponent();
           
        }

        #region ISupportInitialize
        public void BeginInit() {}
        public void EndInit() {}
        #endregion

        #region Variáveis privadas dos parâmetros

        private int _cursorColumn;
        private int _cursorRow;

        #endregion

        #region Eventos

        public delegate void AfterConnectDelegate(object sender, EventArgs e);

        private void AxObjectXMFDisplay_Resize(object sender, EventArgs e)
        {
            var zoomFactorWidth = outputView.Width / (float)BASE_SIZE.Width;
            var zoomFactorHeight = outputView.Height / (float)BASE_SIZE.Height;
            var zoomFactor = (zoomFactorWidth > zoomFactorHeight ? zoomFactorHeight : zoomFactorWidth);
            outputView.ZoomFactor = zoomFactor > 0 ? zoomFactor : outputView.ZoomFactor;
        }
        #endregion

        #region Propriedades
        public bool HostConnected { get; private set; }

        public override string Text => _telaAtual.Texto;
        public string[] MFText => _telaAtual.Texto.ToLineArray();

        protected bool IsInDesignMode =>
            ((bool) Parent.GetType().GetProperty("IsInDesignMode")?.GetValue(Parent, null)) ||
            LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        #endregion

        #region Métodos Públicos

        public void Connect()
        {
            HostConnected = true;
        }

        public void Disconnect()
        {
            Text = "";
            HostConnected = false;
        }

        public string GetScreen2(int row, int column, int length)
        {
            row--;   
            column--;
            var stbScreen = new StringBuilder();
            while (length > 0)
            {
                stbScreen.AppendFormat(MFText[row++].Substring(column, length>80? 80: length));
                length = length - 80;
            }
            return stbScreen.ToString();
        }

        public void PutScreen2(string text, int row, int column)
        {
            SetPosition(column, row);
            PutScreen(text);
        }

        public void RDE_SendKeys_Hsynch(string command, bool waitForEvents, int waitCount, int timeOut, string szVerString, int row, int column)
        {
            KeyEvent(command);
        }

        public void SendKeys(string text)
        {
            if (!KeyEvent(text))
            {
                PutScreen2(text, _cursorPosition.Y, _cursorPosition.X);
            }
        }

        public void LoadConfig(string configName)
        {
            if (IsInDesignMode) return;

            _configuracoes.Nome = configName;

            //Verifica se já não existe um config Salvo
            if (!File.Exists($"{Application.LocalUserAppDataPath}\\{_configuracoes.Nome}.xml"))
            {
                var config = new TelaConfiguracoes { Configuracoes = _configuracoes };

                config.ShowDialog(ParentForm ?? new Form());
            }
            else
            {
                try
                {
                    var serializer = new Serializer<BaseMf>();
                    _configuracoes = serializer.XmlDeserialize(File.ReadAllText($"{Application.LocalUserAppDataPath}\\{_configuracoes.Nome}.xml"));
                }
                catch
                {
                    MessageBox.Show(@"O arquivo de conficurações salvo é inválido ou está corrompido. Por favor, selecione um novo arquivo.");
                    var config = new TelaConfiguracoes { Configuracoes = _configuracoes };
                    config.ShowDialog(ParentForm);
                }
            }
            _telaAtual = new Tela(_configuracoes.TelaInicial, _configuracoes);
            UpdateScreen();
        }
        #endregion

        #region Métodos Privados
        private bool KeyEvent(string command)
        {
            
            try
            {
                var tela = _telaAtual.NavegaConsoleKey(command.ToConsoleKey(), _cursorPosition);
                if (tela.Texto != "")
                {
                    _telaAtual = tela;
                }
                UpdateScreen();
                return true;
            }
            catch (ArgumentException)
            { 
                return false;
            }
        }

        private void UpdateScreen()
        {
              outputView.InvokeIfRequired(c => c.Text = _telaAtual.Texto);
        }

        private void SetPosition(int positionX, int positionY)
        {
            _cursorPosition.X = positionX;
            _cursorPosition.Y = positionY;
            _telaAtual.PosicaoX = positionX;
            _telaAtual.PosicaoY = positionY;
        }

        private void PutScreen(string text)
        {
            _telaAtual.Escreve(text);
            UpdateScreen();
        }
        #endregion

        #region Atributos do controle
        public bool AutoFont { get; set; }

        public int AutoFontMinimumWidth { get; set; }

        public bool CharacterCase { get; set; }

        public string CharacterSetId2 { get; set; }

        public int ClipboardConfiguration { get; set; }

        public int CursorBlinkRate { get; set; }

        public int CursorColumn { get { return _cursorColumn; } set { _cursorPosition.X = value; _cursorColumn = value; } }
        public int CursorRow { get { return _cursorRow; } set { _cursorPosition.Y = value; _cursorRow = value; } }
        public int CursorSize { get; set; }

        public int CursorStyle { get; set; }

        public bool DblClkSelection { get; set; }

        public bool DisableSoundAlarm { get; set; }

        public int DisableStartPrinter { get; set; }

        public int EventVersion { get; set; }

        public int FixedAspectRatio { get; set; }

        public bool FontBold { get; set; }

        public int FontCharacterSpacing { get; set; }

        public bool FontItalic { get; set; }

        public int FontSize { get; set; }

        public string FontTypeFace { get; set; }

        public int FontWeight { get; set; }

        public int GDDMLogicalHeight { get; set; }

        public int GDDMLogicalWidth { get; set; }

        public int HostInterfaceConfiguration { get; set; }

        public int KeyboardConfiguration { get; set; }

        public bool KeyboardUnlockWCC { get; set; }

        public int LicenseScheme { get; set; }

        public string LightPen { get; set; }

        public bool NumericFieldOverride { get; set; }

        public AxHost.State OcxState { get; set; }

        public bool OutlinePresentationSpace { get; set; }

        public bool PadFieldsWithSpaces { get; set; }

        public int PasteInsert { get; set; }

        public bool RDE_Trace_Hsynch { get; set; }

        public bool ReportInformation { get; set; }

        public int ScreenIdVersion { get; set; }

        public bool ShowLightPenCursor { get; set; }

        public bool SmartInsert { get; set; }

        public int TextBlinkRate { get; set; }

        public int WatermarkConfiguration { get; set; }

        public bool WordWrap { get; set; }
        #endregion
    }
}
