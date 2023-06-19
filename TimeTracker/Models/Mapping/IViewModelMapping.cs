using AutoMapper;

namespace TimeTracker.Models.Mapping;

public interface IViewModelMapping
{
	void Create(IMapperConfigurationExpression configuration);
}
