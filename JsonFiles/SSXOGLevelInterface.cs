using SSXMultiTool.FileHandlers.LevelFiles.OGPS2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.JsonFiles
{
    public class SSXOGLevelInterface
    {

        public void ExtractOGLevelFiles(string LoadPath, string ExtractPath)
        {
            WDXHandler wdxHandler = new WDXHandler();
            wdxHandler.Load(LoadPath + ".wdx");
        }

    }
}
