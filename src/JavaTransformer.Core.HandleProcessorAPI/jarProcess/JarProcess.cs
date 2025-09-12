using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ru.megantcs.core.common
{
    /**
     * Old Code
     */
    public class JarProcess
    {
        public class Buffer
        {
            const string temp = "tools/temp";

            string name;
            string path;

            public Buffer(string pth)
            {
                FileInfo fileInfo = new FileInfo(pth);

                path = pth;
                name = fileInfo.Name;

                __init__();
            }

            private void swap()
            {
                try
                {
                    if (!Directory.Exists(temp))
                    {
                        Directory.CreateDirectory(temp);
                    }
                    else
                    {
                        Directory.Delete(temp, true);
                        Directory.CreateDirectory(temp);
                    }
                }
                catch { }
            }

            private void __init__()
            {
                swap();
                create();
            }

            public void create()
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException(path);

                ZipFile.ExtractToDirectory(path, getTemp());
            }

            public void clear()
            {
                swap();
            }

            public FileInfo[] GetFiles()
            {
                return GetDirectory().GetFiles();
            }

            public FileInfo[] GetFiles(string pattern)
            {
                return GetDirectory().GetFiles(pattern, SearchOption.AllDirectories);
            }


            public DirectoryInfo GetDirectory()
            {
                return new DirectoryInfo(getTemp());
            }

            public string getTemp()
            {
                return temp + "\\" + name;
            }
        }

        public JarInfo getInfo(string path)
        {
            bool searched = false;
            JarInfo info = new JarInfo();
            Buffer buffer = new Buffer(path);

            info.Path = path;

            buffer.clear();
            buffer.create();

            foreach (var file in buffer.GetFiles("*.json"))
            {
                var class_detected = JsonDocument.SearchProperty(file.FullName, "entrypoints.client");
                if (class_detected == null)
                    continue;

                info.ClassLoad = (String)class_detected;

                   // see interface fabric ClientModInitialize
                info.MethodLoad = "OnInitialize";


                searched = true;
            }

            if (!searched)
            {
                foreach (var file in buffer.GetFiles("*.class"))
                {
                    /**
                     * the package that contains the @Mod. 
                     * annotation package The Forge entry point is located 
                     * in the constructor of the class that contains the @Mod annotation
                     */
                    if (JavaNativeLibraries.HasAnnotationManual(file.FullName, "Lnet/minecraftforge/fml/common/Mod;"))
                    {
                        /**
                         * jni (java native interface) call constructor
                         */
                        info.MethodLoad = "<init>"; 

                        /** getting the class signature.
                            Which contains the annotation: Lnet/minecraftforge/fml/common/Mod;
                        */
                        info.ClassLoad = JavaNativeLibraries.jvmti_get_signature(file.FullName);

                        searched = true;
                        break;
                    }
                }
            }

            buffer.clear();
            

            return info;
        }
    }
}
