using AutoMapper;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using TestWS.Utils;

namespace TestWS.Reports
{
    public abstract class ReportBaseStrategy<T> : IReportBuilder
    {
        protected SQLDatabaseUtil DatabaseUtil { get; set; }
        public Dictionary<string, object> Parameters { get; set; }

        protected ReportBaseStrategy(IMapper mapper)
        {
            DatabaseUtil = new SQLDatabaseUtil(mapper);
            Parameters = new Dictionary<string, object>();
        }

        public string BuildReport()
        {
            var model = GetDataModel();
            CreateReportsDirectoryIfNotExists();

            var filename = Path.Combine(Constants.ReportsDirectory, string.Concat(InternalGetDownloadFileName(), DateTime.Now.ToString("_yyyyMMdd-hhmmss"), GetTargetExtension()));
            
            InternalBuildReport(filename, model);

            return GetFileLinkURL(filename);
        }

        protected string TemplateFileName => InternalGetTemplateFileName();

        protected abstract string InternalGetTemplateFileName();
        protected abstract string InternalGetDownloadFileName();
        protected abstract T GetDataModel();

        protected string GetFileLinkURL(string filePath)
        {
            var approot = HostingEnvironment.ApplicationPhysicalPath.TrimEnd('\\');
            return filePath.Replace(approot, string.Empty).Replace('\\', '/');
        }

        protected void InternalBuildReport(string filename, T model)
        {
            var templatePath = HostingEnvironment.MapPath(Path.Combine(Constants.ExcelTemplatesDirectory, TemplateFileName));
            if (string.IsNullOrEmpty(templatePath))
                throw new ApplicationException($"Unable to map path \"{templatePath}\".");

            using (var templateFileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                var workbook = WorkbookFactory.Create(templateFileStream);
                
                ProcessWorkbook(workbook, model);

                SaveWorkbook(workbook, filename);
            }
        }

        protected abstract void ProcessWorkbook(IWorkbook workbook, T model);

        private void SaveWorkbook(IWorkbook workbook, string filename)
        {
            var targetFilePath = HostingEnvironment.MapPath(filename);
            if (string.IsNullOrEmpty(targetFilePath))
                throw new ApplicationException($"Unable to map path \"{targetFilePath}\".");

            if (File.Exists(targetFilePath))
                File.Delete(targetFilePath);

            using (var outputFileStream = new FileStream(targetFilePath, FileMode.CreateNew))
            {
                workbook.Write(outputFileStream);
                outputFileStream.Close();
            }            
        }

        private object GetTargetExtension()
        {
            return Path.GetExtension(TemplateFileName);
        }

        private static void CreateReportsDirectoryIfNotExists()
        {
            try
            {
                var reportsPath = HostingEnvironment.MapPath(Constants.ReportsDirectory);
                if (string.IsNullOrEmpty(reportsPath))
                    throw new ApplicationException($"Unable to map path \"{reportsPath}\".");

                if (!Directory.Exists(reportsPath))
                    Directory.CreateDirectory(reportsPath);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Unable to create reports directory.", e);
            }
        }
    }
}