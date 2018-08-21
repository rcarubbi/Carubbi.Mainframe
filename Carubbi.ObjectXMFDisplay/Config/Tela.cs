using Carubbi.Extensions;
using Carubbi.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Carubbi.ObjectXMFDisplay.Config
{
    /// <summary>
    /// Representa uma tela de uma aplicação mainframe
    /// </summary>
    public class Tela
    {
        private BaseMf _configuracoes;

        public Tela()
        {

        }

        public Tela(string nome, BaseMf configuracoes)
        {
            CarregarTela(nome, configuracoes);
        }


        /// <summary>
        /// Carrega uma tela de acordo com um arquivo .mfc
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="configuracoes"></param>
        public void CarregarTela(string nome, BaseMf configuracoes)
        {
            _configuracoes = configuracoes;
            //Verifica se existe o arquivo da tela
            if (!File.Exists($"{_configuracoes.CaminhoArquivo}\\{nome}.mfc"))
            {
                throw new Exception($"Arquivo de configurações não encontrado para a tela {nome}.");
            }

            var telaTmp = new Serializer<Tela>().XmlDeserialize(File.ReadAllText($"{_configuracoes.CaminhoArquivo}\\{nome}.mfc").Trim());
            Texto = telaTmp.Texto;
            TextoEditavel = telaTmp.TextoEditavel;
            ItensNavegacao = telaTmp.ItensNavegacao;
        }

        /// <summary>
        /// Exibe o texto informado como parâmetro na tela corrente nas próximas posições editáveis disponíveis
        /// </summary>
        /// <param name="texto">Texto a ser exibido</param>
        public void Escreve(string texto)
        {
            try
            {
                var linha = PosicaoY - 1;
                var coluna = PosicaoX - 1;
                var textoEmLinhas = Texto.ToLineArray();
                var textoLinha = textoEmLinhas[linha].ToCharArray();
                var editavelLinha = TextoEditavel.ToLineArray()[linha].ToCharArray();

                //Se a primeira posição não for editável, o cursor não está em uma posição válida
                if (editavelLinha[coluna] != '@')
                    return;

                //Preenche todas as posições pulando campos não editáveis, não pula linhas
                foreach (var letra in texto)
                {
                    while (true)
                    {
                        if (editavelLinha[coluna] == '@')
                        {
                            textoLinha[coluna] = letra;
                            coluna++;
                            break;
                        }

                        coluna++;
                    }
                }
                //Atualiza dados
                textoEmLinhas[linha] = new string(textoLinha);
                Texto = string.Join("\n", textoEmLinhas);
            }
            catch
            {
                // ignored
            }
        }
        
        /// <summary>
        /// Texto inicial exibido na tela quando esta é carregada
        /// </summary>
        public string Texto { get; set; } = "";

        /// <summary>
        /// Versão do Texto inicial com a identificação das posições editáveis 
        /// </summary>
        public string TextoEditavel { get; set; } = "";

        /// <summary>
        /// Lista com os itens de navegação
        /// </summary>
        public List<Navegacao> ItensNavegacao { get; set; } = new List<Navegacao>();

        /// <summary>
        /// Posição do cursor no eixo X
        /// </summary>
        public int PosicaoX { get; set; }

        /// <summary>
        /// Posição do cursor no eixo Y
        /// </summary>
        public int PosicaoY { get; set; }

        /// <summary>
        /// Simula o envio de uma determinada tecla para uma coordenada da tela e efetua as navegações configuradas caso necessário
        /// </summary>
        /// <param name="tecla">Tecla enviada</param>
        /// <param name="cursorPosition">Posição do cursor na tela</param>
        /// <returns>Tela com as alterações sofridas após o envio da tecla</returns>
        public Tela NavegaConsoleKey(ConsoleKey tecla, Point cursorPosition)
        {
            foreach (var navegacao in ItensNavegacao)
            {
                if (navegacao.VerificaNavegacaoConsoleKey(tecla) 
                    && (navegacao.PosicaoCursorX == cursorPosition.X || navegacao.PosicaoCursorX == 0)
                    && (navegacao.PosicaoCursorY == cursorPosition.Y || navegacao.PosicaoCursorY == 0))
                {
                    return new Tela(navegacao.TelaDestino, _configuracoes);
                }
            }
            return new Tela();
        }
    }
}
