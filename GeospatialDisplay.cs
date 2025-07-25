using UnityEngine;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARSubsystems;

public class GeospatialDisplay : MonoBehaviour
{
    public ARCoreExtensions arCoreExtensions;
    public AREarthManager earthManager;

    public GeospatialPose? CurrentPose { get; private set; }

    void Update()
    {
        if (earthManager == null || earthManager.EarthTrackingState != TrackingState.Tracking)
        {
            CurrentPose = null;
            return;
        }

        CurrentPose = earthManager.CameraGeospatialPose;
    }
}
