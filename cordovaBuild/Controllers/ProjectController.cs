﻿using cordovaBuild.Data.Model;
using cordovaBuild.Data.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace cordovaBuild.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult BuildLog(string projectId, string buildType)
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> BuildAndroid(Project project)
        {
            var projectRepo = new ProjectRepository();
            if (project.Id.ToString().Trim() == "000000000000000000000000")
            {
                project.DateCreated = Helper.SetDateForMongo(DateTime.Now);
                project.User = User.Identity.Name;
                await projectRepo.CreateSync(project);
            }

            var output = ProcessBuild(project, "android", User.Identity.Name);
            return Json(new { success = true, responseText = output }, JsonRequestBehavior.AllowGet);
        }

        private Task ProcessBuild(Project project, string buildType, string user)
        {
            return Task.Factory.StartNew(() =>
            {
                var baseWorkingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"].ToString();
                var baseOutputDirectory = ConfigurationManager.AppSettings["OutputDirectory"].ToString();

                var workingDirectory = Path.Combine(baseWorkingDirectory, DateTime.Now.Ticks.ToString());
                if (!Directory.Exists(workingDirectory))
                {
                    Directory.CreateDirectory(workingDirectory);
                }

                var outputDirectory = Path.Combine(baseOutputDirectory, string.Format("{0}\\{1}\\{2}", user, project.Id.ToString(), buildType));
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                var buildRepo = new BuildRepository();
                var ticks = DateTime.Now.Ticks.ToString();
                var build = new Build
                {
                    ProjectId = project.Id,
                    BuildType = buildType,
                    Status = "Pulling source files",
                    BuildDateTime = Helper.SetDateForMongo(DateTime.Now),
                    BuildFileLog = string.Format("{0}{1}_{2}Build.txt", user, ticks, buildType),
                    GitFileLog = string.Format("{0}{1}_{2}GitPull.txt", user, ticks, buildType),
                    OutputZipFile = string.Format("{0}{1}_{2}Build.zip", user, ticks, buildType),
                };
                buildRepo.Create(build);

                var gitCommand = string.Format("git clone https://{0}:{1}@{2}", project.GitUsername, project.GitPassword, project.GitUrl.Replace("https://", ""));

                ExecuteCommand(gitCommand, workingDirectory, outputDirectory, build.GitFileLog);

                var workingDirectoryInfo = new DirectoryInfo(workingDirectory);
                var projectFolderName = (workingDirectoryInfo.GetDirectories()).FirstOrDefault().ToString();

                if (!string.IsNullOrEmpty(projectFolderName))
                {
                    var projectFolder = Path.Combine(workingDirectory, projectFolderName);
                    var cordovaAddPlatformCommand = string.Format("cordova platform add {0}", buildType);

                    ExecuteCommand(cordovaAddPlatformCommand, projectFolder, outputDirectory, build.BuildFileLog);

                    var cordovaBuildCommand = string.Format("cordova build {0}", buildType);

                    ExecuteCommand(cordovaBuildCommand, projectFolder, outputDirectory, build.BuildFileLog);

                    var buildDirectory = Path.Combine(workingDirectory, string.Format("platforms\\{0}", buildType));
                    var zipFullPath = Path.Combine(outputDirectory, build.OutputZipFile);
                    ZipFile.CreateFromDirectory(buildDirectory, zipFullPath);
                    
                }

            });

        }

        private void BuildAndroid(string projectId)
        {

        }

        private SecureString convertToSecureString(string strPassword)
        {
            var secureStr = new SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }

        private void ExecuteCommand(string command, string workingDirectory, string outputDirectory, string outputFilename)
        {
            var file = Path.Combine(outputDirectory, outputFilename);
            using (var fileStream = System.IO.File.CreateText(file))
            {
                int exitCode;
                ProcessStartInfo processInfo;
                Process process;
                processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
                processInfo.WorkingDirectory = workingDirectory;
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo.UserName = ConfigurationManager.AppSettings["CommandUsername"].ToString();
                processInfo.Password = convertToSecureString(ConfigurationManager.AppSettings["CommandPassword"].ToString());
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;

                process = Process.Start(processInfo);

                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                    fileStream.WriteLine(e.Data);
                process.BeginOutputReadLine();

                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                    fileStream.WriteLine(e.Data);

                process.BeginErrorReadLine();

                process.WaitForExit();

                exitCode = process.ExitCode;
                process.Close();

            }
        }


    }
}