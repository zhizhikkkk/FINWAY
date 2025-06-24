using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

public static class UnityWebRequestExtensions
{
    public static Task<UnityWebRequest> SendWebRequestAsync(this UnityWebRequest unityWebRequest)
    {
        var tcs = new TaskCompletionSource<UnityWebRequest>();

        var operation = unityWebRequest.SendWebRequest();

        operation.completed += _ =>
        {
#if UNITY_2020_1_OR_NEWER
            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError ||
                unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
#else
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
#endif
            {
                tcs.SetException(new Exception(unityWebRequest.error));
            }
            else
            {
                tcs.SetResult(unityWebRequest);
            }
        };

        return tcs.Task;
    }
}
