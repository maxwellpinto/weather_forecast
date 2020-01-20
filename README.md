# Microserviços Weather Forecast 

Esse projeto tem o objetivo construir dois microserviços. Sendo um para cadastrar e solicitar previsões de tempo e outro para consumir informações públicas na API http://api.openweathermap.org. Ambos os eventos são registadros em filas.

Por ser tratar de microserviços e acreditar que essas aplicações devem ter seu "ciclo de vida" independente, resolvi utilizar container com Docker. Além do Docker, utilizei também o Docker-compose para facilitar no "gerenciamento" dos trêS containers obtidos na aplicação (Microserviço 1, Microserviço 2 e RabbitMQ).

## Projeto manageForecast 

Projeto responsável por cadastrar previsões do tempo conforme parametros enviado via Get, no seguinte formato : http://localhost:10005/api/WeatherManager/blumenau/br

    - O Parametros recebidos nessa API são concatenados em uma URL e um evento na fila de "weatherForecastRequest" é postado no RabbitMQ.

Além dessa API, o projeto tem um evento registrado no RabbitMQ com o objeto de escutar o resultado da previsão do tempo que estão na fila "weatherForecastResponse". Após receber essas previsões, o sistema salva os dados em um banco de dados. Para esse projeto, utlizamos o SQLLite..

## Projeto weatherForecast 

Projeto responsável por escutar pedidos de previsão de tempo que estão na fila "weatherForecastRequest". Após capturar o evento, o mesmo consulta a previsão do tempo na API http://api.openweathermap.org e disponibiliza o retorno na fila de resultados "weatherForecastResponse".

## Projeto clientWeatherForecast 

Projeto Console Aplication que tem por objetivo dispara cinco chamadas seguidas para a API de previsão de tempo.

Deverá ser utilizado apenas penas testar as Serviços após a execução dos mesmos.


Pré requisitos
------------------------------

