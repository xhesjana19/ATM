using System;
using System.Linq;
using System.Linq.Expressions;
using ATM.Core.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ATM.Web.Infrastructure
{
    public static class ModelStateExtensions
    {
        public static bool RemoveErrors(this ModelStateDictionary modelState, string key)
        {
            if (modelState.ContainsKey(key))
            {
                modelState[key].Errors.Clear();
                modelState[key].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

                return true;
            }

            return false;
        }

        public static void AddModelErrorFromResults(this ModelStateDictionary modelState, Result result)
        {
            foreach (var message in result.Messages)
            {
                modelState.AddModelError(message.Code, message.Text);
            }
        }
    }
}