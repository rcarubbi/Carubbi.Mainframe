using System.Configuration;

namespace Carubbi.Mainframe.Config
{
    /// <summary>
    /// Seção de Configuração "Mainframe" pode conter as configurações de 1 ou mais terminais
    /// </summary>
    public class MainframeConfigSection : ConfigurationSection
    {
        public static MainframeConfigSection MainframeSection { get; } = ConfigurationManager.GetSection("Mainframe") as MainframeConfigSection;

        [ConfigurationProperty("Terminals")]
        [ConfigurationCollection(typeof(MainframeConfigCollection))]
        public MainframeConfigCollection MainframeTerminals => this["Terminals"] as MainframeConfigCollection;
    }
}
