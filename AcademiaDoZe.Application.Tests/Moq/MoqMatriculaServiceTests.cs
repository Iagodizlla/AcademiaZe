using AcademiaDoZe.Application.Interfaces;
using Moq;

namespace AcademiaDoZe.Application.Tests;

public class MoqMatriculaServiceTests
{
    private readonly Mock<IMatriculaService> _matriculaServiceMock;
    private readonly IMatriculaService _matriculaService;
    public MoqMatriculaServiceTests()
    {
        // para testar um serviço, em vez de se conectar ao banco de dados real, vamos injetar um mock do repositório.
        // isso permite que você teste a lógica de negócio do seu serviço sem depender do banco de dados.

        _matriculaServiceMock = new Mock<IMatriculaService>();
        _matriculaService = _matriculaServiceMock.Object;
    }
}