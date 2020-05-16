using AutoMapper;
using Cognito.Business.Models;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Filtering;
using Cognito.Shared.Helpers;
using Cognito.Web.BindingModels;
using Cognito.Web.BindingModels.Common;
using Cognito.Web.BindingModels.Contact;
using Cognito.Web.BindingModels.Detail;
using Cognito.Web.BindingModels.Domain;
using Cognito.Web.BindingModels.Filtering;
using Cognito.Web.BindingModels.Project;
using Cognito.Web.BindingModels.Task;
using Cognito.Web.BindingModels.Website;
using System.Linq;
using Cognito.Web.BindingModels.Point;
using Cognito.Web.BindingModels.Message;

namespace Cognito.Web.Infrastructure.Mapper
{
    // TODO: No reference in Web project to DataModel
    public class BindingModelsProfile : Profile
    {
        public BindingModelsProfile()
        {
            // BindingModels => Entities

            CreateMap<CreateWebsiteBindingModel, Website>();
            CreateMap<UpdateWebsiteBindingModel, Website>();
            CreateMap<CreateContactBindingModel, Contact>();
            CreateMap<UpdateContactBindingModel, Contact>();
            CreateMap<CreateDomainBindingModel, Domain>();
            CreateMap<UpdateDomainBindingModel, Domain>();
            CreateMap<DomainLicenseBindingModel, DomainLicense>();
            CreateMap<AddressBindingModel, Address>();
            CreateMap<TenantBindingModel, Tenant>();
            CreateMap<CreateContactBindingModel, Contact>();
            CreateMap<AccruedTimeBindingModel, AccruedTime>();
            CreateMap<ProjectBindingModel, Project>();
            CreateMap<UserProjectBindingModel, ProjectUser>();
            CreateMap<CreatePointBindingModel, Point>();
            CreateMap<UpdatePointBindingModel, Point>();

            CreateMap<TaskBindingModel, ProjectTask>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.TaskTypeId, opt => opt.MapFrom(src => src.TaskTypeId))
                .ForMember(dest => dest.NextDate, opt => opt.MapFrom(src => src.NextDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.TimeId, opt => opt.MapFrom(src => src.TimeId))
                .ForMember(dest => dest.TaskStatusId, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsEvent, opt => opt.MapFrom(src => src.IsEvent))
                .ForMember(dest => dest.Subtasks, opt => opt.MapFrom(src => src.Subtasks.Select(a => new Subtask { UserId = a })))
                .ForMember(dest => dest.Accrued, opt => opt.MapFrom(src => AccruedHelper.ConvertToNumber(src.Accrued)));

            CreateMap<DetailBindingModel, Detail>()
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Detail))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.BeginLine, opt => opt.MapFrom(src => src.BeginLine))
                .ForMember(dest => dest.BeginPage, opt => opt.MapFrom(src => src.BeginPage))
                .ForMember(dest => dest.EndLine, opt => opt.MapFrom(src => src.EndLine))
                .ForMember(dest => dest.EndPage, opt => opt.MapFrom(src => src.EndPage))
                .ForMember(dest => dest.Chrono, opt => opt.MapFrom(src => src.Chrono))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
                .ForMember(dest => dest.DetailTypeId, opt => opt.MapFrom(src => src.DetailTypeId))
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId));

            CreateMap<PagingBindingModel, Paging>();
            CreateMap<SortingBindingModel, Sorting>();
            CreateMap<PropertyFilterBindingModel, PropertyFilter>();
            CreateMap<DataFilterBindingModel, DataFilter>();

            CreateMap<MessageBindingModel, Message>();

            // BindingModels => Business Models

            CreateMap<LocalFilterBindingModel, LocalFilter>();
        }
    }
}
