
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyShop.Admin.Application.Services.Interface
{
    public interface IBaseAdminCRUDApplicationService<TEntity, TInput, TOutput> : IApplicationService
    {

        Task<List<TOutput>> GetListAsync();

        IPagedResult<TOutput> GetPage(IPagedResultRequest input);

        Task<TOutput> GetAsync(long id);

        Task<TOutput> CreateAsync(TInput input);

        Task<bool> DeleteAsync(long id);
    }
}
