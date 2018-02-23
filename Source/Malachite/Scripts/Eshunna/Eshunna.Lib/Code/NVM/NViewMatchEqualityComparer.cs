using System;
using System.Collections.Generic;
using System.Linq;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.NVM
{
    public class NViewMatchEqualityComparer : IEqualityComparer<NViewMatch>
    {
        public bool Equals(NViewMatch x, NViewMatch y)
        {
            bool output = true;

            int nCamerasX = x.Cameras.Length;
            int nCamerasY = y.Cameras.Length;
            bool cameraCountMatch = nCamerasX == nCamerasY;
            if(!cameraCountMatch)
            {
                output = false;

                string message = $@"Camera count mismatch- X: {nCamerasX}, Y: {nCamerasY}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iCamera = 0; iCamera < nCamerasX; iCamera++)
                {
                    Camera cameraX = x.Cameras[iCamera];
                    Camera cameraY = y.Cameras[iCamera];

                    bool cameraMatch = this.CameraComparer.Equals(cameraX, cameraY);
                    if(!cameraMatch)
                    {
                        output = false;

                        string message = $@"Camera mismatch: {iCamera}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            int nFeaturePointsX = x.FeaturePoints.Length;
            int nFeaturePointsY = y.FeaturePoints.Length;
            bool featurePointCountMatch = nFeaturePointsX == nFeaturePointsY;
            if(!featurePointCountMatch)
            {
                output = false;
                string message = $@"Feature point count mismatch- X: {nFeaturePointsX}, Y: {nFeaturePointsY}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iFeaturePoint = 0; iFeaturePoint < nFeaturePointsX; iFeaturePoint++)
                {
                    FeaturePoint featurePointX = x.FeaturePoints[iFeaturePoint];
                    FeaturePoint featurePointY = y.FeaturePoints[iFeaturePoint];

                    bool featurePointMatch = this.FeaturePointComparer.Equals(featurePointX, featurePointY);
                    if(!featurePointMatch)
                    {
                        output = false;

                        string message = $@"Feature point mismatch: {iFeaturePoint}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            int nModelsWithPlyX = x.ModelIndicessWithPlyFiles.Length;
            int nModelsWithPlyY = y.ModelIndicessWithPlyFiles.Length;
            bool modelsWithPlyCountMatch = nModelsWithPlyX == nModelsWithPlyY;
            if(!modelsWithPlyCountMatch)
            {
                output = false;
                string message = $@"Models with ply count mismatch- X: {nModelsWithPlyX}, Y: {nModelsWithPlyY}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iModelWithPly = 0; iModelWithPly < nModelsWithPlyX; iModelWithPly++)
                {
                    int indexX = x.ModelIndicessWithPlyFiles[iModelWithPly];
                    int indexY = y.ModelIndicessWithPlyFiles[iModelWithPly];

                    bool indexMatch = indexX == indexY;
                    if(!indexMatch)
                    {
                        output = false;

                        string message = $@"Model with PLY index mismatch: {iModelWithPly}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            //bool output =
            //    x.Cameras.SequenceEqual(y.Cameras, this.CameraComparer) &&
            //    x.FeaturePoints.SequenceEqual(y.FeaturePoints, this.FeaturePointComparer) &&
            //    x.ModelIndicessWithPlyFiles.SequenceEqual(y.ModelIndicessWithPlyFiles);
            return output;
        }

        public int GetHashCode(NViewMatch obj)
        {
            int output = obj.GetHashCode(); // Just use the reference-class default since N-view matches are large objects.
            return output;
        }


        public IEqualityComparer<Camera> CameraComparer { get; }
        public IEqualityComparer<FeaturePoint> FeaturePointComparer { get; }
        public ILog Log { get; }


        public NViewMatchEqualityComparer(IEqualityComparer<Camera> cameraComparer, IEqualityComparer<FeaturePoint> featurePointComparer, ILog log)
        {
            this.CameraComparer = cameraComparer;
            this.FeaturePointComparer = featurePointComparer;
            this.Log = log;
        }

        public NViewMatchEqualityComparer(ILog log)
            : this(new CameraEqualityComparer(), new FeaturePointComparer(), log)
        {
        }

        public NViewMatchEqualityComparer()
            : this(new DummyLog())
        {
        }
    }
}
