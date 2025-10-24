using System.IO;
using UnityEngine;

namespace ShapezShifter.Textures
{
    public static class FileTextureLoader
    {
        public static Texture2D LoadTexture(string path)
        {
            byte[] data = File.ReadAllBytes(path);

            Texture2D texture = new(2, 2);
            texture.LoadImage(data);
            return texture;
        }

        public static Sprite LoadTextureAsSprite(string path, out Texture2D texture)
        {
            byte[] data = File.ReadAllBytes(path);

            texture = new Texture2D(2, 2);
            texture.LoadImage(data);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}