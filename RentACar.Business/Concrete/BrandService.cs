using Microsoft.AspNetCore.Hosting;
using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.Brand;
using RentACar.Entities;

namespace RentACar.Business.Concrete
{
    public class BrandService : IBrandService
    {
        private readonly RentACarDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandService(RentACarDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IDataResult<List<Brand>> GetAll() =>
            new SuccessDataResult<List<Brand>>(_context.Brands.OrderBy(b => b.Name).ToList());

        public IDataResult<List<Brand>> GetActive() =>
            new SuccessDataResult<List<Brand>>(_context.Brands.Where(b => b.IsActive).OrderBy(b => b.Name).ToList());

        public IDataResult<Brand> GetById(int id)
        {
            var brand = _context.Brands.Find(id);
            return brand == null
                ? new ErrorDataResult<Brand>("Marka bulunamadı.")
                : new SuccessDataResult<Brand>(brand);
        }

        public IResult Add(BrandCreateDto dto)
        {
            var brand = new Brand
            {
                Name = dto.Name,
                LogoUrl = dto.LogoUrl,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.Now
            };
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return new SuccessResult("Marka eklendi.");
        }

        public IResult Update(BrandUpdateDto dto)
        {
            var brand = _context.Brands.Find(dto.BrandId);
            if (brand == null) return new ErrorResult("Marka bulunamadı.");

            brand.Name = dto.Name;
            brand.IsActive = dto.IsActive;
            if (!string.IsNullOrEmpty(dto.LogoUrl))
                brand.LogoUrl = dto.LogoUrl;

            _context.Brands.Update(brand);
            _context.SaveChanges();
            return new SuccessResult("Marka güncellendi.");
        }

        public IResult Delete(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null) return new ErrorResult("Marka bulunamadı.");

            if (!string.IsNullOrEmpty(brand.LogoUrl))
            {
                var path = Path.Combine(_env.WebRootPath, brand.LogoUrl.TrimStart('/'));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return new SuccessResult("Marka silindi.");
        }

        public IResult ToggleStatus(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null) return new ErrorResult("Marka bulunamadı.");
            brand.IsActive = !brand.IsActive;
            _context.SaveChanges();
            return new SuccessResult();
        }
    }
}