using System;
using System.IO;


namespace Eshunna.Lib.OBJ
{
    public class MaterialFileSerializer
    {
        public static void Serialize(string filePath, MaterialFile materialFile)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(
@"newmtl material0
Ka 1.000000 1.000000 1.000000
Kd 1.000000 1.000000 1.000000
Ks 0.000000 0.000000 0.000000
Tr 1.000000
illum 1
Ns 0.000000"
                );

                string line = $@"map_Kd {materialFile.TextureFileRelativePath}";
                writer.WriteLine(line);
            }
        }
    }
}
