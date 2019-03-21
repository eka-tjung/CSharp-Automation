using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;

namespace CUTests.Utilities
{
    public class GradesManagerHelper
    {
        private Application _testApp;
        private Window _testWindow;

        public Application GetTestApp()
        {
            return this._testApp;
        }

        public Window GetTestWindow()
        {
            return this._testWindow;
        }

        public void LaunchGradesManager()
        {
            String pathToClass = Environment.CurrentDirectory;
            String pathToExe = pathToClass + @"\ContosoUniversityGrades\bin\debug\ContosoUniversityGrades.exe";
            _testApp = Application.Launch(pathToExe);
            var windows = _testApp.GetWindows();
            _testWindow = _testApp.GetWindow("Contoso University Grades", InitializeOption.NoCache);
        }
    }
}
