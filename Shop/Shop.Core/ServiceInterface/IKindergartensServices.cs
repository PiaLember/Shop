using Shop.Core.Domain;
using Shop.Core.Dto;
using System;


namespace Shop.Core.ServiceInterface
{
    public interface IKindergartensServices
    {
        Task<Kindergarten> Create(KindergartenDto dto);
        Task<Kindergarten> DetailsAsync(Guid id);
        Task<Kindergarten> Update(KindergartenDto dto);
    }
}
