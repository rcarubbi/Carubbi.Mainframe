using Carubbi.ObjectXMFDisplay.Util;
using System;

namespace Carubbi.ObjectXMFDisplay.Config
{
    /// <summary>
    /// Representa uma configuração de navegação de telas
    /// </summary>
    public class Navegacao
    {
        public Navegacao()
        {

        }

        /// <summary>
        /// Verifica se uma determinada tecla é equivalente a tecla configurada nesta navegação
        /// </summary>
        /// <param name="tecla">Tecla a ser pesquisada</param>
        /// <returns>Indicador se a tecla pesquisada confere com a tecla configurada na navegação</returns>
        public bool VerificaNavegacaoConsoleKey(ConsoleKey tecla)
        {
            return ConsoleKey != "" && tecla == ConsoleKey.ToConsoleKey();
        }

        /// <summary>
        /// Posição que o cursor deve estar no Eixo X para que a navegação aconteça (Caso não seja definida a navagação ocorrerá em qualquer posição)
        /// </summary>
        public int PosicaoCursorX { get; set; }

        /// <summary>
        /// Posição que o cursor deve estar no Eixo Y para que a navegação aconteça (Caso não seja definida a navagação ocorrerá em qualquer posição)
        /// </summary>
        public int PosicaoCursorY { get; set; }

        /// <summary>
        /// Nome do arquivo .mfc da Tela para aonde será feita a navegação
        /// </summary>
        public string TelaDestino { get; set; } = "";

        /// <summary>
        /// Tecla que causa a navegação
        /// </summary>
        public string ConsoleKey { get; set; } = "";
    }
}
