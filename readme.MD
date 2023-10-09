# DESAFIO BT - INOA

## Objetivo:

O objetivo do sistema é avisar, via e-mail, caso a cotação de um ativo da B3 caia mais do que certo nível, ou suba acima de outro.

O programa deve ser uma aplicação de console.

Ele deve ser chamado via linha de comando com 3 parâmetros.

- O ativo a ser monitorado
- O preço de referência para venda
- O preço de referência para compra

## Configuração do projeto

Antes de executar o projeto é necessário ajustar o arquivo de configuração que fica em ./Inoa.Challenge/src/Inoa.Challenge.Console/appsettings.json

#### Email Settings

```
"emailSettings": {
    "credentials": {
      "username": "",
      "password": ""
    },
    "smtp": "",
    "port": null,
    "from": "",
    "to": "",
    "enableSsl": true
}
```

Caso Credentials nao seja necessário, pode setar a propriedade para null.  
Se a porta nao for definida, será usada a porta 25

#### Yahoo Settings

Será enviado por email um exemplo de configuração.

#### interval

intervalo de tempo que o programa irá esperar até executar novamente, por padrão está 5 minutos (5 _ 60 _ 1000)

## Exemplo de chamada

```
dotnet run Inoa.Challenge.Console.dll -a PETR4 -s 22.67 -b 22.59
```

onde

- a: asset,
- b: buy,
- s:sell

## Destaques do projeto

Projeto desenvolvido usando as melhores técnicas em .net core, em especial Clean Code, Clean Architecture e Vertical Slice.

- Foi dividido em usecases e gateways para chamadas externas.
- Abstrações para as APIs de email e busca de valores.
- Todo feito com injeção de dependência para propiciar testes unitários
- Configurado logs em arquivo (fica na pasta bin/log) para exemplificar
- Utilizado várias libs de mercado, como:
  - refit (abstração de camada http)
  - polly (resiliência na camada http, faz 3 tentativas antes de estourar erro)
  - CommandLineParser (parser de linha de comando com várias validações imbutidas)
  - Serilog (auxiliar no log da aplicação)
  - rabbitMq (lib nativa do rabbitMq)
  - xunit (lib para testes)
  - fluentAssertions (lib que facilita os asserts dos testes)
  - moq (lib usada para mocker as dependências)
- O usecase de Notification está apto a enviar notificação via e-mail ou mensageria
- Utilização do padrão IOptions para injeção de dependência do appSettings
- Projeto de testes unitários para as classes que contém regra de negócio.
- Criação de um arquivo de template para o HTML que fica embedded na aplicação
- Utilizado várias features das versões mais novas do C# como Records, pattern Matching, required properties, namespace reduzido e programação assíncrona (async/await)
