# C# Reciclagem API

Este projeto é uma API desenvolvida em C# para gerenciar o sistema de reciclagem.

## Executando a API C#

1. **Iniciar o Banco de Dados Oracle com Docker Compose**

   Primeiro, inicie o banco de dados Oracle localmente usando o Docker Compose:

   ```bash
   docker-compose -f ./docker-compose.db.yml up -d
   ```

   Isso criará e iniciará um container com o Oracle Database necessário para a aplicação.

2. **Restaurar as Dependências**

   Depois que o banco de dados estiver em execução, restaure as dependências do projeto:

   ```bash
   dotnet restore
   ```

3. **Executar a Aplicação com `dotnet watch`**

   Para rodar a aplicação e monitorar mudanças no código, use o comando:

   ```bash
   dotnet watch run
   ```

   A API estará disponível em `http://localhost:5074` ou `http://localhost:5074` (para HTTPS).
