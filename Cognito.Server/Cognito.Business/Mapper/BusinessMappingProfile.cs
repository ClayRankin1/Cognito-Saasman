using AutoMapper;
using Cognito.Business.ViewModels;
using Cognito.Business.ViewModels.Task;
using Cognito.DataAccess.Entities;
using Cognito.Shared.Constants;
using Cognito.Shared.Helpers;
using System.Linq;

namespace Cognito.Business.Mapper
{
    public class BusinessMappingProfile : Profile
    {
        public BusinessMappingProfile()
        {
            CreateMap<Website, WebsiteViewModel>();
            CreateMap<Domain, DomainViewModel>();
            CreateMap<Project, ProjectViewModel>();
            CreateMap<ProjectUser, ProjectUserViewModel>();
            CreateMap<Tenant, TenantViewModel>()
                .ForMember(dest => dest.TenantAdminEmails, opt => opt.MapFrom(src => string.Join(", ", src.Domains.SelectMany(d => d.UserDomains.Select(ud => ud.User.Email)).Distinct())))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TenantName ?? src.CompanyName));
            CreateMap<Address, AddressViewModel>()
                .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => (int)src.StateId));

            CreateMap<State, StateViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int)src.Id));

            CreateMap<AccruedTime, AccruedTimeViewModel>();
            CreateMap<Contact, ContactViewModel>();
            CreateMap<License, LicenseViewModel>()
                .ForMember(dest => dest.LicenseTypeId, opt => opt.MapFrom(src => (int)src.LicenseTypeId));

            CreateMap<DomainLicense, DomainLicenseViewModel>();
            CreateMap<UserDomain, UserDomainViewModel>();
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<User, TeamMemberViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserName));


            CreateMap<ProjectTask, TaskViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser.UserName))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.NextDate, opt => opt.MapFrom(src => src.NextDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.GroupDate, opt => opt.MapFrom(src => src.NextDate.ToString(DateFormats.TaskGrouDateFormat)))
                .ForMember(dest => dest.TaskTypeId, opt => opt.MapFrom(src => src.TaskTypeId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.TaskStatusId))
                .ForMember(dest => dest.Accrued, opt => opt.MapFrom(src => AccruedHelper.ConvertToString(src.Accrued)))
                .ForMember(dest => dest.AccruedTotal, opt => opt.MapFrom(src => src.AccruedTimes.Sum(at => at.Total)))
                .ForMember(dest => dest.Subtasks, opt => opt.MapFrom(src => src.Subtasks.Select(ta => ta.UserId)))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Nickname))
                .ForMember(dest => dest.DetailsCount, opt => opt.MapFrom(src => src.Details.Count()))
                .ForMember(dest => dest.TimeId, opt => opt.MapFrom(src => src.TimeId))
                .ForMember(dest => dest.IsEvent, opt => opt.MapFrom(src => src.IsEvent));

            CreateMap<Detail, DetailViewModel>()
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Body))
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

            CreateMap<Point, PointViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Label));

            CreateMap<Time, LookupViewModel>();

            CreateMap<PointDetail, PointDetailViewModel>();
            CreateMap<Document, DocumentViewModel>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.DocumentStatusId)).ReverseMap();

            CreateMap<Message, MessageViewModel>();
        }
    }
}
