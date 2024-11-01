# Projeto de Testes - Reciclagem API

Este projeto de testes valida a funcionalidade da API de Reciclagem, utilizando a abordagem BDD (Behavior Driven Development) com a linguagem Gherkin e SpecFlow.

## Tecnologias Utilizadas

- **Linguagem**: C#
- **Framework de Testes**: SpecFlow
- **Bibliotecas**:
  - **RestSharp**: Para realizar requisições HTTP.
  - **Xunit**: Para asserções e execução dos testes.

## Pré-Requisitos

- **.NET Core SDK 8.0** ou superior.
- **API de Reciclagem** deve estar em execução localmente em `http://localhost:5074/api/`.

## Executando os Testes

1. **Acesse o diretório do projeto de testes** no terminal.

2. **Execute o comando**:

   ```bash
   dotnet test
