using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.Location;
using RentACar.Entities;

namespace RentACar.Business.Concrete
{
    public class LocationService : ILocationService
    {
        private readonly RentACarDbContext _context;

        public LocationService(RentACarDbContext context) => _context = context;

        public IDataResult<List<Location>> GetAll()
        {
            var list = _context.Locations.OrderBy(x => x.Name).ToList();
            return new SuccessDataResult<List<Location>>(list);
        }

        public IDataResult<List<Location>> GetPickupLocations()
        {
            var list = _context.Locations
                .Where(x => x.IsPickup && x.IsActive)
                .OrderBy(x => x.Name)
                .ToList();
            return new SuccessDataResult<List<Location>>(list);
        }

        public IDataResult<List<Location>> GetDropoffLocations()
        {
            var list = _context.Locations
                .Where(x => x.IsDropoff && x.IsActive)
                .OrderBy(x => x.Name)
                .ToList();
            return new SuccessDataResult<List<Location>>(list);
        }

        public IDataResult<Location> GetById(int id)
        {
            var item = _context.Locations.FirstOrDefault(x => x.LocationId == id);
            if (item == null) return new ErrorDataResult<Location>("Lokasyon bulunamadı.");
            return new SuccessDataResult<Location>(item);
        }

        public IResult Add(LocationCreateDto dto)
        {
            var location = new Location
            {
                Name = dto.Name,
                Address = dto.Address,
                IsPickup = dto.IsPickup,
                IsDropoff = dto.IsDropoff,
                IsActive = dto.IsActive
            };
            _context.Locations.Add(location);
            _context.SaveChanges();
            return new SuccessResult("Lokasyon eklendi.");
        }

        public IResult Update(LocationUpdateDto dto)
        {
            var location = _context.Locations.Find(dto.LocationId);
            if (location == null) return new ErrorResult("Lokasyon bulunamadı.");

            location.Name = dto.Name;
            location.Address = dto.Address;
            location.IsPickup = dto.IsPickup;
            location.IsDropoff = dto.IsDropoff;
            location.IsActive = dto.IsActive;

            _context.Locations.Update(location);
            _context.SaveChanges();
            return new SuccessResult("Lokasyon güncellendi.");
        }

        public IResult Delete(int id)
        {
            var item = _context.Locations.Find(id);
            if (item == null) return new ErrorResult("Lokasyon bulunamadı.");
            _context.Locations.Remove(item);
            _context.SaveChanges();
            return new SuccessResult("Lokasyon silindi.");
        }
    }
}