using UnityEngine;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System;

namespace MyFramework
{
    /// <summary>
    /// 文件压缩工具
    /// </summary>
    public static class ZipTools
    {
        public delegate void ZipOpeProgress(string Describe,float Progress);
        #region 压缩
        /// <summary>
        /// 压缩文件和文件夹
        /// </summary>
        /// <param name="_fileOrDirectoryArray">文件夹路径和文件名</param>
        /// <param name="_outputPathName">压缩后的输出路径文件名</param>
        /// <param name="_password">压缩密码</param>
        /// <param name="_zipCallback">ZipCallback对象，负责回调</param>
        /// <returns></returns>
        public static IEnumerator Zip(string[] _fileOrDirectoryArray, string _outputPathName, string _password = null,string[] filter = null, ZipOpeProgress zipback = null)
        {
            float Progress = 0;
            string Describe = string.Empty;
            ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(_outputPathName));
            zipOutputStream.SetLevel(6);
            if (!string.IsNullOrEmpty(_password))
                zipOutputStream.Password = _password;

            for (int index = 0; index < _fileOrDirectoryArray.Length; ++index)
            {
                string fileOrDirectory = _fileOrDirectoryArray[index];
                if (Directory.Exists(fileOrDirectory))
                    yield return ZipDirectory(fileOrDirectory, string.Empty, zipOutputStream, filter);
                else if (File.Exists(fileOrDirectory))
                    yield return ZipFile(fileOrDirectory, string.Empty, zipOutputStream, filter);
                if (zipback != null)
                {
                    Describe = "正在压缩 " + fileOrDirectory;
                    Progress = (float)index / (float)_fileOrDirectoryArray.Length;
                    zipback(Describe, Progress);
                }
            }

