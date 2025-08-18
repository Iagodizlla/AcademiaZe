# Academia do Zé

## Introdução

- O projeto Academia do Zé é um sistema de controle de academia, desenvolvido em contexto acadêmico para aplicar conceitos de arquitetura em camadas e Domain-Driven Design (DDD).
- A aplicação tem como objetivo gerenciar informações de alunos, colaboradores, acessos e demais recursos da academia, garantindo organização, segurança e escalabilidade.
- O sistema foi construído em C# com .NET 8.0, utilizando boas práticas de design de software para separar responsabilidades e facilitar manutenção e evolução do projeto.

---


## Tecnologias

<p align="left">
  <img src="https://skillicons.dev/icons?i=cs" height="50"/>
  <img src="https://skillicons.dev/icons?i=dotnet" height="50"/>
  <img src="https://skillicons.dev/icons?i=visualstudio" height="50"/>
  <img src="https://skillicons.dev/icons?i=git" height="50"/>
  <img src="https://skillicons.dev/icons?i=github" height="50"/>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/mysql/mysql-original.svg" height="50"/>
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/microsoftsqlserver/microsoftsqlserver-plain.svg" height="50"/>
  <img src="https://skillicons.dev/icons?i=docker" height="50"/>
</p>

---

## Funcionalidades

- **Alunos**
  - Cadastrar, editar, excluir e visualizar alunos
  - Gerenciar informações pessoais, planos e histórico de acessos

- **Colaboradores**
  - Cadastrar, editar, excluir e visualizar colaboradores
  - Gerenciar funções e permissões dentro do sistema

- **Acessos**
  - Registrar entradas e saídas de alunos e colaboradores
  - Consultar relatórios de frequência, permanência e horários mais procurados

---

## Como utilizar

1. Clone o repositório ou baixe o código fonte.
2. Abra o terminal ou o prompt de comando e navegue até a pasta raiz
3. Utilize o comando abaixo para restaurar as dependências do projeto.

```
dotnet restore
```

4. Em seguida, compile a solução utilizando o comando:
   
```
dotnet build --configuration Release
```

5. Para executar o projeto compilando em tempo real
   
```
dotnet run --project AcademiaDoZe.ConsoleApp
```

6. Para executar o arquivo compilado, navegue até a pasta `./AcademiaDoZe.WebApp/bin/Release/net8.0/` e execute o arquivo:
   
```
AcademiaDoZe.ConsoleApp.exe
```

## Requisitos

- .NET SDK (recomendado .NET 8.0 ou superior) para compilação e execução do projeto.

- Visual Studio 2022 ou superior (opcional, para desenvolvimento).
