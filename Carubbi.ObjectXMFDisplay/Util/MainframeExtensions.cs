using System;

namespace Carubbi.ObjectXMFDisplay.Util
{
    /// <summary>
    /// Extension Methods para o Controle Simulador de Emulação de Terminal Mainframe
    /// </summary>
    public static class MainframeExtensions
    {
        /// <summary>
        /// Transforma uma tecla em um comando legível pelo Simulador
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
                default: throw new ArgumentOutOfRangeException(nameof(value), string.Format("O comando enviado não é um comando válido!"));
            }
        }

        /// <summary>
        /// Transforma um comando do simulador em uma tecla
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ConsoleKey ToConsoleKey(this string value)
        {
            switch (value)
            {
                case "@E": return ConsoleKey.Enter;
                case "@C": return ConsoleKey.Escape;
                case "@1": return ConsoleKey.F1;
                case "@2": return ConsoleKey.F2;
                case "@3": return ConsoleKey.F3;
                case "@4": return ConsoleKey.F4;
                case "@5": return ConsoleKey.F5;
                case "@6": return ConsoleKey.F6;
                case "@7": return ConsoleKey.F7;
                case "@8": return ConsoleKey.F8;
                case "@9": return ConsoleKey.F9;
                case "@a": return ConsoleKey.F10;
                case "@b": return ConsoleKey.F11;
                case "@c": return ConsoleKey.F12;
                default: throw new ArgumentOutOfRangeException(nameof(value), string.Format("O comando enviado não é um comando válido!"));
            }
        }

      
        
    }
}
