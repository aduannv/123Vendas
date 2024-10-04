
# 123Vendas

123Vendas é um sistema de gerenciamento de vendas que permite criar, atualizar, cancelar vendas e itens associados. Este projeto foi desenvolvido utilizando padrões de design como DDD (Domain-Driven Design) e inclui uma API RESTful para interagir com as funcionalidades do sistema.

## Estrutura do Projeto

```
123Vendas
│
├── src
│   ├── 123Vendas.Api          # Camada da API
│   ├── 123Vendas.Application   # Camada de aplicação
│   ├── 123Vendas.Domain        # Camada de domínio
│   └── 123Vendas.Infrastructure # Camada de infraestrutura
│
└── tests
    ├── 123Vendas.UnitTests     # Testes unitários
    └── 123Vendas.IntegrationTests # Testes de integração
```

## Pré-requisitos

Antes de executar o projeto, você precisará ter instalado:

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

## Instalação

1. Clone o repositório:
   ```bash
   git clone https://github.com/aduannv/123Vendas.git
   ```

2. Navegue até o diretório do projeto:
   ```bash
   cd 123Vendas
   ```

3. Execute o projeto:
   ```bash
   docker-compose up
   ```
   
4. Acesse o swagger no navegador:
   ```bash
   https://localhost:57640/swagger
   ```

## Testes

Para executar os testes unitários e de integração, use os seguintes comandos:

- Para testes unitários:
  ```bash
  dotnet test tests/123Vendas.UnitTests
  ```

- Para testes de integração:
  ```bash
  dotnet test tests/123Vendas.IntegrationTests
  ```

## Como Contribuir

1. Faça um fork do projeto.
2. Crie uma nova branch (`feature/nome-da-sua-feature`).
3. Faça suas alterações e commit (`git commit -m 'Adicionei uma nova feature'`).
4. Faça o push para a branch (`git push origin feature/nome-da-sua-feature`).
5. Crie um Pull Request.

### Git Flow

Este projeto utiliza o Git Flow para o gerenciamento de branches:

- `main`: branch principal com a versão estável do código.
- `develop`: branch de desenvolvimento onde novas funcionalidades são integradas.
- `feature/*`: branches para desenvolvimento de novas funcionalidades.
- `release/*`: branches para preparar novas versões.
- `hotfix/*`: branches para corrigir bugs em produção.

## Licença

Este projeto está licenciado sob a MIT License - veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## Contato

Se você tiver alguma dúvida, sinta-se à vontade para entrar em contato:

- **Nome**: Aduan
- **Email**: aduannvl@gmail.com