            zipOutputStream.Finish();
            zipOutputStream.Close();
            if (zipback != null)
                zipback("压缩完毕", 1);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="_filePathName">文件路径名</param>
        /// <param name="_parentRelPath">要压缩的文件的父相对文件夹</param>
        /// <param name="_zipOutputStream">压缩输出流</param>
        /// <param name="_zipCallback">ZipCallback对象，负责回调</param>
        /// <returns></returns>
        private static IEnumerator ZipFile(string _filePathName, string _parentRelPath, ZipOutputStream _zipOutputStream, string[] filter = null)
        {
            bool Isfilter = false;
            if (filter != null)
            {
                for (int n = 0; n < filter.Length; n++)
                {
                    if (_filePathName.EndsWith(filter[n]))
                    {
                        Isfilter = true;
                        break;
                    }
                }
            }

            if (!Isfilter)
            {
                ZipEntry entry = null;
                FileStream fileStream = null;

                string entryName = _parentRelPath + '/' + Path.GetFileName(_filePathName);
                entry = new ZipEntry(entryName);
                entry.DateTime = DateTime.Now;
                fileStream = File.OpenRead(_filePathName);
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                fileStream.Close();
                entry.Size = buffer.Length;
                _zipOutputStream.PutNextEntry(entry);
                _zipOutputStream.Write(buffer, 0, buffer.Length);
                if (null != fileStream)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            yield return new WaitForEndOfFrame();
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="_path">要压缩的文件夹</param>
        /// <param name="_parentRelPath">要压缩的文件夹的父相对文件夹</param>
        /// <param name="_zipOutputStream">压缩输出流</param>
        /// <param name="_zipCallback">ZipCallback对象，负责回调</param>
        /// <returns></returns>
        private static IEnumerator ZipDirectory(string _path, string _parentRelPath, ZipOutputStream _zipOutputStream, string[] filter = null)
        {
            ZipEntry entry = null;

            string entryName = Path.Combine(_parentRelPath, Path.GetFileName(_path) + '/');
            entry = new ZipEntry(entryName);
            entry.DateTime = System.DateTime.Now;
            entry.Size = 0;
            _zipOutputStream.PutNextEntry(entry);
            _zipOutputStream.Flush();
            string[] files = Directory.GetFiles(_path);
            for (int index = 0; index < files.Length; ++index)
            {
                yield return ZipFile(files[index], Path.Combine(_parentRelPath, Path.GetFileName(_path)), _zipOutputStream, filter);
            }

            string[] directories = Directory.GetDirectories(_path);
            for (int index = 0; index < directories.Length; ++index)
            {
                yield return ZipDirectory(directories[index], Path.Combine(_parentRelPath, Path.GetFileName(_path)), _zipOutputStream, filter);
            }

        }
        #endregion


        #region 解压
        /// <summary>
        /// 解压Zip包
        /// </summary>
        /// <param name="_filePathName">Zip包的文件路径名</param>
        /// <param name="_outputPath">解压输出路径</param>
        /// <param name="_password">解压密码</param>
        /// <param name="_unzipCallback">UnzipCallback对象，负责回调</param>
        /// <returns></returns>
        public static IEnumerator UnzipFile(string _filePathName, string _outputPath, string _password = null,ZipOpeProgress zipback = null)
        {
              yield return UnzipFile(File.OpenRead(_filePathName), _outputPath, _password, zipback);
        }

        /// <summary>
        /// 解压Zip包
        /// </summary>
        /// <param name="_fileBytes">Zip包字节数组</param>
        /// <param name="_outputPath">解压输出路径</param>
        /// <param name="_password">解压密码</param>
        /// <param name="_unzipCallback">UnzipCallback对象，负责回调</param>
        /// <returns></returns>
        public static IEnumerator UnzipFile(byte[] _fileBytes, string _outputPath, string _password = null, ZipOpeProgress zipback = null)
        {

           yield return UnzipFile(new MemoryStream(_fileBytes), _outputPath, _password, zipback);

        }


        /// <summary>
        /// 解压Zip包
        /// </summary>
        /// <param name="_inputStream">Zip包输入流</param>
        /// <param name="_outputPath">解压输出路径</param>
        /// <param name="_password">解压密码</param>
        /// <param name="_unzipCallback">UnzipCallback对象，负责回调</param>
        /// <returns></returns>
        public static IEnumerator UnzipFile(Stream _inputStream, string _outputPath, string _password = null, ZipOpeProgress zipback = null)
        {
            float Progress = 0;
            // 创建文件目录
            if (!Directory.Exists(_outputPath))
                Directory.CreateDirectory(_outputPath);

            // 解压Zip包
            ZipEntry entry = null;
            using (ZipInputStream zipInputStream = new ZipInputStream(_inputStream))
            {
                if (!string.IsNullOrEmpty(_password))
                    zipInputStream.Password = _password;

                while (null != (entry = zipInputStream.GetNextEntry()))
                {
                    if (string.IsNullOrEmpty(entry.Name))
                        continue;
                    string filePathName = string.Empty;
                    if (entry.Name[0] == '\\' || entry.Name[0] == '/')
                    {
                        filePathName = Path.Combine(_outputPath, entry.Name.Substring(1));
                    }
                    else
                    {
                        filePathName = Path.Combine(_outputPath, entry.Name);
                    }

                    //创建文件目录
                    if (entry.IsDirectory)
                    {
                        Directory.CreateDirectory(filePathName);
                        continue;
                    }
                    using (FileStream fileStream = File.Create(filePathName))
                    {
                        byte[] bytes = new byte[1024];
                        while (true)
                        {
                            int count = zipInputStream.Read(bytes, 0, bytes.Length);
                            if (count > 0)
                                fileStream.Write(bytes, 0, count);
                            else
                            {
                                break;
                            }
                        }
                        if (zipback != null)
                        {
                            Progress = Progress + fileStream.Length / (float)_inputStream.Length;
                            zipback("正在解压文件:"+ entry.Name, Progress);
                        }
                        yield return new WaitForEndOfFrame();
                    }

                }
            }
        }
        #endregion
    }
}

