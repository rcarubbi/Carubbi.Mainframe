using System.Configuration;
using System.Drawing;

namespace Carubbi.Mainframe.Config
{
    /// <summary>
    /// Elemento de Configuração "Terminal"
    /// </summary>
    public class MainframeConfigElement : ConfigurationElement
    {
        /// <summary>
        /// Id do controle MFDisplay que deve receber estas configurações
        /// </summary>
        [ConfigurationProperty("Nome", IsKey = true, IsRequired = true)]
        public string Nome {
            get => (string)base["Nome"];
            set => base["Nome"] = value;
        }

        /// <summary>
        /// Nome do sistema a ser emulado
        /// </summary>
        [ConfigurationProperty("Sistema", IsRequired = true, DefaultValue = "text/plain")]
        public string Sistema {
            get => (string) base["Sistema"];
            set => base["Sistema"] = value;
        }

        /// <summary>
        /// Hashtable de configurações Chave/Valor
        /// </summary>
        [ConfigurationProperty("Settings")]
        public KeyValueConfigurationCollection Settings => (KeyValueConfigurationCollection)base["Settings"];

        /// <summary>
        /// Converte um valor de configuração no padrão 0;0 em uma estrutura de dados de coordenada Point
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public Point GetPositionSetting(string parameterName)
        {
            var posicao = Settings[parameterName].Value.Split(';');
            return new Point { X = int.Parse(posicao[1]), Y = int.Parse(posicao[0]) };
        }
    }
}
 