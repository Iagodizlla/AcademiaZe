using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application_.Services;
using AcademiaDoZe.Domain.Entities;
using CommunityToolkit.Mvvm.Input;
namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    [QueryProperty(nameof(MatriculaId), "Id")]
    public partial class MatriculaViewModel : BaseViewModel
    {
        public IEnumerable<EAppMatriculaPlano> MatriculaPlanos { get; } = Enum.GetValues(typeof(EAppMatriculaPlano)).Cast<EAppMatriculaPlano>();
        public IEnumerable<EAppMatriculaRestricoes> MatriculaRestricoesMedicas { get; } = Enum.GetValues(typeof(EAppMatriculaRestricoes)).Cast<EAppMatriculaRestricoes>();

        private readonly IMatriculaService _matriculaService;
        private readonly IAlunoService _alunoService;
        private MatriculaDTO _matricula = new()
        {
            AlunoMatricula = new AlunoDTO { Nome = string.Empty, Cpf = string.Empty, DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-18)),
                Telefone = string.Empty, Endereco = new LogradouroDTO { Cep = string.Empty, Nome = string.Empty, Bairro = string.Empty, Cidade = string.Empty, Estado = string.Empty, Pais = string.Empty },
                Numero = string.Empty },
            Plano = EAppMatriculaPlano.Mensal,
            DataInicio = DateOnly.FromDateTime(DateTime.Today),
            DataFim = DateOnly.FromDateTime(DateTime.Today.AddMonths(1)),
            Objetivo = string.Empty,
            RestricoesMedicas = EAppMatriculaRestricoes.None,
            ObservacoesRestricoes = string.Empty,
        };
        public MatriculaDTO Matricula
        {
            get => _matricula;
            set => SetProperty(ref _matricula, value);
        }
        private int _matriculaId;
        public int MatriculaId
        {
            get => _matriculaId;
            set => SetProperty(ref _matriculaId, value);
        }
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }
        public MatriculaViewModel(IMatriculaService matriculaService, IAlunoService alunoService)
        {
            _matriculaService = matriculaService;
            _alunoService = alunoService;
            Title = "Detalhes da Matricula";
        }

        /* métodos de comandos */
        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
        public async Task InitializeAsync()
        {
            if (MatriculaId > 0)
            {
                IsEditMode = true;
                Title = "Editar Matricula";
                await LoadMatriculaAsync();
            }
            else
            {
                IsEditMode = false;
                Title = "Nova Matricula";
            }
        }
        [RelayCommand]
        public async Task LoadMatriculaAsync()
        {
            if (MatriculaId <= 0)
                return;
            try
            {
                IsBusy = true;
                var matriculaData = await _matriculaService.ObterPorIdAsync(MatriculaId);

                if (matriculaData != null)

                {
                    Matricula = matriculaData;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar matricula: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        public async Task SaveMatriculaAsync()
        {
            if (IsBusy)
                return;
            if (!ValidateMatricula(Matricula))
                return;
            try
            {
                IsBusy = true;
                // Verifica se o Aluno existe antes de continuar

                var alunoData = await _matriculaService.ObterPorAlunoIdAsync(Matricula.AlunoMatricula.Id);
                if (alunoData == null)

                {
                    await Shell.Current.DisplayAlert("Erro", "O Aluno informado não existe. O cadastro não pode continuar.", "OK");
                    return;
                }
                Matricula.AlunoMatricula = alunoData;
                if (IsEditMode)
                {
                    await _matriculaService.AtualizarAsync(Matricula);

                    await Shell.Current.DisplayAlert("Sucesso", "Matricula atualizada com sucesso!", "OK");

                }
                else
                {
                    await _matriculaService.AdicionarAsync(Matricula);

                    await Shell.Current.DisplayAlert("Sucesso", "Matricula criada com sucesso!", "OK");

                }
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao salvar matricula: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        public async Task SearchByCpfAsync()
        {
            if (string.IsNullOrWhiteSpace(Matricula.AlunoMatricula.Cpf))
                return;
            try
            {
                IsBusy = true;
                var alunoData = await _alunoService.ObterPorCpfAsync(Matricula.AlunoMatricula.Cpf);

                if (alunoData != null)

                {
                    Matricula.AlunoMatricula = alunoData;
                    OnPropertyChanged(nameof(Matricula));
                    await Shell.Current.DisplayAlert("Aviso", "CPF encontrado! Aluno preenchido automaticamente.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Aviso", "CPF não encontrado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar CPF: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        public async Task SelecionarLaudoAsync()
        {
            try
            {
                string escolha = await Shell.Current.DisplayActionSheet("Origem da Imagem", "Cancelar", null, "Galeria", "Câmera");
                FileResult? result = null;
                if (escolha == "Galeria")

                {
                    result = await FilePicker.Default.PickAsync(new PickOptions
                    {
                        PickerTitle = "Selecione uma imagem",
                        FileTypes = FilePickerFileType.Images
                    });
                }
                else if (escolha == "Câmera")
                {
                    if (MediaPicker.Default.IsCaptureSupported)
                    {
                        result = await MediaPicker.Default.CapturePhotoAsync();
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Erro", "Captura de foto não suportada neste dispositivo.", "OK");
                        return;
                    }
                }
                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    Matricula.LaudoMedico = new ArquivoDTO { Conteudo = ms.ToArray() };
                    OnPropertyChanged(nameof(Matricula));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao selecionar imagem: {ex.Message}", "OK");
            }
        }

        private static bool ValidateMatricula(MatriculaDTO matricula)
        {
            const string validationTitle = "Validação";
            if (string.IsNullOrWhiteSpace(matricula.Objetivo))
            {
                Shell.Current.DisplayAlert(validationTitle, "Objetivo é obrigatório.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(matricula.ObservacoesRestricoes))
            {
                Shell.Current.DisplayAlert(validationTitle, "Observações é obrigatório.", "OK");
                return false;
            }
            if (matricula.DataInicio == default)
            {
                Shell.Current.DisplayAlert(validationTitle, "Data de Inicio é obrigatória.", "OK");
                return false;
            }
            if (matricula.DataFim == default)
            {
                Shell.Current.DisplayAlert(validationTitle, "Data de Fim é obrigatória.", "OK");
                return false;
            }
            if (matricula.AlunoMatricula == null)
            {
                Shell.Current.DisplayAlert(validationTitle, "Aluno é obrigatório.", "OK");
                return false;
            }
            return true;
        }
    }
}