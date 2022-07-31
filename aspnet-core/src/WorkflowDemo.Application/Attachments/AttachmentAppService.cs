using Abp.Application.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using WorkflowDemo.Attachments.Dtos;

namespace WorkflowDemo.Attachments
{
    /// <summary>
    /// 附件管理
    /// </summary>
    public class AttachmentAppService : ApplicationService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        public async Task<string> SingleUpload([FromForm]IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("form");
            }

            string ext = Path.GetExtension(file.FileName);
            string realName = $"{Guid.NewGuid()}{ext}";
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string fileName = Path.Combine(folder, realName);

            using (FileStream fs = File.Create(fileName))
            {
                await file.CopyToAsync(fs);
            }

            return realName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        public async Task<string> Upload([FromForm]FormFileWrapper wrapper)
        {
            if (wrapper == null || wrapper.FormFile == null)
            {
                throw new ArgumentNullException("file");
            }

            string ext = Path.GetExtension(wrapper.FormFile.FileName);
            string realName = $"{wrapper.Tag}-{Guid.NewGuid()}{ext}";
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", realName);

            using (FileStream fs = File.Create(fileName))
            {
                await wrapper.FormFile.CopyToAsync(fs);
            }

            return realName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        public async Task<List<string>> UploadList([FromForm]FormFileCollectionWrapper wrapper)
        {
            if (wrapper == null)
            {
                throw new ArgumentNullException("wrapper");
            }

            List<string> files = new List<string>();

            foreach (var file in wrapper.FileCollection)
            {
                string ext = Path.GetExtension (file.FileName);
                string realName = $"{Guid.NewGuid()}{ext}";
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", realName);
                using (FileStream fs = File.Create(fileName))
                {
                    await file.CopyToAsync(fs);
                }
                files.Add(realName);
            }

            return files;
        }
    }
}
