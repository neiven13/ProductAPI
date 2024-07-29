# ProductAPI - Teste Técnico

## Descrição
Uma API que poderá ser usada por outra aplicação cliente, com rotas definidas para realizar operações de CRUD e com um dashboard, que exibe preço médio e total de produtos para os tipos possível de produtos, definidos pelas regras de negócio.

A API também conta com uma implementação de autenticação usando HTTP Basic. Sendo necessário fornecer usuário e senha para realizar as requisições. Para esta API foi definido o seguinte usuário e senha para fins de teste:

```bash
    usuário: user
    senha: password
```
## Tecnologias
- **.NET 6**: Framework de desenvolvimento para a API.
- **PostgreSQL 15**: Banco de dados relacional para armazenar informações.
- **Entity Framework Core**: ORM para interação com o banco de dados.
- **Swagger**: Ferramenta para documentar e testar a API.

## Como usar

### Pré-requisitos
Antes de começar você precisará ter instalado em sua máquina o SDK do .NET 6, também será necessário que tenha instalado o banco de dados PostgreSQL e seu gerenciador pgAdmin.

### Configurações de usuário e senha no pgAdmin

Com o pgAdmin abertos será preciso configurar uma senha para o usuário 'postgres'.

1. Na aba lateral esquerda, vá até Login/Group Roles:
![](assets/login_group-roles.PNG)

2. Clique com o botão direito no usuário 'postgres' e vá em 'Properties':
![](assets/user_postgres.PNG)

3. Com a janela 'Properties" aberta, vá até a guia Definition e, no campo password, digite "postgres@2024":
![](assets/postgres_password.PNG)

### Execução

1. Clone o repositório:
```bash
    git clone https://github.com/neiven13/ProductAPI.git
```

2. Navegue até a pasta do projeto:
```bash
    cd ProductAPI
```

3. Restaure os pacotes:
```bash
    dotnet restore
```

4. Instancie o banco de dados:
```bash
    dotnet ef databse update
```

4. Execute a aplicação:
```bash
    dotnet watch run
```
### Extras
* Foram usados os seguintes princípios SOLID:
    1. SRP — Single Responsibility Principle:

    2. DIP — Dependency Inversion Principle:

### Pontos de melhoria futura
1. Implementação de índices no banco de dados, ...
2. Implementação 
        