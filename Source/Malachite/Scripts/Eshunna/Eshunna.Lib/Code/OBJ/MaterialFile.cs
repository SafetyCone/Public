using System;


namespace Eshunna.Lib.OBJ
{
    public class MaterialFile
    {
        public string TextureFileRelativePath { get;set;}


        public MaterialFile(string textureFileRelativePath)
        {
            this.TextureFileRelativePath = textureFileRelativePath;
        }
    }
}
