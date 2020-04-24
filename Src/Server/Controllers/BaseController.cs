using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace BTB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;
        private ISession _session;

        protected IMediator Mediator
        {
            get
            {
                if (_mediator == null)
                {
                    _mediator = HttpContext.RequestServices.GetService<IMediator>();
                }

                return _mediator;
            }
        }

        protected ISession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = HttpContext.Session;
                }

                return _session;
            }
        }
    }
}