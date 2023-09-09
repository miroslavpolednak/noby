using Microsoft.AspNetCore.Http;

namespace NOBY.Services.FileAntivirus;

[TransientService, AsImplementedInterfacesService]
internal sealed class FileAntivirusService
    : IFileAntivirusService
{
    public async Task<IFileAntivirusService.CheckFileResults> CheckFile(IFormFile file)
    {
        /*using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
        }*/

        return IFileAntivirusService.CheckFileResults.Unknown;
    }
}
