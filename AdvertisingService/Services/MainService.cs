using System.Linq;
using System.Threading.Tasks;
using AdvertisingService.Models;
using AdvertisingService.Protos;
using Grpc.Core;
using MongoDB.Bson;

namespace AdvertisingService.Services
{
    public class MainService: Advertising.AdvertisingBase
    {
        private readonly AdvertisementsService _advertisingService;

        public MainService(AdvertisementsService advertisingService)
        {
            _advertisingService = advertisingService;
        }

        public override async Task<AdvertsList> GetAdvertisements(GetAdvertsRequest request, ServerCallContext context)
        {
            // simple linq select statement on top of database query
            return new()
            {
                Advert =
                {
                    (await _advertisingService.GetAllAdvertsAsync()).Select(x => new Advert
                    {
                        Message = x.Message,
                        Title = x.Title,
                        ImageUrl = x.ImageUri
                    })
                }
            };
        }

        public override async Task<GenericResponse> UpsertAdvertisement(Advert request, ServerCallContext context)
        {
            await _advertisingService.UpsertAsync(new Advertisement
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Message = request.Message,
                Title = request.Title,
                ImageUri = request.ImageUrl
            });
            return new GenericResponse
            {
                ErrorMessage = "",
                IsSuccessful = true
            };
        }

        public override async Task<GenericResponse> DeleteAdvertisement(DeleteAdvertRequest request, ServerCallContext context)
        {
            await _advertisingService.DeleteByIdAsync(request.AdvertId);
            return new GenericResponse
            {
                ErrorMessage = "",
                IsSuccessful = true
            };
        }
    }
}