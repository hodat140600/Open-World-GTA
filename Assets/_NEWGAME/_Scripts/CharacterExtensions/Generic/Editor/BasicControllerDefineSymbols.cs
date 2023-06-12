using System.Collections.Generic;
namespace _GAME._Scripts._DefineSymbolsManager
{
    public class BasicControllerDefineSymbols : ExtensionDefineSymbols
    {
        public override List<string> GetSymbols
        {
            get
            {
                return new List<string>() { "DATHQ_BASIC" };
            }
        }
    }
}