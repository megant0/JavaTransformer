using ru.megantcs.core.common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class JarInfo
{
    public String Path;
    public String ClassLoad { get; set; }
    public String MethodLoad { get; set; }

    public override string ToString()
    {
        return $"Path=" + Path + ", Target Class=" + ClassLoad + ", Target Method: " + MethodLoad;
    }
}

