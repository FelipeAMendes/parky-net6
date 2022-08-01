using ParkyWeb.Models.ViewModels;
using ParkyWeb.Services.Interfaces;

namespace ParkyWeb.Controllers.Extensions
{
    public static class NationalParksControllerExtensions
    {
        public static async Task DefinePicture(this NationalParksController _,
                                               INationalParkService nationalParkService,
                                               NationalParkViewModel nationalParkDto,
                                               IFormFileCollection files)
        {
            if (files.Count > 0)
            {
                await using var fileStream = files[0].OpenReadStream();
                await using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                var imageFile = memoryStream.ToArray();

                nationalParkDto.Picture = imageFile;
            }
            else
            {
                var existingPark = await nationalParkService.GetAsync(nationalParkDto.Id);
                nationalParkDto.Picture = existingPark.Picture;
            }
        }

        public static async Task<bool> InsertOrUpdateNationalPark(this NationalParksController _,
                                                                  INationalParkService nationalParkService,
                                                                  NationalParkViewModel nationalParkDto)
        {
            if (nationalParkDto.Id != 0)
            {
                var resultUpdate = await nationalParkService.UpdateAsync(nationalParkDto);
                
                return resultUpdate;
            }

            var resultCreate = await nationalParkService.CreateAsync(nationalParkDto);

            return resultCreate;
        }
    }
}