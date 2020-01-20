# Microserviços Weather Forecast 

Esse projeto tem como objetivo contruir dois microserviços para cadastra e consumir a previsão do tempo.

## Projeto manageForecast 

Nesse projeto temos uma API responsável por cadastrar previsões do tempo conforme parametros enviado via Get  ex : http://localhost:10005/api/WeatherManager/blumenau/br

    - O Parametros recebidos por meio da API são concatenados em uma URL e um evento na fila de "requests" é postado no RabbitMQ.

Além dessa API, o projeto tem um evento registrado no RabbitMQ com o objeto de escutar previsão do tempo e armazenar essas previsões do tempo é um banco de dados (Nesse exemplo, usamos o SQL Lite).

## Projeto weatherForecast 

Nesse projeto temos um evento responsável por escutar pedidos de previsão de tempo. Assim que o sistema captura um evento, o mesmo consulta a previsão do tempo do site http://api.openweathermap.org e disponibiliza o retorno em uma fila de resultados.

Pré requisitos
------------------------------

- Visual Studio 2019 ou VS Code (https://visualstudio.microsoft.com/pt-br/vs/community/ ou https://code.visualstudio.com/).
- .Net core 3.0 (https://dotnet.microsoft.com/download/dotnet-core/3.0)
- Docker and Docker-compose for windows (https://docs.docker.com/docker-for-windows/install/)
- Clonar o projeto :
    ~~~PowerShell 
          git clone https://github.com/MaxwellAP/weather_forecast 
    ~~~

Setup Docker
------------------------------
Abra o terminal de comando na raiz do projeto (no local onde estão os arquivos WeatherForecast.sln e o docker-compose.yaml), sem seguida execute a seguinte linha de comando :

~~~PowerShell
docker-compose up --build
~~~

Aguarde a executação do procedimento, a primeira execução deve demorar alguns segundos. Após subir todos os serviços, teramos um resultado semelhante ao seguinte :

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

Em seguida, basta abrir o Postman e realizar as chamadas. Em ambos os casos, o verbo utilizando é o GET, seguem as URLss: 

 - http://localhost:10005/api/WeatherManager/blumenau/br -> API de consulta e cadastro de previsão do tempo
 - http://localhost:10005/api/WeatherManager/blumenau/br -> API que retorna o resultado da pesquisa em banco de dados

A todo momento, os registros e o log dos eventos criados e consumidos são escrito no terminal. Além disso, você pode executar o Console Application contido na Solution, para realizar uma rajada de cinco testes seguidos.

Setup Visual Studio Code
------------------------------

Abre o terminal e crie o repositorio docker para executar o RabbitMQ

~~~PowerShell
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 -p 4369:4369 -p 5671:5671-p 25672:25672 -p 15671:15671-e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest rabbitmq:3-management
~~~

Ainda com o terminal aberto, acesse do diretório do projeto /weather_forecast e restaure as dependencias em seus respectivos projetos

~~~PowerShell
dotnet restore
~~~

Prescione CRTL + D em seguida escolha o projeto ManagerWeather Launch (web) e execute. O sistema deverá instaciar o projeto web rodanda na porta 10005