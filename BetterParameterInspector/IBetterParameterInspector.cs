using System;

namespace BetterParameterInspector
{
    public interface IBetterParameterInspector
    {
        object BeforeCall(string operationName, object[] inputs);
        void AfterCall(string operationName, object[] outputs, ref object returnValue, object correlationState);
    }
}
