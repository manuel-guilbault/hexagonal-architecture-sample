using HexagonalArchitectureSample.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace HexagonalArchitectureSample.WebApi.ActionResults
{
    public class EntityResult: OkObjectResult
    {
        public EntityResult(object value, VersionTag entityTag) : base(value)
        {
            EntityTag = entityTag;
        }

        public VersionTag EntityTag { get; }

        public override void OnFormatting(ActionContext context)
        {
            base.OnFormatting(context);

            context.HttpContext.Response.GetTypedHeaders().ETag 
                = new EntityTagHeaderValue($"\"{EntityTag}\"");
        }
    }
}
