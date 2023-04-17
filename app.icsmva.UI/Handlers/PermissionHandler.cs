using app.icsmva.DAO.dao.RolesAndPrivilegeMap;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace app.icsmva.DAO.Handlers
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {

        public AuthorizationRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; }
    }

    public class PermissionHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRolePrivilegemap _rolePrivilegemap;

        public PermissionHandler(IHttpContextAccessor httpContextAccessor, IRolePrivilegemap rolePrivilegemap)
        {
            _httpContextAccessor = httpContextAccessor;
            _rolePrivilegemap = rolePrivilegemap;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            if (context.Resource is HttpContext httpContext && httpContext.GetEndpoint() is RouteEndpoint endpoint)
            {
                endpoint.RoutePattern.RequiredValues.TryGetValue("controller", out var _controller);
                endpoint.RoutePattern.RequiredValues.TryGetValue("action", out var _action);

                endpoint.RoutePattern.RequiredValues.TryGetValue("page", out var _page);
                endpoint.RoutePattern.RequiredValues.TryGetValue("area", out var _area);
               var role = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == "RoleId").Value;
               var checkparmition = _rolePrivilegemap.Getparmition(Convert.ToInt32(role), (string)_action, (string)_controller);

                // Check if a parent action is permitted then it'll allow child without checking for child permissions
                if (!string.IsNullOrWhiteSpace(requirement?.PermissionName) && !requirement.PermissionName.Equals("Authorization"))
                {
                    _action = requirement.PermissionName;
                }

                if (requirement != null && context.User.Identity?.IsAuthenticated == true && _controller != null && _action != null&& checkparmition.ActionName!=null&&checkparmition.PrivilegeName!=null)
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask;
        }
    }
}