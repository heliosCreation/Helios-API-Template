using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Template.Application;
using Template.Application.Responses;

namespace Application.UnitTests
{
    /// <summary>
    /// Help to repplicate the handling of a request trough the MediatR pipeline in test. Go through the validation process as well.
    /// </summary>
    /// <typeparam name="T"> The command of type IRequest.</typeparam>
    /// <typeparam name="U">The command handler of type IRequestHandler.</typeparam>
    /// <typeparam name="V">The command validator of type AbstractValidator.</typeparam>
    /// <typeparam name="X">The expected response derived from BaseResponse</typeparam>
    /// <returns>The result of the handler processsing the request.</returns>
    public class RequestHandlerHelper<T, U, V, X>
        where T : IRequest<X>
        where U : IRequestHandler<T, X>
        where V : AbstractValidator<T>
        where X : BaseResponse, new()
    {
        public async Task<X> HandleRequest(T command, U handler, V validator)
        {
            var validationBehavior = new ValidationBehaviour<T, X>(new List<V>()
            {
                validator
            });
            return await validationBehavior.Handle(command, CancellationToken.None, () =>
            {
                return handler.Handle(command, cancellationToken: CancellationToken.None);
            });
        }

    }
}
