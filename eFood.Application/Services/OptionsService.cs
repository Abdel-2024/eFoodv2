using AutoMapper;
using eFood.Application.DTOs;
using eFood.Application.IServices;
using eFood.Domain.Entities;
using eFood.Domain.IRepositories;
using eFood.Infra.Storage.IServices;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFood.Application.Services
{
    public class OptionsService : IOptionsService
    {
        private readonly IGenericRepository _repo;
        private readonly IUnitOfWork _unitOfwork;   
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _storage;
        private string containerName = "Options";

        public OptionsService(  IGenericRepository genericRepository,
                                IUnitOfWork unitOfwork,
                                IWebHostEnvironment env,
                                IMapper mapper,
                                IFileStorageService storage)
        {
            _repo = genericRepository;
            _unitOfwork = unitOfwork;
            _env = env;
            _mapper = mapper;
            _storage = storage;
        }

        public async Task<AppsResult<OptionsDTO>> AddOption(OptionsCreateDTO model) 
        {
            var option = _mapper.Map<Options>(model);      

            if (!object.Equals(model.File, null))
            {
                option.ImgUrl = await _storage.SaveFile(containerName, model.File);
            }

            var result = await _repo.AddItem<Options>(option);

            var resultToReturn = _mapper.Map<AppsResult<OptionsDTO>>(result);

            if (!_env.IsDevelopment())
            {
                resultToReturn.Entity = null;
            }

            return resultToReturn;
        }

        public async Task<AppsResult<OptionsDTO>> DeleteOption(int Id)
        {
            var result = await _repo.GetOne<Options>(Id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<OptionsDTO>>(result);
            }

            var deletingItem = await _repo.DeleteItem<Options>(result.Entity);

            var resultToReturn = _mapper.Map<AppsResult<OptionsDTO>>(deletingItem);

            return resultToReturn;
        }

        public async Task<AppsListResult<OptionsDTO>> GetAllFilterOptions(OptionsFilterDTO filter)
        {
            var predicate = PredicateBuilder.New<OptionsDTO>(); 

            if (filter.Page != 0)
            {
                filter.Pagination.Page = filter.Page;
            }

            if (filter.PageSize != 0)
            {
                filter.Pagination.PageSize = filter.PageSize;
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                predicate = predicate.And(c => c.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.ListIDs))
            {
                string[] splitString = filter.ListIDs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> listIDs = new List<int>();

                foreach (string item in splitString)
                {
                    listIDs.Add(Convert.ToInt32(item));
                }

                predicate = predicate.And(c => listIDs.All(i => i != c.OptionId));
            }

            if (!predicate.IsStarted)
            {
                predicate = null;
            }


            var options = _unitOfwork.GetQueryable<Options>();
            var typeOptions = _unitOfwork.GetQueryable<TypeOption>();

            var queryable =  typeOptions.GroupJoin(options, 
                            t => t.TypeOptionId,
                            o => o.TypeOptionId,
                            (t, o) => new { t, o })
                .SelectMany(
                    to => to.o.DefaultIfEmpty(),
                    (type, option) => new OptionsDTO
                    {
                        OptionId = (option == null) ? 0 : option.OptionId,
                        Name = option.Name,
                        Description = option.Description,
                        ImgUrl = option.ImgUrl,
                        TypeOptionId = type.t.TypeOptionId,
                        TypeOption = _mapper.Map<TypeOptionSummaryDTO>(type.t)
                    }).OrderBy(o=> o.Name).AsQueryable();

            var resultToReturn = await _unitOfwork.GettAllAsync<OptionsDTO>(queryable, predicate, filter.Pagination);

            return resultToReturn;
        }

        public async Task<AppsListResult<OptionsDTO>> GetAllOptions()
        {
            var result = await _repo.GetAll<Options>();

            var resultToReturn = _mapper.Map<AppsListResult<OptionsDTO>>(result);

            return resultToReturn;
        }

        public async Task<AppsResult<OptionsDTO>> UpdateOption(OptionsCreateDTO model, int Id)  
        {
            var result = await _repo.GetOne<Options>(Id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<OptionsDTO>>(result);
            }

            var option = _mapper.Map(model, result.Entity);

            if (!object.Equals(model.File, null))
            {
                option.ImgUrl = await _storage.EditFile(containerName, model.File, option.ImgUrl);
            }

            var savingResult = await _repo.SaveChanges();

            if (savingResult.Success)
            {
                result.Entity = option;
            }

            result.Success = savingResult.Success;
            result.Errors = savingResult.Errors;

            var resultToReturn = _mapper.Map<AppsResult<OptionsDTO>>(result);

            return resultToReturn;
        }
    }
}
