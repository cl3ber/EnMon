# Monitor de Energia - Trabalho Integrador da FATEC Itaquera para os alunos de Automação

Projeto de monitor de energia utilizando as seguintes tecnologias:

* C++ : Para desenvolvimento do código que será executado na Placa controladora. 
* C# e .Net Framework 4.5: Para desenvolvimento dos Serviços RestFull que controlam a captura dos dados do Arduino e tratamento para exibição dos dados.
* HTML: Para exibição dos dados em uma página web.
* JQuery: Biblioteca utilizada para realizar as chamadas ao serviço de uma maneira mais simples e realizar um tratamento de dados na página. Utilizada a versão 2.1.4 (https://jquery.com/)
* Chartist: Utilizada para gerar gráficos mostrando os consumos (gráfico de area e de barra). Versão utilizada 0.7.4 (https://gionkunz.github.io/chartist-js/)
* Biblioteca EnMonLib: Bibiblioteca responsável por realizar a leitura e conversão dos dados do sensor XPTO utilizada no código em c++

##Breve descrição:

Este projeto foi iniciado para auxiliar o grupo da minha esposa a realizar a leitura e exibição de dados de uma corrente elétrica utilizando a biblioteca EnMonLib, Arduino e C++. 
O projeto teve como base o código utilizado e disponibilizado pelo Manoel Lemos (https://github.com/mlemos/energy-monitor-cpbr7). 
Como na época em que começamos as pesquisas tivemos muita dificuldade em encontrar informações sobre a biblioteca EnMonLib e até mesmo sobre como estruturar o código no Arduino, 
decidimos - uma vez que concluído parcialmente o projeto - documentar e disponibilizar este projeto de maneira a auxiliar qualquer pessoa que deseje desenvolver um projeto semelhante.

No primeiro deploy deste projeto, iremos incluir os arquivos utilizados e realizaremos algumas melhorias de código. 
O desejo é disponibilizar todo o Código comentado até o fim de Dezembro, juntamente com um passo-a-passo do ambiente montado, configurações do servidor do Serviço e etc.

Qualquer dúvida, sugestão, ou erro encontrado antes do fim dessa primeira etapa (ou depois dela) pode ser feita, e será respondida o mais breve possível

Obrigado a Todos,

###Paula Célia e Cleber de Jesus
