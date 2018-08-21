using System;

namespace Carubbi.Mainframe
{ /// <summary>
    /// Extension Methods para o Controle de Interface de Emulação de Terminal Mainframe
    /// </summary>
    public static class MainframeExtensions
    { 
        /// <summary>
        /// Transforma uma tecla em um comando legível pelo Controle de Interface de Emulação de Terminal Mainframe
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToComandText(this ConsoleKey value)
        {
            switch (value)
            {
                case ConsoleKey.Enter: return "@E";
                case ConsoleKey.Escape: return "@C";
                case ConsoleKey.F1: return "@1";
                case ConsoleKey.F2: return "@2";
                case ConsoleKey.F3: return "@3";
                case ConsoleKey.F4: return "@4";
                case ConsoleKey.F5: return "@5";
                case ConsoleKey.F6: return "@6";
                case ConsoleKey.F7: return "@7";
                case ConsoleKey.F8: return "@8";
                case ConsoleKey.F9: return "@9";
                case ConsoleKey.F10: return "@a";
                case ConsoleKey.F11: return "@b";
                case ConsoleKey.F12: return "@c";
                default: throw new ArgumentOutOfRangeException(nameof(value), @"O comando enviado não é um comando válido!");
            }
        }
    }
}
