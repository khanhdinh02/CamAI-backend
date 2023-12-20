using Core.Application.Specifications.Repositories;
using Core.Domain.Entities;

namespace Core.Application;

public class BrandByIdRepoSpec(Guid id) : RepositorySpecification<Brand>(x => x.Id == id);
