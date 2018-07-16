using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MyFramework
{
    /// <summary>
    /// 拍照相册工具
    /// </summary>
    public static class PhotographTool
    {

        /// <summary>  
        /// 屏幕截图保存
        /// 左下角为(0,0)  
        /// </summary>  
        /// <param name="mRect">M rect.</param>  
        /// <param name="mFileName">M file name.</param>  
        public static Texture2D PhotoSave(string path)
        {
            Texture2D mTexture = new Texture2D((int)Screen.width, (int)Screen.height, TextureFormat.RGB24, false);
            mTexture.ReadPixels(new Rect(0,0, (int)Screen.width, (int)Screen.height), 0, 0); 
            mTexture.Apply();
            byte[] bytes = mTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, bytes);
            return mTexture;
        }

        /// <summary>
        /// 相机截图保存
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Sprite PhotoSave(Camera cam,string path)
        {

            RenderTexture mRender = new RenderTexture((int)Screen.width, (int)Screen.height, 3);
            cam.targetTexture = mRender;
            cam.Render();
            RenderTexture.active = mRender;
            Texture2D mTexture = new Texture2D((int)Screen.width, (int)Screen.height, TextureFormat.RGB24, false);
            mTexture.ReadPixels(new Rect(0,0, (int)Screen.width, (int)Screen.height), 0, 0);
            mTexture.Apply();
            cam.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(mRender); 
            byte[] bytes = mTexture.EncodeToPNG();

            string targetDir = Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(targetDir))
            {
                System.IO.Directory.CreateDirectory(targetDir);
            }
            System.IO.File.WriteAllBytes(path, bytes);
            return Sprite.Create(mTexture, new Rect(0, 0, (int)Screen.width, (int)Screen.height), Vector2.zero); ;  
        }
    }
}
