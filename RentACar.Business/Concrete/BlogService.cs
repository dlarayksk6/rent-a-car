using RentACar.Business.Abstract;
using RentACar.Core.Results;
using RentACar.DataAccess;
using RentACar.DTOs.Blog;
using RentACar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IResult = RentACar.Core.Results.IResult;
namespace RentACar.Business.Concrete
{
    public class BlogService : IBlogService
    {
        private readonly RentACarDbContext _context;

        public BlogService(RentACarDbContext context)
        {
            _context = context;
        }

        public IDataResult<List<BlogListDto>> GetAll()
        {
            var posts = _context.BlogPosts
                .OrderByDescending(b => b.CreatedAt)
                .ToList()              // ← ekle
                .Select(b => ToDto(b))
                .ToList();
            return new SuccessDataResult<List<BlogListDto>>(posts);
        }
        public IDataResult<List<BlogListDto>> GetAllApproved()
        {
            var posts = _context.BlogPosts
                .Where(b => b.IsApproved)
                .OrderByDescending(b => b.CreatedAt)
                .Take(3)
                .ToList()  // önce listeye çek
                .Select(b => ToDto(b))  // sonra map et
                .ToList();
            return new SuccessDataResult<List<BlogListDto>>(posts);
        }
        public IDataResult<BlogListDto> GetById(int blogPostId)
        {
            var post = _context.BlogPosts.FirstOrDefault(b => b.BlogPostId == blogPostId);
            if (post == null) return new ErrorDataResult<BlogListDto>("Blog bulunamadı.");
            return new SuccessDataResult<BlogListDto>(ToDto(post));
        }

        public IResult Add(BlogCreateDto dto, string webRootPath)
        {
            var post = new BlogPost
            {
                Title = dto.Title,
                Content = dto.Content,
                AuthorId = dto.AuthorId,
                CreatedAt = DateTime.Now,
                IsApproved = false,
                ImageUrl = dto.ImageUrl ?? "/images/no-image.png"  // string al
            };

            _context.BlogPosts.Add(post);
            _context.SaveChanges();
            return new SuccessResult("Blog eklendi.");
        }

        public IResult Update(BlogUpdateDto dto, string webRootPath)
        {
            var post = _context.BlogPosts.Find(dto.BlogPostId);
            if (post == null) return new ErrorResult("Blog bulunamadı.");

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrEmpty(dto.ImageUrl))
            {
                DeleteImage(post.ImageUrl, webRootPath);
                post.ImageUrl = dto.ImageUrl;
            }

            _context.SaveChanges();
            return new SuccessResult("Blog güncellendi.");
        }
        public IResult Approve(int blogPostId)
        {
            var post = _context.BlogPosts.Find(blogPostId);
            if (post == null) return new ErrorResult("Blog bulunamadı.");

            post.IsApproved = true;
            post.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return new SuccessResult("Blog onaylandı.");
        }

        public IResult Delete(int blogPostId, string webRootPath)
        {
            var post = _context.BlogPosts.Find(blogPostId);
            if (post == null) return new ErrorResult("Blog bulunamadı.");

            DeleteImage(post.ImageUrl, webRootPath);
            _context.BlogPosts.Remove(post);
            _context.SaveChanges();
            return new SuccessResult("Blog silindi.");
        }

        private void DeleteImage(string? imageUrl, string webRootPath)
        {
            if (string.IsNullOrEmpty(imageUrl) || imageUrl == "/images/no-image.png") return;
            var path = Path.Combine(webRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(path)) File.Delete(path);
        }

     

        private BlogListDto ToDto(BlogPost b) => new BlogListDto
        {
            BlogPostId = b.BlogPostId,
            Title = b.Title,
            Content = b.Content,
            AuthorId = b.AuthorId,
            ImageUrl = b.ImageUrl,
            IsApproved = b.IsApproved,
            CreatedAt = b.CreatedAt,
            UpdatedAt = b.UpdatedAt
        };
    }
}