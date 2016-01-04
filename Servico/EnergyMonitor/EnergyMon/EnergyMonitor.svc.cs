using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
           UriTemplate = "BuscarDadosPorDia")]
        public List<DadosEnergyMonitor> ConsultarDadosPorDia()
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
                            Data = Convert.ToDateTime(dadosArquivo[0]),
                            Potencia = Convert.ToDouble(dadosArquivo[1], CultureInfo.InvariantCulture),
                            Consumo = Convert.ToDouble(dadosArquivo[2], CultureInfo.InvariantCulture),
                            Valor = Convert.ToDouble(dadosArquivo[3], CultureInfo.InvariantCulture)
                        });
                    }
                }
            }

            var retorno = (from linha in dados
                         group linha by new { Data = linha.Data.Date } into g
                         select new DadosEnergyMonitor()
                         {
                             Data = g.Key.Data,
                             Potencia = g.Sum(dado => dado.Potencia),
                             Consumo = g.Sum(dado => dado.Consumo),
                             Valor = g.Sum(dado => dado.Valor)
                         }).ToList();
            return retorno;
        }
        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   UriTemplate = "BuscarDadosUltimaHora")]
        public List<DadosEnergyMonitor> ConsultarDadosUlitmaHora()
        {
            var dados = new List<DadosEnergyMonitor>();
            if (!File.Exists(caminhoArquivo))
                return dados;
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
                            Data = Convert.ToDateTime(dadosArquivo[0]),
                            Potencia = Convert.ToDouble(dadosArquivo[1], CultureInfo.InvariantCulture),
                            Consumo = Convert.ToDouble(dadosArquivo[2], CultureInfo.InvariantCulture),
                            Valor = Convert.ToDouble(dadosArquivo[3], CultureInfo.InvariantCulture)
                        }); 
                        }

                    }


            }

            var dataFinal = dados.Max(d => d.Data);
            

            return dados.OrderBy(d => d.Data).Where(d => d.Data >= dataFinal.AddHours(-1) && d.Data <= dataFinal).ToList();
        }
    }
}
