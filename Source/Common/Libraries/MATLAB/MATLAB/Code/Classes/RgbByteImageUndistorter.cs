using System.Dynamic;

using Public.Common.Lib.Visuals;
using Public.Common.Lib.Visuals.MATLAB;


namespace Public.Common.MATLAB
{
    public class RgbByteImageUndistorter : IRgbByteImageUndistorter
    {
        #region Static

        public static void LoadCameraParametersFileToVariable(MatlabApplication matlabApplication, string cameraParametersFilePath, string variableName)
        {
            matlabApplication.LoadSpecific(cameraParametersFilePath, variableName);
        }

        public static string LoadCameraParametersFileToVariable(MatlabApplication matlabApplication, string cameraParametersFilePath)
        {
            string variableName = RgbByteImageUndistorter.GetCameraParametersTempVariableName();
            RgbByteImageUndistorter.LoadCameraParametersFileToVariable(matlabApplication, cameraParametersFilePath, variableName);

            return variableName;
        }

        public static string GetCameraParametersTempVariableName()
        {
            string output = Matlab.GetTemporaryVariableName(@"cameraParams");
            return output;
        }

        #endregion


        private MatlabApplication MatlabApplication { get; }
        private string CameraParametersVariableName { get; set; }


        public RgbByteImageUndistorter(MatlabApplication matlabApplication, string camerParametersVariableName)
        {
            this.MatlabApplication = matlabApplication;
            this.CameraParametersVariableName = camerParametersVariableName;
        }

        public RgbByteImageUndistorter(MatlabApplication matlabApplication)
        {
            this.MatlabApplication = matlabApplication;
        }

        public RgbByteImage Undistort(RgbByteImage image)
        {
            using (Variable imageVar = new Variable(this.MatlabApplication))
            using (Variable undistortedImageVar = new Variable(this.MatlabApplication))
            {
                // Marshall the image to MATLAB.
                this.MatlabApplication.PutRgbImage(imageVar.Name, image);

                // Undistort the image using the camera parameters.
                string command = $@"{undistortedImageVar.Name} = undistortImage({imageVar.Name}, {this.CameraParametersVariableName});";
                this.MatlabApplication.Execute(command);

                // Marshall the undistorted image back to MATLAB.
                RgbByteImage output = this.MatlabApplication.GetRgbImage(undistortedImageVar.Name);
                return output;
            }
        }

        public void PutCameraParameters(ExpandoObject cameraParameters)
        {
            this.CameraParametersVariableName = RgbByteImageUndistorter.GetCameraParametersTempVariableName();
            this.MatlabApplication.PutStructure(this.CameraParametersVariableName, cameraParameters);
        }

        public RgbByteImageUndistorter LoadCameraParmeters(string cameraParametersFilePath)
        {
            this.CameraParametersVariableName = RgbByteImageUndistorter.LoadCameraParametersFileToVariable(this.MatlabApplication, cameraParametersFilePath);

            return this;
        }
    }
}
