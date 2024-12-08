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
    public class TypeOptionService : ITypeOptionService
    {
        private readonly IGenericRepository _repo;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _storage;
        private string containerName = "TypeOption";

        public TypeOptionService(IGenericRepository genericRepository,
                                 IWebHostEnvironment env,
                                 IMapper mapper,
                                 IFileStorageService storage)
        {
            _repo = genericRepository;
            _env = env;
            _mapper = mapper;
            _storage = storage;
        }

        public async Task<AppsResult<TypeOptionDTO>> AddTypeOption(TypeOptionCreateDTO model)
        {

            var typeOption = _mapper.Map<TypeOption>(model);

            if (!object.Equals(model.File, null))
            {
                typeOption.ImgUrl = await _storage.SaveFile(containerName, model.File);
            }

            var result = await _repo.AddItem<TypeOption>(typeOption);

            var resultToReturn = _mapper.Map<AppsResult<TypeOptionDTO>>(result);

            if (!_env.IsDevelopment())
            {
                resultToReturn.Entity = null;
            }

            return resultToReturn;
        }

        public async Task<AppsResult<TypeOptionDTO>> DeleteTypeOption(int Id)
        {
            var result = await _repo.GetOne<TypeOption>(Id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<TypeOptionDTO>>(result);
            }

            var deletedItem = await _repo.DeleteItem<TypeOption>(result.Entity);

            var resultToReturn = _mapper.Map<AppsResult<TypeOptionDTO>>(deletedItem);

            return resultToReturn;
        }

        public async Task<AppsListResult<TypeOptionDTO>> GetAllFiltredTypeOption(TypeOptionFilterDTO filter)
        {
            var predicate = PredicateBuilder.New<TypeOption>();

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

                predicate = predicate.And(c => listIDs.All(i => i != c.TypeOptionId));
            }

            if (!predicate.IsStarted)
            {
                predicate = null;
            }

            var result = await _repo.GetAll<TypeOption>(filter.Pagination,
                                                                predicate,
                                                                c => c.OrderByDescending(n => n.TypeOptionId),
                                                                c => c.Include(n => n.Options));

            var resultToReturn = _mapper.Map<AppsListResult<TypeOptionDTO>>(result);

            return resultToReturn;
        }

        public async Task<AppsListResult<TypeOptionDTO>> GetAllTypeOption()
        {
            var result = await _repo.GetAll<TypeOption>();

            var resultToReturn = _mapper.Map<AppsListResult<TypeOptionDTO>>(result);

            return resultToReturn;
        }

        public async Task<AppsResult<TypeOptionDTO>> UpdateTypeOption(TypeOptionCreateDTO model, int Id)
        {

            var result = await _repo.GetOne<TypeOption>(Id);

            if (!result.Success)
            {
                return _mapper.Map<AppsResult<TypeOptionDTO>>(result);
            }

            var typeOption = _mapper.Map(model, result.Entity); 

            if (!object.Equals(model.File, null))
            {
                typeOption.ImgUrl = await _storage.EditFile(containerName, model.File, typeOption.ImgUrl);
            }

            var savingItem = await _repo.SaveChanges();

            if (savingItem.Success)
            {
                result.Entity = typeOption;
            }

            result.Success = savingItem.Success;
            result.Errors = savingItem.Errors;

            var resultToReturn = _mapper.Map<AppsResult<TypeOptionDTO>>(result);

            return resultToReturn;
        }
    }
}
