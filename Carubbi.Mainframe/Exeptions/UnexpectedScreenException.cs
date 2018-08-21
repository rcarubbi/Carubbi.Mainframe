using System;

namespace Carubbi.Mainframe.Exeptions
{
    /// <summary>
    /// Exceção disparada quando uma tela não esperada é encontrada após uma navegação
    /// </summary>
    public class UnexpectedScreenException : Exception
    {

        public string Screen { get; }


        public UnexpectedScreenException(string message, string screen)
            : base(message, null)
        {
            Screen = screen;
        }

        public UnexpectedScreenException(string message, string screen, Exception innerException)
            : base(message, innerException)
        {
            Screen = screen;
        }
    }
}
