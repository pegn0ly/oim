using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.Networking;

namespace OIMWeb
{
    // класс, непосредственно отправляющий запросы в бэкенд.
    public static class DjangoConnector
    {
        static private readonly string RootURL = "http://127.0.0.1:8000/";

        static private readonly SortedList<string, string> DefaultHeaders = new SortedList<string, string>()
        {
            {"Authorization", "Token 501b98e9c3c179316821d06faab144d1b67b4289"},
        };

        public static IEnumerator PostData(ContentRequest request, Action<object> Callback)
        {
            WWWForm form = new WWWForm();
            foreach(KeyValuePair<string, string> kvp in request.Data)
            {
                form.AddField(kvp.Key, kvp.Value);
            }
            //Debug.Log("Form: " + form.ToString());
            UnityWebRequest rq = UnityWebRequest.Post(RootURL + request.URL, form);
            //Debug.Log("Posting " + request.Data + " to " + RootURL + request.URL);
            SetHeaders(ref rq, request.Headers);

            yield return rq.SendWebRequest();

            if(rq.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection error");
            }
            else if(rq.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Protocol error");
            }
            else
            {
                if(Callback != null)
                {
                    Callback(rq.downloadHandler.text);
                }
            }
        }

        public static IEnumerator GetData(ContentRequest request, Action<object> Callback)
        {
            WWWForm form = new WWWForm();
            if(request.Data != null)
            {
                foreach(KeyValuePair<string, string> kvp in request.Data)
                {
                    form.AddField(kvp.Key, kvp.Value);
                }
            }

            UnityWebRequest rq = UnityWebRequest.Get(RootURL + request.URL);

            SetHeaders(ref rq, request.Headers);

            yield return rq.SendWebRequest();

            switch (rq.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("Connection error");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Data processing error");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("Protocol error");
                    break;
                case UnityWebRequest.Result.Success:
                    Callback(rq.downloadHandler.text);
                    break;
                default:
                    break;
            }
        }

        public static IEnumerator PutData(ContentRequest request, Action<object> Callback)
        {
            UnityWebRequest rq = UnityWebRequest.Put(RootURL + request.URL, JsonConvert.SerializeObject(request.Data));
            SetHeaders(ref rq, request.Headers);
            yield return rq.SendWebRequest();

            switch (rq.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("Connection error");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Data processing error");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("Protocol error");
                    break;
                case UnityWebRequest.Result.Success:
                    Callback(rq.downloadHandler.text);
                    break;
                default:
                    break;
            } 
        }

        private static void SetHeaders(ref UnityWebRequest request, SortedList<string, string> headers)
        {
            foreach(KeyValuePair<string, string> kvp in DjangoConnector.DefaultHeaders)
            {
                request.SetRequestHeader(kvp.Key, kvp.Value);
            }
            if(headers != null)
            {
                foreach(KeyValuePair<string, string> kvp in headers)
                {
                    //Debug.Log("Adding header " + kvp.Key + " with value of " + kvp.Value);
                    request.SetRequestHeader(kvp.Key, kvp.Value);
                }
            }
        }
    }
}