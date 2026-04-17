using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Entities;

namespace RentACar.DataAccess
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<RentACarDbContext>();

            
            string[] roles = new[] { "Admin", "Personel" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

          
            string adminEmail = "admin@rentacar.com";
            string adminPassword = "Admin123!";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, adminPassword);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

    
            if (!context.Permissions.Any())
            {
                var permissions = new[]
                {
       // Araçlar
new Permission { Name = "Araçları Görüntüle", Code = "Cars.View" },
new Permission { Name = "Araç Ekle/Düzenle", Code = "Cars.Manage" },
new Permission { Name = "Araç Sil", Code = "Cars.Delete" },

// Rezervasyonlar
new Permission { Name = "Rezervasyonları Görüntüle", Code = "Reservations.View" },
new Permission { Name = "Tarih Bloke Et / Rezervasyon Ekle-Düzenle", Code = "Reservations.Manage" },
new Permission { Name = "Rezervasyon Sil", Code = "Reservations.Delete" },
new Permission { Name = "Rezervasyon Onayla", Code = "Reservations.Approve" },
new Permission { Name = "Rezervasyon İptal", Code = "Reservations.Cancel" },
// Blog
new Permission { Name = "Blog Görüntüle", Code = "Blog.View" },
new Permission { Name = "Blog Ekle/Düzenle", Code = "Blog.Manage" },
new Permission { Name = "Blog Sil", Code = "Blog.Delete" },
new Permission { Name = "Blog Onayla", Code = "Blog.Approve" },

// Kampanyalar
new Permission { Name = "Kampanyaları Görüntüle", Code = "Campaigns.View" },
new Permission { Name = "Kampanya Ekle/Düzenle", Code = "Campaigns.Manage" },
new Permission { Name = "Kampanya Sil", Code = "Campaigns.Delete" },

// Sözleşmeler
new Permission { Name = "Sözleşmeleri Görüntüle", Code = "Contracts.View" },
new Permission { Name = "Sözleşme Ekle/Düzenle", Code = "Contracts.Manage" },
new Permission { Name = "Sözleşme Sil", Code = "Contracts.Delete" },

// Markalar
new Permission { Name = "Markaları Görüntüle", Code = "Brands.View" },
new Permission { Name = "Marka Ekle/Düzenle", Code = "Brands.Manage" },
new Permission { Name = "Marka Sil", Code = "Brands.Delete" },

// Lokasyonlar
new Permission { Name = "Lokasyonları Görüntüle", Code = "Locations.View" },
new Permission { Name = "Lokasyon Ekle/Düzenle", Code = "Locations.Manage" },
new Permission { Name = "Lokasyon Sil", Code = "Locations.Delete" },

// Kullanıcılar
new Permission { Name = "Kullanıcı Yönetimi", Code = "Users.Manage" },

// Diğer
new Permission { Name = "Ana Sayfa Düzenle", Code = "Home.Edit" },
new Permission { Name = "Hakkımızda Düzenle", Code = "About.Edit" },
new Permission { Name = "İletişim Düzenle", Code = "Contact.Edit" },
new Permission { Name = "Dashboard Görüntüle", Code = "Dashboard.View" },
new Permission { Name = "Raporları Görüntüle", Code = "Reports.View" },
                };

                context.Permissions.AddRange(permissions);
                await context.SaveChangesAsync();
            }
            if (!context.AboutContents.Any())
            {
                context.AboutContents.Add(new AboutContent
                {
                    Title = "Güvenilirliğin ve Konforun Tek Adresi",
                    SubTitle = "RentACar: Yolculuğunuzun Anahtarı",
                    Description1 = "RentACar olarak, ulaşımı sadece bir zorunluluk olmaktan çıkarıp, yolculuğun konforlu ve güvenilir bir parçası haline getirmeyi hedefliyoruz.",
                    Description2 = "Araç kiralamanın karmaşık süreçlerini ortadan kaldırıyor, size sadece yola odaklanma fırsatı sunuyoruz.",
                    Feature1Title = "Birinci Sınıf Hizmet",
                    Feature1Text = "Kaliteyle harmanlanmış profesyonel hizmet.",
                    Feature2Title = "7/24 Yol Yardım",
                    Feature2Text = "Nerede olursanız olun yanınızdayız.",
                    Feature3Title = "Uygun Fiyat",
                    Feature3Text = "Minimum maliyetle maksimum kalite.",
                    Feature4Title = "Ücretsiz Transfer",
                    Feature4Text = "Havaalanı + şehir içi teslimat ücretsiz.",
                    BannerTitle = "Müşterilerimize her durum için geniş bir yelpazede araçlar sunuyoruz.",
                    BannerText = "Araç kiralama acentamızda, bütçesi ne olursa olsun herkesin güvenilir ve konforlu bir araç kullanmanın keyfini çıkarmayı hak ettiğine inanıyoruz.",
                    CompletedOrders = 15425,
                    HappyCustomers = 8745,
                    CarFleet = 235,
                    YearsExperience = 15,
                    UpdatedAt = DateTime.Now,
                    HeroTitle = "Konforlu Araçlarla Güvenli Yolculuk",
                    HeroSubText = "İster iş, ister tatil… Modern ve bakımlı araç filomuzla yolculuğunuzun keyfini çıkarın.",
                    HeroFeature1Title = "Birinci Sınıf Hizmet",
                    HeroFeature1Text = "Profesyonel hizmet, kaliteli araçlar ve hızlı teslimat.",
                    HeroFeature2Title = "7/24 Yol Yardım",
                    HeroFeature2Text = "Olası durumlarda her zaman yanınızdayız."
                });
                await context.SaveChangesAsync();
            }

            if (!context.SiteContacts.Any())
            {
                context.SiteContacts.Add(new SiteContact
                {
                    Phone = "0000 000 00 00",
                    Email = "info@rentacar.com",
                    Address = "Trabzon",
                    Facebook = "#",
                    Instagram = "#",
                    Twitter = "#",
                    WorkingHours = "7/24 Hizmet",  // ← ekle
                    CreatedAt = DateTime.Now
                });
                await context.SaveChangesAsync();
            }
        }
    }
}