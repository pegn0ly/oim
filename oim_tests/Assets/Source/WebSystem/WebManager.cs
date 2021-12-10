using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OIMWeb
{
    public class WebManager : MonoBehaviour
    {
        // GET request
        public void Get(ContentRequest request, Action<object> Callback)
        {
            StartCoroutine(DjangoConnector.GetData(request, Callback));
        }

        // POST request
        public void Post(ContentRequest request, Action<object> Callback)
        {
            StartCoroutine(DjangoConnector.PostData(request, Callback));
        }

        // PUT request
        public void Put(ContentRequest request, Action<object> Callback)
        {
            StartCoroutine(DjangoConnector.PutData(request, Callback));
        }
    }

    public class ContentRequest
    {
        private string _URL;
        public string URL
        {
            get
            {
                return _URL;
            }
        }

        private Dictionary<string, string> _Data;
        public Dictionary<string, string> Data
        {
            get
            {
                return _Data;
            }
        }

        private SortedList<string, string> _Headers;
        public SortedList<string, string> Headers
        {
            get
            {
                return _Headers;
            }
        }

        public ContentRequest(string url, Dictionary<string, string> data = null, SortedList<string, string> headers = null)
        {
            _URL = url;
            _Data = data;
            _Headers = headers;
        }
    }
}