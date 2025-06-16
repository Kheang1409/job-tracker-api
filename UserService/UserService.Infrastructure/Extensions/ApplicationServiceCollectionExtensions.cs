using JobTracker.UserService.Application.Auths.Commands.Login;
using JobTracker.UserService.Application.Auths.Commands.ResetPassword;
using JobTracker.UserService.Application.Auths.Commands.ForgotPassword;

using JobTracker.UserService.Application.Addresses.Commands.CreateAddress;
using JobTracker.UserService.Application.Addresses.Commands.DeleteAddress;
using JobTracker.UserService.Application.Addresses.Commands.UpdateAddress;
using JobTracker.UserService.Application.Addresses.Queries.GetAddresses;
using JobTracker.UserService.Application.Addresses.Queries.GetAddress;

using JobTracker.UserService.Application.Skills.Commands.CreateSkill;
using JobTracker.UserService.Application.Skills.Commands.DeleteSkill;
using JobTracker.UserService.Application.Skills.Commands.UpdateSkill;
using JobTracker.UserService.Application.Skills.Queries.GetSkills;
using JobTracker.UserService.Application.Skills.Queries.GetSkill;

using JobTracker.UserService.Application.Users.Commands.CreateUser;
using JobTracker.UserService.Application.Users.Commands.DeleteUser;
using JobTracker.UserService.Application.Users.Commands.UpdateUser;
using JobTracker.UserService.Application.Users.Queries.GetUsers;
using JobTracker.UserService.Application.Users.Queries.GetUserProfile;


using JobTracker.UserService.Application.Projects.Commands.CreateProject;
using JobTracker.UserService.Application.Projects.Commands.DeleteProject;
using JobTracker.UserService.Application.Projects.Commands.UpdateProject;
using JobTracker.UserService.Application.Projects.Queries.GetProjects;
using JobTracker.UserService.Application.Projects.Queries.GetProject;

using JobTracker.UserService.Application.Behaviors;
using JobTracker.UserService.Application.Services;


using JobTracker.UserService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;

namespace JobTracker.UserService.Infrastructure.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            //Auth
            cfg.RegisterServicesFromAssemblyContaining<ForgotPasswordCommand>();
            cfg.RegisterServicesFromAssemblyContaining<ResetPasswordCommand>();
            cfg.RegisterServicesFromAssemblyContaining<LoginCommand>();

            //Users
            cfg.RegisterServicesFromAssemblyContaining<CreateUserCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateUserCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateUserWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<DeleteUserCommand>();

            cfg.RegisterServicesFromAssemblyContaining<GetUserProfileQuery>();
            cfg.RegisterServicesFromAssemblyContaining<GetUsersQuery>();


            //Skill
            cfg.RegisterServicesFromAssemblyContaining<CreateSkillCommand>();
            cfg.RegisterServicesFromAssemblyContaining<CreateSkillWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateSkillCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateSkillWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<DeleteSkillCommand>();

            cfg.RegisterServicesFromAssemblyContaining<GetSkillQuery>();
            cfg.RegisterServicesFromAssemblyContaining<GetSkillsQuery>();

            //Address
            cfg.RegisterServicesFromAssemblyContaining<CreateAddressCommand>();
            cfg.RegisterServicesFromAssemblyContaining<CreateAddressWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateAddressCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateAddressWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<DeleteAddressCommand>();

            cfg.RegisterServicesFromAssemblyContaining<GetAddressQuery>();
            cfg.RegisterServicesFromAssemblyContaining<GetAddressesQuery>();

            //Project
            cfg.RegisterServicesFromAssemblyContaining<CreateProjectCommand>();
            cfg.RegisterServicesFromAssemblyContaining<CreateProjectWithIdCommand>(); 
            cfg.RegisterServicesFromAssemblyContaining<UpdateProjectCommand>();
            cfg.RegisterServicesFromAssemblyContaining<UpdateProjectWithIdCommand>();
            cfg.RegisterServicesFromAssemblyContaining<DeleteProjectCommand>();

            cfg.RegisterServicesFromAssemblyContaining<GetAddressQuery>();
            cfg.RegisterServicesFromAssemblyContaining<GetAddressesQuery>();
        });

        //Users
        services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteUserCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetUserProfileQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetUsersQueryValidator>();

        //Auth
        services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<ForgotPasswordCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<ResetPasswordCommandValidator>();

        //Skill
        services.AddValidatorsFromAssemblyContaining<CreateSkillCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteSkillCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetSkillQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetSkillsQueryValidator>();

        //Address
        services.AddValidatorsFromAssemblyContaining<CreateAddressCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteAddressCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetAddressQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetAddressesQueryValidator>();

        // Project
        services.AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteProjectCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetProjectQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<GetProjectsQueryValidator>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
