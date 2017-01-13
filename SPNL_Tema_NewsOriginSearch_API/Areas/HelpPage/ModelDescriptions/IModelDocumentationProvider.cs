using System;
using System.Reflection;

namespace SPNL_Tema_NewsOriginSearch_API.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}