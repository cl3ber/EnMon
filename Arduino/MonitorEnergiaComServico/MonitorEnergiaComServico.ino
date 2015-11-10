// BIBLIOTECAS NECESSÁRIAS
#include <EmonLib.h>    // Emon Library : cálculos para os sensores.
#include <SPI.h>        // SPI Library : necessária para o Shield Ethernet.
#include <Ethernet.h>   // Ethernet Library : para o comunicação com a nuvem.
#include <String.h>     // String Library: trabalhar com caracteres.

// VARIÁVEIS GLOBAIS
EnergyMonitor emon1;    // Uma instância de um monitor de energial da Emon Library.
const int CT_PIN = 0;   // Pino onde está conectado o sinal do sensor de corrente.

IPAddress servico(192, 168, 1, 100);
byte mac[] = { 0x98, 0x4F, 0xEE, 0x00, 0x77, 0xA2 };
EthernetClient client;

void setup()
{
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }
  
  Ethernet.begin(mac);
  Serial.print("server is at ");
  Serial.println(Ethernet.localIP());

  delay(2000);

  // Inicializa o monitor de energia.
  emon1.current(CT_PIN, 111.11);    // Calibração do sensor (deveria ser 111.1 para o SCT-013-100).
}

// Envia dados para servidor.
void sendData(double Irms, double Potencia, double Consumo, double calc) {
  if (!client.connected()) {
    client.stop();
  }

  client.connect(servico,80);  // Conecta no servidor na porta 80 (vamos usar http).
  if(!client.connected()) {
    client.connect(servico, 80);
  }

  if(client.connected()){
    Serial.println("Conectado");
    Serial.println("GET HTTP://192.168.1.100/EnMon/EnergyMonitor.svc/Armazenar/" + String(Potencia, 4) + "/" + String(Consumo, 4) + "/" + String(calc, 4));
    client.println("GET /EnMon/EnergyMonitor.svc/Armazenar/" + String(Potencia, 4) + "/" + String(Consumo, 4) + "/" + String(calc, 4) + " HTTP/1.1");
    client.println("Host: 192.168.1.100"); 
    client.println("Connection: close");
    client.println("User-Agent: Arduino");
    client.println("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
    client.println("Accept-Encoding: gzip, deflate, sdch");
    client.println();
    while (client.available()) {
      char c = client.read();
      client.println(c);
      Serial.println(c);
    }
    
    delay(100);
    client.stop();
    Serial.println("Dados enviados!");
  }
  else {
    // kf you didn't get a connection to the server:
    Serial.println("<FAUSTÃO>EROOOOOOU!!</FAUSTÃO>");
  }
}


void loop()
{
  // Inicializa o monitor de energia.
  // Mede a corrente usando a biblioteca EmonLib e calcula potência.
  // Imprime dados na serial para depuração.
  double Consumo = 0, Irms = 0, Potencia = 0;
  Irms = emon1.calcIrms(1480);  // Mede a corrente RMS.
  Potencia = Irms * 127.0;   // Calcula a potência aparente (supondo que a rede elétrica esteja em 127 V).
  Consumo = ((Potencia / 1000) * 3600) * 0.43; // Calcula o consumo por kWh
  Serial.print("Corrente: ");
  Serial.print(Irms);   // Imprime a corrente na serial.
  Serial.print(" A \t");
  Serial.print("Potencia: ");
  Serial.print(Potencia); // Imprime a potência na serial.
  Serial.print(" W \t");
  
  Serial.print("Consumo por hora: "); // Imprime o consumo por hora na serial.
  Serial.print(Consumo);
  Serial.println(" R$ \t");
  Serial.println();

  // Envia dados para o servidor.
  sendData(Irms, Potencia, Consumo, Consumo);

  // Aguarde 1 segundos e siga em frente.
  delay(60000);

}
