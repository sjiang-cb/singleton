﻿/*
 * Copyright 2020-2021 VMware, Inc.
 * SPDX-License-Identifier: EPL-2.0
 */

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;

namespace SingletonClient.Implementation.Support
{
    public class SingletonAccessService : IAccessService
    {
        /// <summary>
        /// IAccessService
        /// </summary>
        public string HttpGet(string url, Hashtable headers, int timeout, ILog logger = null)
        {
            string result = "";

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                if (timeout > 0)
                {
                    req.Timeout = timeout * 1000;
                }
                req.Method = "GET";

                if (headers != null)
                {
                    foreach (DictionaryEntry item in headers)
                    {
                        req.Headers.Add((string)item.Key, (string)item.Value);
                    }
                }

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();

                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }

                if (headers != null)
                {
                    headers.Clear();

                    string[] keys = resp.Headers.AllKeys;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        headers.Add(keys[i].ToLower(), resp.Headers.Get(keys[i]));
                    }
                }
            }
            catch (WebException e)
            {
                HttpWebResponse resp = (HttpWebResponse)e.Response;
                if (headers != null && resp != null)
                {
                    headers.Clear();
                    headers.Add(SingletonConst.HeaderResponseCode, resp.StatusCode.ToString());
                }
                if (logger != null)
                {
                    logger.Log(LogType.Info, e.Message);
                }
                return null;
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    logger.Log(LogType.Info, e.Message);
                }
                return null;
            }
            return result;
        }

        /// <summary>
        /// IAccessService
        /// </summary>
        public string HttpPost(string url, string text, Hashtable headers, int timeout, ILog logger = null)
        {
            if (text == null)
            {
                return null;
            }

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            byte[] bs = Encoding.UTF8.GetBytes(text);
            string responseData = null;
            if (timeout > 0)
            {
                req.Timeout = timeout * 1000;
            }
            req.Method = "POST";
            req.ContentType = "application/json";
            req.ContentLength = bs.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = reader.ReadToEnd().ToString();
                    }
                }
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    logger.Log(LogType.Info, e.Message);
                }
                return null;
            }

            return responseData;
        }
    }
}

