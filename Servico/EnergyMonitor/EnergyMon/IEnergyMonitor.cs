using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EnergyMon
{
    [ServiceContract]
    public interface IEnergyMonitor
    {

        [OperationContract]
        void Armazenar(string potencia, string consumo, string valor);

        [OperationContract]
        List<DadosEnergyMonitor> ConsultarDadosPorDia();
        [OperationContract]
        List<DadosEnergyMonitor> ConsultarDadosUlitmaHora();
    }

    public class DadosEnergyMonitor
    {
        public double Potencia { get; set; }
        public DateTime Data { get; set; }
        public double Consumo { get; set; }
        public double Valor { get; set; }
    }
}
