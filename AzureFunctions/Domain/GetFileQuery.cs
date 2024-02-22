using System.Collections.Generic;
using MediatR;

namespace AzureFunctions.Domain
{
    public class GetFileQuery : IRequest<IEnumerable<File>>
    {
        public string Name { get; set; }
    }
}