using System.Collections.Generic;

namespace Carubbi.ObjectXMFDisplay.Config
{
    /// <summary>
    /// Representa um conjunto de arquivos .mfc que compõem uma aplicação mainframe
    /// </summary>
    public class BaseMf
    {
        /// <summary>
        /// Nome do arquivo .mfc
        /// </summary>
        public string Nome { get; set; } = "";

        /// <summary>
        /// Caminho dos arquivos .mfc de uma determinada aplicação
        /// </summary>
        public string CaminhoArquivo { get; set; } = "";

        /// <summary>
        /// Arquivo .mfc da Tela inicial de uma aplicação
        /// </summary>
        public string TelaInicial { get; set; } = "";

        /// <summary>
        /// Lista de arquivos .mfc que compõem uma aplicação
        /// </summary>
        public List<string> Telas { get; set; } = new List<string>();
    }
}
