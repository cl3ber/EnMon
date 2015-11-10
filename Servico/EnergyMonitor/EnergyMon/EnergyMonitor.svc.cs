using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EnergyMon
{
    public class EnergyMonitor : IEnergyMonitor
    {
        string caminhoArquivo = "\\DadosEnergyMonitor\\DadosCapturados.txt";

        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "Armazenar/{potencia}/{consumo}/{valor}")]
        public void Armazenar(string potencia, string consumo, string valor)
        {

            if (!string.IsNullOrEmpty(potencia) && !string.IsNullOrEmpty(consumo))
            {
                if (!File.Exists(caminhoArquivo))
                {
                    var novoArquivo = new FileStream(caminhoArquivo, FileMode.Append);
                    novoArquivo.Close();
                }

                using (FileStream arquivo = new FileStream(caminhoArquivo, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter leitor = new StreamWriter(arquivo))
                    {
                        leitor.WriteLine(DateTime.Now.ToString() + "|" + potencia + "|" + consumo + "|" + valor);
                        leitor.Close();
                    }
                    arquivo.Close();
                }
            }

            //return "Data Execução:" + DateTime.Now.ToString() + "|" + potencia + "|" + consumo;
        }
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "BuscarDados")]
        public List<DadosEnergyMonitor> ConsultarDados()
        {
            var dados = new List<DadosEnergyMonitor>();
            if (!File.Exists(caminhoArquivo))
                return dados;

            //var dados = new List<DadosEnergyMonitor>();

            using (FileStream arquivo = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader leitor = new StreamReader(arquivo))
                {
                    while (!leitor.EndOfStream)
                    {
                        string linha = leitor.ReadLine();
                        var dadosArquivo = linha.Split('|');
                        dados.Add(new DadosEnergyMonitor()
                        {
                            Data = dadosArquivo[0],
                            Potencia = Convert.ToDouble(dadosArquivo[1]),
                            Consumo = Convert.ToDouble(dadosArquivo[2]),
                            Valor = Convert.ToDouble(dadosArquivo[3])
                        });
                    }
                }
            }

            var ListaTratada = new List<DadosEnergyMonitor>();
            var DataFim = DateTime.MinValue;
            DadosEnergyMonitor dadosProvisorio = null;

            dados.OrderBy(d => d.Data).ToList().ForEach(d =>
                {
                    if (DataFim < Convert.ToDateTime(d.Data) || DataFim == DateTime.MinValue)
                    {
                        DataFim = Convert.ToDateTime(d.Data).AddHours(1);
                        if (dadosProvisorio == null)
                            dadosProvisorio = new DadosEnergyMonitor();
                        else
                        {
                            ListaTratada.Add(dadosProvisorio);
                            dadosProvisorio = new DadosEnergyMonitor();
                        }

                        dadosProvisorio.Data = d.Data;
                    }

                    dadosProvisorio.Consumo += d.Consumo;
                    dadosProvisorio.Potencia += d.Potencia;
                    dadosProvisorio.Valor += d.Valor;

                });

            if (dadosProvisorio != null)
                ListaTratada.Add(dadosProvisorio);
            

            return ListaTratada.OrderBy(d => d.Data).ToList();
        }
    }
}