- Visual Studio 2019 ou VS Code (https://visualstudio.microsoft.com/pt-br/vs/community/ ou https://code.visualstudio.com/).
- .Net core 3.0 (https://dotnet.microsoft.com/download/dotnet-core/3.0)
- Docker and Docker-compose for windows (https://docs.docker.com/docker-for-windows/install/)
- Serviço do RabbiMQ rodando
- Clonar o projeto :
    ~~~PowerShell 
          git clone https://github.com/MaxwellAP/weather_forecast 
    ~~~

Setup Docker
------------------------------
Abra o terminal de comando na raiz do projeto (no local onde estão os arquivos WeatherForecast.sln e o docker-compose.yaml), em seguida, execute a seguinte linha de comando :

~~~PowerShell
docker-compose up --build
~~~

Aguarde a executação do procedimento, a primeira execução deve demorar alguns segundos. Após subir todos os serviços, teremos um resultado semelhante ao exibido abaixo :

~~~PowerShell
rabbitmq_1                 | 2020-01-20 07:56:04.201 [info] <0.618.0> connection <0.618.0> (172.19.0.3:56762 -> 172.19.0.2:5672): user 'guest' authenticated and granted access to vhost '/'
weather_forecast_1         | info: Microsoft.Hosting.Lifetime[0]
weather_forecast_1         |       Now listening on: http://[::]:80
weather_forecast_1         | info: Microsoft.Hosting.Lifetime[0]
weather_forecast_1         |       Application started. Press Ctrl+C to shut down.
weather_forecast_1         | info: Microsoft.Hosting.Lifetime[0]
weather_forecast_1         |       Hosting environment: Production
weather_forecast_1         | info: Microsoft.Hosting.Lifetime[0]
weather_forecast_1         |       Content root path: /app
manage_weather_forecast_1  | info: Microsoft.Hosting.Lifetime[0]
manage_weather_forecast_1  |       Now listening on: http://[::]:80
manage_weather_forecast_1  | info: Microsoft.Hosting.Lifetime[0]
manage_weather_forecast_1  |       Application started. Press Ctrl+C to shut down.
manage_weather_forecast_1  | info: Microsoft.Hosting.Lifetime[0]
manage_weather_forecast_1  |       Hosting environment: Production
manage_weather_forecast_1  | info: Microsoft.Hosting.Lifetime[0]
manage_weather_forecast_1  |       Content root path: /app
~~~

Em seguida, basta abrir o Postman e realizar as chamadas. Em ambos os casos, o verbo utilizando é o GET. 

URLs: 

 - http://localhost:10005/api/WeatherManager/blumenau/br -> API de consulta e cadastro de previsão do tempo
 - http://localhost:10005/api/WeatherManager/All -> API que retorna o resultado da pesquisa em banco de dados

Após realizar a chamada na API responsável por consultar a previsão do tempo, um log será gerado no console com as informações de obtidas do RabbitMQ, conforme exemplo : 

~~~PowerShell
manage_weather_forecast_1  | [ Manager ] Event to Weather request was Published to RabbitMQ
manage_weather_forecast_1  | [ Manager ]  Weater Forecast was enqueued: blumenau,br
weather_forecast_1         |  [Weater Forecast] Requets to Weather was received from Rabbit: blumenau,br
weather_forecast_1         | [Weater Forecast] Retrieving city weather forecast : blumenau,br
manage_weather_forecast_1  | [ Manager ] Event to Weather request was Published to RabbitMQ
manage_weather_forecast_1  | [ Manager ]  Weater Forecast was enqueued: blumenau,br
weather_forecast_1         |  [Weater Forecast] Requets to Weather was received from Rabbit: blumenau,br
weather_forecast_1         | [Weater Forecast] Retrieving city weather forecast : blumenau,br
weather_forecast_1         | [Weater Forecast] Event to weater response was published to RabbitMQ
manage_weather_forecast_1  | [ Manager ] Weather was received from Rabbit: {"coord":{"lon":-49.07,"lat":-26.92},"weather":[{"id":802,"main":"Clouds","description":"scattered clouds","icon":"03d"}],"base":"stations","main":{"temp":302.01,"feels_like":304.11,"temp_min":300.15,"temp_max":303.15,"pressure":1014,"humidity":74},"visibility":10000,"wind":{"speed":5.1,"deg":80},"clouds":{"all":40},"dt":1579534645,"sys":{"type":1,"id":8398,"country":"BR","sunrise":1579509590,"sunset":1579558445},"timezone":-10800,"id":3469968,"name":"Blumenau","cod":200}
~~~

Em seguida, pode realizar a chamada na API http://localhost:10005/api/WeatherManager/All e verificar a previsão do tempo que foi devidamente registrada na base de dados.

Setup Visual Studio Code
------------------------------

caso não tenha uma imagem do RabbitMQ em seu docker, abra o terminal e digite a linha de comando abaixo. Esse comando irá baixar uma menssagem e iniciar a execução do container.

~~~PowerShell
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 -p 4369:4369 -p 5671:5671-p 25672:25672 -p 15671:15671-e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest rabbitmq:3-management
~~~

**Obs : Caso já tenha uma imagem e um container RabbitMQ basta inicicar esse serviços, sem a 
ncessidade de criar um novo container.**

**Obs 1 : Os dados de configuração para conexão com o RabbitMQ estão no arquivos appsettings.json.**

**Obs 2 : A aplicação já está configurada para rodar no Docker ou um ambiente local sem container. A aplicação roconhece o ambiente e configura o Host conforme o ambiente.**

Ainda com o terminal aberto, certifique-se de que todas as dependencias de pacotes externos foram restauradas.

~~~PowerShell
dotnet restore
~~~

Em seguide digite o comando para iniciar o Visual Studio Code a partir do diretório onde estão os projetos.

~~~PowerShell
code .
~~~


Já com o VS Code aberto, prescione as teclas CRTL + D para exibir as configurações de Debug. Em seguida, escolha o projeto **ManagerWeather Launch (web)** e execute. O projeto deverá instaciar um serviço WEB, rodanda na porta 10005.

Em seguida, execute o mesmo procedimento para o serviço **WeatherForecast Launch (web)**. O projeto deverá instanciar um serviço WEB, rodando na porta 10000.

Pronto!! Basta executar o Console Application 'clientWeatherForecast' ou fazer a chamada via Postman, nas seguintes URLs:

 - http://localhost:10005/api/WeatherManager/blumenau/br -> API de consulta e cadastro de previsão do tempo
 - http://localhost:10005/api/WeatherManager/All -> API que retorna o resultado da pesquisa em banco de dados
