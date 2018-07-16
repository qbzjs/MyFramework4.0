using System.Runtime.InteropServices;
using UnityEngine;

namespace MyFramework
{
    public static class MyIOSInterface
    {
        [DllImport("__Internal")]
        private static extern void _Init();
        [DllImport("__Internal")]
        private static extern void _OpenPhotoLibrary();
        [DllImport("__Internal")]
        private static extern void _OpenPhotoLibrary_allowsEditing();
        [DllImport("__Internal")]
        private static extern void _SaveImageToPhotosAlbum(string readAddr);
        [DllImport("__Internal")]
        private static extern void _SaveVideoToPhotosAlbum(string readAddr);
        [DllImport("__Internal")]
        private static extern void _SharePictures(string readAddr,bool IsCircle);
        [DllImport("__Internal")]
        private static extern void _ShareVideo(string readAddr, bool IsCircle);

        //初始化借口
        public static void Init()
        {
            _Init();
        }

        /// <summary>
        /// 打开相册 是否可编辑
        /// </summary>
        public static void OpenAlbum(bool allowsEditing = false)
        {
            if (allowsEditing)
                _OpenPhotoLibrary_allowsEditing();
            else
                _OpenPhotoLibrary();
        }

        /// <summary>
        /// 保存图像到相册
        /// </summary>
        /// <param name="readAddr"></param>
        public static void SaveImageToPhotosAlbum(string readAddr)
        {
            LoggerHelper.Error("SAVE Iamge To PhotosAlbum:" + readAddr);
            _SaveImageToPhotosAlbum(readAddr);
        }

        /// <summary>
        /// 保存视屏到相册
        /// </summary>
        /// <param name="readAddr"></param>
        public static void SaveVideoToPhotosAlbum(string readAddr)
        {
            _SaveVideoToPhotosAlbum(readAddr);
        }

        /// <summary>
        /// 分享图片  是否是朋友圈
        /// </summary>
        /// <param name="readAddr"></param>
        public static void SharePictures(string readAddr,bool IsCircle)
        {
            _SharePictures(readAddr, IsCircle);
        }

        /// <summary>
        /// 分享视屏  是否是朋友圈
        /// </summary>
        /// <param name="readAddr"></param>
        public static void ShareVideo(string readAddr, bool IsCircle)
        {
            _ShareVideo(readAddr, IsCircle);
        }
    }
}
