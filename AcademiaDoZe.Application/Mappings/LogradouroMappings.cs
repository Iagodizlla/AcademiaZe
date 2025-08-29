using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;

namespace AcademiaDoZe.Application.Mappings;

public static class LogradouroMappings
{
    public static LogradouroDTO ToDto(this Logradouro logradouro)
    {
        return new LogradouroDTO
        {
            Id = logradouro.Id,
            Cep = logradouro.Cep,
            Nome = logradouro.Nome,
            Bairro = logradouro.Bairro,
            Cidade = logradouro.Cidade,
            Estado = logradouro.Estado,
            Pais = logradouro.Pais
        };
    }
    public static Logradouro ToEntity(this LogradouroDTO logradouroDto)
    {
        return Logradouro.Criar(
        logradouroDto.Id,
        logradouroDto.Cep,
        logradouroDto.Nome,
        logradouroDto.Bairro,
        logradouroDto.Cidade,
        logradouroDto.Estado,
        logradouroDto.Pais);
    }
    public static Logradouro UpdateFromDto(this Logradouro logradouro, LogradouroDTO logradouroDto)
    {
        // Cria uma nova instância do Logradouro com os valores atualizados

        return Logradouro.Criar(
        logradouro.Id, // Mantém o ID original
        logradouro.Cep, // Mantém o CEP original
        logradouroDto.Nome ?? logradouro.Nome,
        logradouroDto.Bairro ?? logradouro.Bairro,
        logradouroDto.Cidade ?? logradouro.Cidade,
        logradouroDto.Estado ?? logradouro.Estado,
        logradouroDto.Pais ?? logradouro.Pais);
    }
}