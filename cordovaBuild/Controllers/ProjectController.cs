using cordovaBuild.Data.Model;
using cordovaBuild.Data.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public ActionResult BuildAndroid(Project project)
        {
            var projectRepo = new ProjectRepository();
            if (string.IsNullOrEmpty(project.Id.ToString().Trim()))
            {
                projectRepo.Create(project);
            }

            var build = new Build
            {
                ProjectId = project.Id,
                BuildType = "Android",
                Status = "Pulling source file from git",
                BuildDateTime = Helper.SetDateForMongo(DateTime.Now),
                Filename = string.Format("{0}{1}_AndroidBuild.txt", User.Identity.Name, DateTime.Now.Ticks)
            };

            var output = ProcessBuild("cordova build android", @"C:\fray\Projects\ADBuild\hello", @"c:\fray", build.Filename);
            return Json(new { success = true, responseText = output }, JsonRequestBehavior.AllowGet);
        }

        private void BuildAndroid(string projectId)
        {

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
                processInfo.CreateNoWindow = false;
                processInfo.UseShellExecute = false;
                // *** Redirect the output ***
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

                // *** Read the streams ***
                // Warning: This approach can lead to deadlocks, see Edit #2
                //string output = process.StandardOutput.ReadToEnd();
                //string error = process.StandardError.ReadToEnd();

                exitCode = process.ExitCode;
                process.Close();

            }
        }

        private Task ProcessBuild(string command, string workingDirectory, string outputDirectory, string outputFilename)
        {
            return Task.Factory.StartNew(() =>
            {
                ExecuteCommand(command, workingDirectory, outputDirectory, outputFilename);
            });
            
        }
    }
}