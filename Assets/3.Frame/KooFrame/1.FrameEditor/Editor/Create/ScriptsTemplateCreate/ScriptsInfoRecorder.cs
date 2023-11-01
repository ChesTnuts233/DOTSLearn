using UnityEngine;

namespace KooFrame
{
#if UNITY_EDITOR


    public class ScriptsInfoRecorder : UnityEditor.AssetModificationProcessor
    {
        private static void OnWillCreateAsset(string assetName)
        {
            //Debug.Log(assetName);
        }
    }
#endif
}