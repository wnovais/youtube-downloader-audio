using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeAudioDownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Digite a URL do vídeo do YouTube: ");
            string url = Console.ReadLine();

            Console.Write("Digite o caminho completo do diretório onde deseja salvar o arquivo de áudio: ");
            string diretorio = Console.ReadLine();

            try
            {
                // cria cliente do YoutubeExplode
                var youtube = new YoutubeClient();

                // obtem informações do vídeo
                var video = await youtube.Videos.GetAsync(url);

                // obtem stream de áudio
                var streamInfo = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                var audioStreamInfo = streamInfo.GetAudioOnlyStreams().GetWithHighestBitrate();

                // verifica se o diretório existe. Caso contrário, cria diretório
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                // salva o arquivo de áudio no diretório com o nome do vídeo e a extensão do arquivo
                var nomeArquivo = $"{Path.Combine(diretorio, video.Title)}.{audioStreamInfo.Container}";
                await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, nomeArquivo);

                Console.WriteLine($"O áudio do vídeo '{video.Title}' foi baixado com sucesso!");
                Console.WriteLine($"O arquivo foi salvo em: {Path.GetFullPath(nomeArquivo)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao baixar o áudio: {ex.Message}");
            }
        }
    }
}
