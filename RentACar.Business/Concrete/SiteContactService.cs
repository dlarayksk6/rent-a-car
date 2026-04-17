using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.Contact;
using RentACar.Entities;

namespace RentACar.Business.Concrete
{
    public class SiteContactService : ISiteContactService
    {
        private readonly RentACarDbContext _context;

        public SiteContactService(RentACarDbContext context)
        {
            _context = context;
        }

        public IDataResult<SiteContact> Get()
        {
            var contact = _context.SiteContacts.FirstOrDefault();
            if (contact == null) return new ErrorDataResult<SiteContact>("İletişim bilgisi bulunamadı.");
            return new SuccessDataResult<SiteContact>(contact);
        }

        public IResult Update(ContactUpdateDto dto)
        {
            var contact = _context.SiteContacts.FirstOrDefault();
            if (contact == null)
            {
                var newContact = new SiteContact
                {
                    Phone = dto.Phone,
                    Email = dto.Email,
                    Address = dto.Address,
                    Facebook = dto.Facebook,
                    Instagram = dto.Instagram,
                    Twitter = dto.Twitter,
                    WhatsappNumber = dto.WhatsappNumber,
                    WorkingHours = dto.WorkingHours,
                    CreatedAt = DateTime.Now
                };
                _context.SiteContacts.Add(newContact);
            }
            else
            {
                contact.Phone = dto.Phone;
                contact.Email = dto.Email;
                contact.Address = dto.Address;
                contact.Facebook = dto.Facebook;
                contact.Instagram = dto.Instagram;
                contact.Twitter = dto.Twitter;
                contact.WhatsappNumber = dto.WhatsappNumber;
                contact.WorkingHours = dto.WorkingHours;
                contact.UpdatedAt = DateTime.Now;
                _context.SiteContacts.Update(contact);
            }
            _context.SaveChanges();
            return new SuccessResult("İletişim bilgileri güncellendi.");
        }
    }
}