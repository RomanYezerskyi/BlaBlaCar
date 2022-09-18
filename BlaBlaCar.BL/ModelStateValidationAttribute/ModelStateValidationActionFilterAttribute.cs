using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlaBlaCar.BL.ModelStateValidationAttribute
{

    public class ModelStateValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)

                context.Result = new ContentResult()
                {
                    Content = "Modelstate 111not valid",
                    StatusCode = 400
                };
            base.OnActionExecuting(context);
        }
    }
}
