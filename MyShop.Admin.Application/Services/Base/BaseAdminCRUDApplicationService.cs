using MyShop.Admin.Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MyShop.Admin.Application.Services.Base
{
    public abstract class BaseAdminCRUDApplicationService<TEntity,TInput, TOutput> : ApplicationService, IBaseAdminCRUDApplicationService<TEntity, TInput, TOutput> where TEntity:class,IEntity<long> 
    {
        private readonly IRepository<TEntity, long> _entityRepository;

        public BaseAdminCRUDApplicationService(IRepository<TEntity, long> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<TOutput> CreateAsync(TInput input)
        {
            var data = ObjectMapper.Map<TInput, TEntity>(input);
            var insertResult = await _entityRepository.InsertAsync(data, true);

            return ObjectMapper.Map<TEntity, TOutput>(insertResult);
        }

        public async Task<TOutput> GetAsync(long id)
        {
            var data = await _entityRepository.GetAsync(id);

            return ObjectMapper.Map<TEntity, TOutput>(data);
        }

        public async Task<List<TOutput>> GetListAsync()
        {
            var datas = await _entityRepository.GetListAsync();
            return ObjectMapper.Map<List<TEntity>, List<TOutput>>(datas);

        }

        public async Task<bool> DeleteAsync(long id)
        {
            var data = await _entityRepository.GetAsync(id);
            if (data == null)
            {
                throw new UserFriendlyException("信息不存在!");
            }
            try
            {
                await _entityRepository.DeleteAsync(data);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public IPagedResult<TOutput> GetPage(IPagedResultRequest input)
        {
            var result = _entityRepository.PageBy(input);

            var mapResult = ObjectMapper.Map<List<TEntity>, List<TOutput>>(result.ToList());

            return new PagedResultDto<TOutput>(result.Count(), mapResult);
        }
    }
}
