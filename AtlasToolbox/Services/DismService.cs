using Microsoft.Dism;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtlasToolbox.Services
{
    /// <summary>
    /// Registers the DismService
    /// </summary>
    public class DismService : IDismService, IDisposable
    {
        private bool _isDisposed;
        private const string LOG_FILE_PATH = "dism.log";

        public DismService()
        {
            string installLocation = AppDomain.CurrentDomain.BaseDirectory;
            DismApi.Initialize(DismLogLevel.LogErrors, installLocation + LOG_FILE_PATH);
        }

        public DismPackageFeatureState GetFeatureState(string featureName)
        {
            using DismSession session = DismApi.OpenOnlineSession();
            DismFeatureInfo featureInfo = DismApi.GetFeatureInfo(session, featureName);

            return featureInfo.FeatureState;
        }

        public bool IsFeatureStateMatch(string featureName, DismPackageFeatureState featureState)
        {
            return GetFeatureState(featureName) == featureState;
        }

        public void EnableFeature(string featureName)
        {
            using DismSession session = DismApi.OpenOnlineSession();
            DismApi.EnableFeature(session, featureName, true, false);
        }

        public void DisableFeature(string featureName)
        {
            using DismSession session = DismApi.OpenOnlineSession();
            DismApi.DisableFeature(session, featureName, null, false);
        }

        public void RemoveProvisionedAppxPackage(string packageName)
        {
            using DismSession session = DismApi.OpenOnlineSession();
            IEnumerable<DismAppxPackage> matchedPackages = DismApi
                .GetProvisionedAppxPackages(session)
                .Where(x => x.PackageName.Contains(packageName));

            foreach (DismAppxPackage package in matchedPackages)
            {
                DismApi.RemoveProvisionedAppxPackage(session, package.PackageName);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    DismApi.Shutdown();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
