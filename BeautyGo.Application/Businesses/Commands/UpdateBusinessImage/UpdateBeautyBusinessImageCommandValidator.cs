using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BeautyGo.Application.Businesses.Commands.UpdateBusinessImage;

internal class UpdateBeautyBusinessImageCommandValidator : AbstractValidator<UpdateBeautyBusinessImageCommand>
{
    public UpdateBeautyBusinessImageCommandValidator()
    {
        RuleFor(x => x.Files)
           .NotEmpty().WithMessage("É necessário enviar pelo menos um arquivo.")
           .Must(ContainValidImages).WithMessage("Todos os arquivos devem ser do tipo imagem (JPEG, PNG, GIF).")
           .Must(HaveValidFileSize).WithMessage("O tamanho de cada arquivo não pode exceder 50 MB.");
    }

    private bool ContainValidImages(ICollection<IFormFile> files) =>
        files.All(file => file.ContentType.StartsWith("image/"));

    private bool HaveValidFileSize(ICollection<IFormFile> files)
    {
        const int maxFileSize = 50 * 1024 * 1024; // 50MB
        return files.All(file => file.Length <= maxFileSize);
    }
}
