using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentACar.Core.Results;
using RentACar.DTOs.Blog;
using IResult = RentACar.Core.Results.IResult;
namespace RentACar.Business.Abstract
{
    public interface IBlogService
    {
        IDataResult<List<BlogListDto>> GetAll();
        IDataResult<List<BlogListDto>> GetAllApproved();
        IDataResult<BlogListDto> GetById(int blogPostId);
        IResult Add(BlogCreateDto dto, string webRootPath);
        IResult Update(BlogUpdateDto dto, string webRootPath);
        IResult Approve(int blogPostId);
        IResult Delete(int blogPostId, string webRootPath);
    }
}