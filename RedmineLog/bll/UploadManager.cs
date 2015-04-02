using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Text;

public class UploadManager
{

    public enum HttpMethod
    {
        POST,
        GET
    }

    #region "Fields"

    private int _timeout;
    private Version _httpProtocolVersion;
    private bool _keepAlive;
    private bool _isBusy;
    private bool _operationCanceled;
    private WebHeaderCollection _requestHeaders;
    #endregion
    private System.Text.Encoding _encoding;

    #region "Delegates"

    //Event Handlers delegates
    public delegate void SendDataProgressChangedEventHandler(object sender, SendDataProgressChangedEventArgs e);
    public delegate void SendDataCompletedEventHandler(object sender, SendDataCompletedEventArgs e);

    //Async Operation Delegates
    private System.Threading.SendOrPostCallback _onProgressReportDelegate;

    private System.Threading.SendOrPostCallback _onSendDataCompletedDelegate;
    //Worker Delegates
    private delegate void WorkerEventHandler(string url, string method, System.Collections.Specialized.NameValueCollection postData, System.Collections.Specialized.NameValueCollection postFiles, System.ComponentModel.AsyncOperation asyncOp);

    #endregion

    #region "Events"

    public event SendDataProgressChangedEventHandler SendDataProgressChanged;
    public event SendDataCompletedEventHandler SendDataCompleted;

    #endregion

    #region "Construction"

    public UploadManager()
    {
        //Initialize fields with default values
        _timeout = 30000;
        //30 seconds
        //Most of the web servers support Http Protocol Version 1.1
        _httpProtocolVersion = HttpVersion.Version11;
        _keepAlive = true;
        _isBusy = false;
        _operationCanceled = false;
        //Load the default encoding UTF-8
        _encoding = new UTF8Encoding();
    }

    private void initializeDelegates()
    {
        _onProgressReportDelegate = new System.Threading.SendOrPostCallback(reportProgress);
        _onSendDataCompletedDelegate = new System.Threading.SendOrPostCallback(sendCompleted);
    }

    #endregion

    #region "Asyncronous Operations"

    public void SendDataAsync(string url, System.Collections.Specialized.NameValueCollection postData)
    {
        SendDataAsync(url, "POST", postData, null);
    }

    public void SendDataAsync(string url, string method, System.Collections.Specialized.NameValueCollection postData)
    {
        SendDataAsync(url, method, postData, null);
    }

    public void SendDataAsync(string url, string method, System.Collections.Specialized.NameValueCollection postData, System.Collections.Specialized.NameValueCollection postFiles)
    {
        //Check if UploadManager is busy
        if (this.IsBusy)
        {
            throw new InvalidOperationException("Cannot invoke SendDataAsync while it is already being invoked.");
        }

        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException("url", "url cannot be null or empty.");
        }

        initializeDelegates();

        System.ComponentModel.AsyncOperation asyncOp = System.ComponentModel.AsyncOperationManager.CreateOperation(null);
        WorkerEventHandler workerDelegate = new WorkerEventHandler(sendDataWorker);

        workerDelegate.BeginInvoke(url, method, postData, postFiles, asyncOp, null, null);

    }

    private void sendDataWorker(string url, string method, System.Collections.Specialized.NameValueCollection postData, System.Collections.Specialized.NameValueCollection postFiles, System.ComponentModel.AsyncOperation asyncOp)
    {
        //Declare variables
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        StreamReader responseReader = null;
        FileStream localFileStream = null;

        //Boundary header is used for post stream.
        string boundaryHeader = "f93dcbA3";
        string boundary = "--" + boundaryHeader;
        string footer = System.Environment.NewLine + boundary + "--" + System.Environment.NewLine;

        //Set OperationCanceled flag to false
        _operationCanceled = false;

        //Build Header
        byte[] headerBytes = null;
        FileInfo picFile = null;
        byte[] footerBytes = _encoding.GetBytes(footer);

        SendDataProgressChangedEventArgs progressChanged = default(SendDataProgressChangedEventArgs);
        SendDataCompletedEventArgs operationCompleted = default(SendDataCompletedEventArgs);

        long bytesSent = 0;
        long totalBytesToSend = 0;
        int progressPercentage = 0;

        string curFileName = string.Empty;

        try
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            if (string.IsNullOrEmpty(method))
            {
                request.Method = "POST";
            }
            else
            {
                request.Method = method;
            }

            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundaryHeader);

            if (_requestHeaders != null)
            {
                request.Headers = _requestHeaders;
            }
            //Calculate total bytes to be sent
            request.ContentLength = calculateTotalSize(boundary, footer, postData, postFiles);
            request.ReadWriteTimeout = _timeout;
            request.KeepAlive = _keepAlive;
            request.ProtocolVersion = _httpProtocolVersion;
            //Calculate total bytes to be sent
            totalBytesToSend = request.ContentLength;

            Stream requestStream = request.GetRequestStream();

            if (postFiles != null)
            {
                foreach (string key in postFiles.AllKeys)
                {
                    //check if operation was canceled
                    if (_operationCanceled)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }

                    picFile = new FileInfo(postFiles[key]);
                    curFileName = picFile.Name;

                    //Write header
                    headerBytes = _encoding.GetBytes(buildFileHeader(boundary, key, postFiles.Get(key)));
                    requestStream.Write(headerBytes, 0, headerBytes.Length);

                    //Update progress
                    bytesSent += headerBytes.Length;
                    progressPercentage = Convert.ToInt32((Convert.ToSingle(bytesSent) / Convert.ToSingle(totalBytesToSend)) * 100);
                    progressChanged = new SendDataProgressChangedEventArgs(progressPercentage, bytesSent, totalBytesToSend, curFileName);

                    //Invoke the reportProgress delegate on the right thread for the application
                    asyncOp.Post(reportProgress, progressChanged);

                    //Open the file.
                    localFileStream = picFile.Open(FileMode.Open, FileAccess.Read);
                    int bufferSize = 2047;
                    byte[] buffer = new byte[bufferSize + 1];
                    int bytesRead = 0;

                    //Upload the file by writing the file content to the stream.
                    do
                    {
                        //check if operation was canceled
                        if (_operationCanceled)
                        {
                            localFileStream.Close();
                            break; // TODO: might not be correct. Was : Exit For
                        }

                        bytesRead = localFileStream.Read(buffer, 0, bufferSize);
                        requestStream.Write(buffer, 0, bytesRead);
                        //Update progress
                        bytesSent += bytesRead;
                        progressPercentage = Convert.ToInt32((Convert.ToSingle(bytesSent) / Convert.ToSingle(totalBytesToSend)) * 100);
                        progressChanged = new SendDataProgressChangedEventArgs(progressPercentage, bytesSent, totalBytesToSend, curFileName);

                        //Invokes the delegate on the appropriate thread for the application.
                        asyncOp.Post(reportProgress, progressChanged);
                    } while (bytesRead > 0);

                    localFileStream.Close();
                }
            }

            if (postData != null)
            {
                foreach (string key in postData.AllKeys)
                {
                    //check if operation was canceled
                    if (_operationCanceled)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }

                    //Write header
                    curFileName = key;
                    headerBytes = _encoding.GetBytes(buildDataHeader(boundary, key, postData.Get(key)));
                    requestStream.Write(headerBytes, 0, headerBytes.Length);

                    bytesSent += headerBytes.Length;

                    //Update progress
                    progressPercentage = Convert.ToInt32((Convert.ToSingle(bytesSent) / Convert.ToSingle(totalBytesToSend)) * 100);
                    progressChanged = new SendDataProgressChangedEventArgs(progressPercentage, bytesSent, totalBytesToSend, curFileName);
                    asyncOp.Post(reportProgress, progressChanged);
                }
            }

            //check if operation was canceled
            if (_operationCanceled)
            {
                request.Abort();
                operationCompleted = new SendDataCompletedEventArgs(null, true, null);
            }
            else
            {
                //Write footer
                requestStream.Write(footerBytes, 0, footerBytes.Length);
                requestStream.Close();

                bytesSent += footerBytes.Length;

                //Update progress
                progressPercentage = Convert.ToInt32((Convert.ToSingle(bytesSent) / Convert.ToSingle(totalBytesToSend)) * 100);
                progressChanged = new SendDataProgressChangedEventArgs(progressPercentage, bytesSent, totalBytesToSend, curFileName);
                asyncOp.Post(reportProgress, progressChanged);

                response = (HttpWebResponse)request.GetResponse();
                responseReader = new StreamReader(response.GetResponseStream());
                operationCompleted = new SendDataCompletedEventArgs(null, false, responseReader.ReadToEnd());
            }
        }
        catch (WebException wex)
        {
            if (wex.Status == WebExceptionStatus.RequestCanceled)
            {
                operationCompleted = new SendDataCompletedEventArgs(wex, true, null);
            }
            else
            {
                operationCompleted = new SendDataCompletedEventArgs(wex, false, null);
            }
        }
        catch (Exception ex)
        {
            operationCompleted = new SendDataCompletedEventArgs(ex, false, null);
        }
        finally
        {
            //Clean up resources
            if (response != null)
            {
                response.Close();
            }

            if (responseReader != null)
            {
                responseReader.Close();
            }

            if (localFileStream != null)
            {
                localFileStream.Close();
            }
        }

        asyncOp.PostOperationCompleted(_onSendDataCompletedDelegate, operationCompleted);
    }

    public void CancelAsync()
    {
        _operationCanceled = true;
    }

    #endregion

    #region "Helper Methods"

    //This method is guaranteed to be called on the correct thread
    private void reportProgress(object state)
    {
        SendDataProgressChangedEventArgs e = (SendDataProgressChangedEventArgs)state;
        OnSendDataProgressChanged(e);
    }

    private void sendCompleted(object state)
    {
        SendDataCompletedEventArgs e = (SendDataCompletedEventArgs)state;
        OnSendDataCompleted(e);
    }
    #endregion

    #region "Events"

    protected void OnSendDataProgressChanged(SendDataProgressChangedEventArgs e)
    {
        if (SendDataProgressChanged != null)
        {
            SendDataProgressChanged(this, e);
        }
    }

    protected void OnSendDataCompleted(SendDataCompletedEventArgs e)
    {
        if (SendDataCompleted != null)
        {
            SendDataCompleted(this, e);
        }
    }

    #endregion

    #region "Synchronous Operations"

    /// <summary>
    /// Uploads a string data to a specific resource
    /// </summary>
    /// <param name="url">The resource Url</param>
    /// <param name="data">Data to be uploaded to the specified resource url.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public string UploadString(string url, string data)
    {
        return UploadString(url, "POST", data);
    }

    /// <summary>
    /// Uploads a string data to a specific resource
    /// </summary>
    /// <param name="url">The resource Url</param>
    /// <param name="method">The request method. The default method is "POST"</param>
    /// <param name="data">Data to be uploaded to the specified resource url.</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public string UploadString(string url, string method, string data)
    {
        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException("url", "url cannot be null or empty.");
        }

        HttpWebRequest request = null;
        HttpWebResponse response = null;
        StreamReader responseReader = null;

        //Determine the encoding type
        if (_encoding == null)
        {
            _encoding = Encoding.UTF8;
        }

        byte[] dataBytes = _encoding.GetBytes(data);

        try
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = _timeout;
            request.ReadWriteTimeout = _timeout;
            if (_requestHeaders != null)
            {
                request.Headers = _requestHeaders;
            }
            request.KeepAlive = _keepAlive;
            //The default method is POST
            if (string.IsNullOrEmpty(method))
            {
                request.Method = "POST";
            }
            else
            {
                request.Method = method;
            }
            request.ProtocolVersion = _httpProtocolVersion;

            //Begin uploading
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(dataBytes, 0, dataBytes.Length);
            }

            response = (HttpWebResponse)request.GetResponse();
            responseReader = new StreamReader(response.GetResponseStream());
            return responseReader.ReadToEnd();
        }
        finally
        {
            if (response != null)
            {
                response.Close();
            }

            if (responseReader != null)
            {
                responseReader.Close();
            }
        }
    }

    /// <summary>
    /// Sends a request to a specific Url using POST.
    /// </summary>
    /// <param name="url">Location of posting message</param>
    /// <param name="postData">Sets data fields to be posted to the specified Url</param>
    /// <returns>Returns response from the server as a string value</returns>
    /// <remarks>SendData sends data via HTTP protocol using POST method. If you would like to use GET method consider using SendDataUsingGet.</remarks>
    public string SendData(string url, System.Collections.Specialized.NameValueCollection postData)
    {
        return SendData(url, postData, null);
    }

    /// <summary>
    /// Sends a request to a specific Url.
    /// </summary>
    /// <param name="url">Location of posting message</param>
    /// <param name="postData">Sets data fields to be posted to the specified Url</param>
    /// <param name="method">Specifies the http request method.</param>
    /// <returns>Returns response from the server as a string value</returns>
    /// <remarks></remarks>
    public string SendData(string url, HttpMethod method, System.Collections.Specialized.NameValueCollection postData)
    {
        if (method == HttpMethod.POST)
        {
            return sendDataUsingPost(url, postData, null);
        }
        else
        {
            return sendDataUsingGet(url, postData);
        }
    }

    /// <summary>
    /// Sends a request to a specific url using POST method.
    /// </summary>
    /// <param name="url">Location of posting message</param>
    /// <param name="postData">Sets data fields to be posted to the specified Url</param>
    /// <param name="postFiles">Sets files to be posted to the specified Url</param>
    /// <returns></returns>
    /// <remarks></remarks>
    public string SendData(string url, System.Collections.Specialized.NameValueCollection postData, System.Collections.Specialized.NameValueCollection postFiles)
    {
        return sendDataUsingPost(url, postData, postFiles);
    }

    /// <summary>
    /// Sends a request to a specified Url
    /// </summary>
    /// <param name="url">Location of posting message</param>
    /// <param name="postData">Sets data fields to be posted to the specified Url</param>
    /// <param name="postFiles">Sets files to be posted to the specified Url</param>
    /// <returns>Returns response from the server as a string value.</returns>
    /// <remarks>SendData sends data via HTTP protocol using POST method. If you would like to use GET method consider using SendDataUsingGet.</remarks>
    private string sendDataUsingPost(string url, System.Collections.Specialized.NameValueCollection postData, System.Collections.Specialized.NameValueCollection postFiles)
    {

        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException("url", "url cannot be null or empty.");
        }

        HttpWebRequest request = null;
        HttpWebResponse response = null;
        StreamReader responseReader = null;
        FileStream picStream = null;

        string boundaryHeader = "f93dcbA3";
        string boundary = "--" + boundaryHeader;
        string footer = System.Environment.NewLine + boundary + "--" + System.Environment.NewLine;

        //Build Header
        byte[] headerBytes = null;
        FileInfo picFile = null;
        byte[] footerBytes = _encoding.GetBytes(footer);

        try
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            //Set method
            request.Method = "POST";

            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundaryHeader);
            if (_requestHeaders != null)
            {
                request.Headers = _requestHeaders;
            }
            request.ContentLength = calculateTotalSize(boundary, footer, postData, postFiles);
            request.ReadWriteTimeout = _timeout;
            request.KeepAlive = _keepAlive;
            request.ProtocolVersion = _httpProtocolVersion;
            request.Timeout = _timeout;
            Stream requestStream = request.GetRequestStream();

            if (postFiles != null)
            {
                foreach (string key in postFiles.AllKeys)
                {
                    //Write header
                    headerBytes = _encoding.GetBytes(buildFileHeader(boundary, key, postFiles.Get(key)));
                    requestStream.Write(headerBytes, 0, headerBytes.Length);

                    picFile = new FileInfo(postFiles[key]);

                    //Write file content
                    picStream = picFile.Open(FileMode.Open, FileAccess.Read);
                    int bufferSize = 2047;
                    byte[] buffer = new byte[bufferSize + 1];
                    int bytesRead = 0;

                    do
                    {
                        bytesRead = picStream.Read(buffer, 0, bufferSize);
                        requestStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);

                    picStream.Close();
                }
            }

            if (postData != null)
            {
                foreach (string key in postData.AllKeys)
                {
                    //Write header
                    headerBytes = _encoding.GetBytes(buildDataHeader(boundary, key, postData.Get(key)));
                    requestStream.Write(headerBytes, 0, headerBytes.Length);
                }
            }

            //Write footer
            requestStream.Write(footerBytes, 0, footerBytes.Length);
            requestStream.Close();

            response = (HttpWebResponse)request.GetResponse();
            responseReader = new StreamReader(response.GetResponseStream());
            return responseReader.ReadToEnd();
        }
        finally
        {
            if (response != null)
            {
                response.Close();
            }

            if (responseReader != null)
            {
                responseReader.Close();
            }

            if (picStream != null)
            {
                picStream.Close();
            }
        }
    }

    /// <summary>
    /// Sends a request to a specified Url
    /// </summary>
    /// <param name="url">Location of posting message</param>
    /// <param name="postData">Sets data fields to be posted to the specified Url</param>
    /// <returns>Returns response from the server as a string value.</returns>
    /// <remarks>SendData sends data via HTTP protocol using GET method. If you would like to use POST method consider using SendData.</remarks>
    private string sendDataUsingGet(string url, System.Collections.Specialized.NameValueCollection postData)
    {
        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException("url", "url cannot be null or empty.");
        }

        HttpWebRequest request = null;
        HttpWebResponse response = null;
        StreamReader responseReader = null;

        try
        {
            //Build the new url
            string newUrl = string.Empty;
            if (!url.Contains("?"))
            {
                newUrl = url + "?";
            }

            foreach (string key in postData)
            {
                newUrl += string.Format("{0}={1}&", key, postData[key]);
            }

            //Remove the last &
            newUrl = newUrl.Remove(newUrl.Length - 1, 1);

            request = (HttpWebRequest)WebRequest.Create(newUrl);
            //Set method
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded"
            request.ContentType = "application/xml";
            if (_requestHeaders != null)
            {
                request.Headers = _requestHeaders;
            }
            request.KeepAlive = _keepAlive;
            request.ProtocolVersion = _httpProtocolVersion;
            request.Timeout = _timeout;
            response = (HttpWebResponse)request.GetResponse();
            responseReader = new StreamReader(response.GetResponseStream());
            return responseReader.ReadToEnd();
        }
        finally
        {
            //Clean up resources
            if (response != null)
            {
                response.Close();
            }

            if (responseReader != null)
            {
                responseReader.Close();
            }
        }
    }

    #endregion

    #region "Logic"

    public void EnableHttpsRequest()
    {
        _keepAlive = false;
        _httpProtocolVersion = HttpVersion.Version10;
    }

    /// <summary>
    /// Verifies data and calculates total size of the data which you can assign the returned value into HttpRequest.ContentLength
    /// </summary>
    /// <param name="boundary"></param>
    /// <param name="postData"></param>
    /// <param name="postFiles"></param>
    /// <returns>Returns the total length of the data to be uploaded to the specified url.</returns>
    /// <remarks></remarks>
    private long calculateTotalSize(string boundary, string footer, System.Collections.Specialized.NameValueCollection postData, System.Collections.Specialized.NameValueCollection postFiles)
    {
        long totalSize = 0;
        FileInfo localFile = null;
        string header = null;
        byte[] footerBytes = null;

        //Calculate length of files
        if (postFiles != null)
        {
            foreach (string key in postFiles.AllKeys)
            {
                //Calculate file header
                header = buildFileHeader(boundary, key, postFiles[key]);
                totalSize += _encoding.GetBytes(header).Length;
                //Calculate physical file length
                localFile = new FileInfo(postFiles[key]);
                totalSize += localFile.Length;
            }
        }

        //Calculate length of data
        if (postData != null)
        {
            foreach (string key in postData.AllKeys)
            {
                //Calculate data header
                header = buildDataHeader(boundary, key, postData[key]);
                totalSize += _encoding.GetBytes(header).Length;
            }
        }

        footerBytes = _encoding.GetBytes(footer);
        totalSize += footerBytes.Length;

        return totalSize;
    }

    /// <summary>
    /// Builds http request file header.
    /// </summary>
    /// <param name="boundary">The fixed boundary used in the current http request.</param>
    /// <param name="fieldName">Field Name.</param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    private string buildFileHeader(string boundary, string fieldName, string filePath)
    {
        StringBuilder header = new StringBuilder();
        string fileName = Path.GetFileName(filePath);
        header.AppendLine();
        header.AppendLine(boundary);
        header.AppendFormat("Content-Disposition: form-data; name=\"{0}\";", fieldName);
        header.AppendFormat(" filename=\"{0}\"", fileName);
        header.AppendLine();
        header.AppendFormat("Content-Type: {0}", getContentType(fileName));
        header.AppendLine();
        header.AppendLine();
        return header.ToString();
    }

    private string buildDataHeader(string boundary, string fieldName, string value)
    {
        System.Text.StringBuilder header = new System.Text.StringBuilder();
        header.AppendLine();
        header.AppendLine(boundary);
        header.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", fieldName);
        header.AppendLine();
        header.AppendLine();
        header.Append(value);

        return header.ToString();
    }

    private string getContentType(string fileName)
    {
        string fileExt = Path.GetExtension(fileName).Replace(".", "").ToLower();
        string contentType = string.Empty;
        switch (fileExt)
        {
            case "html":
            case "htm":
                //HTML text data
                contentType = "text/html";
                break;
            case "txt":
                //Plain text
                contentType = "text/plain";
                break;
            case "css":
                //Cascading Sytlesheets
                contentType = "text/css";
                break;
            case "gif":
                contentType = "image/gif";
                break;
            case "png":
                contentType = "image/png";
                break;
            case "jpeg":
            case "jpg":
            case "jpe":
                contentType = "image/jpeg";
                break;
            case "tiff":
            case "tif":
                contentType = "image/tiff";
                break;
            case "bmp":
                contentType = "image/x-ms-bmp";
                break;
            case "wav":
                contentType = "audio/x-wav";
                break;
            case "mpeg":
            case "mpg":
            case "mpe":
                contentType = "video/mpeg";
                break;
            case "qt":
            case "mov":
                contentType = "video/quicktime";
                break;
            case "avi":
                //Microsoft video
                contentType = "video/x-msvideo";
                break;
            case "rtf":
                //Microsoft Rich Text Format
                contentType = "application/rtf";
                break;
            case "pdf":
                //Adobe Acrobat PDF
                contentType = "application/pdf";
                break;
            case "doc":
            case "docx":
                //Microsoft Word Document
                contentType = "application/msword";
                break;
            case "tar":
                contentType = "application/x-tar";
                break;
            case "zip":
                contentType = "application/zip";
                break;
            case "exe":
            case "bin":
                contentType = "application/octet-stream";
                break;
            default:
                contentType = "application/octet-stream";
                break;
        }

        return contentType;
    }
    #endregion

    #region "Properties"

    public System.Text.Encoding Encoding
    {
        get { return _encoding; }
        set { _encoding = value; }
    }

    /// <summary>
    /// Gets or sets the request timeout.
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public int Timeout
    {
        get { return _timeout; }
        set
        {
            _timeout = value;

            //Check if the UploadManager is busy
            //If it is busy throw an InvalidOperationException
            if (_isBusy)
            {
                throw new InvalidOperationException("Cannot set TimeOut while UploadManager is busy");
            }
        }
    }

    /// <summary>
    /// Gets or sets the request Http protocol version
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public Version HttpProtocolVersion
    {
        get { return _httpProtocolVersion; }
        set
        {
            if (!object.ReferenceEquals(value, HttpVersion.Version10) || !object.ReferenceEquals(value, HttpVersion.Version11))
            {
                throw new ArgumentException("Invalid Http Protocol Version. Use the Version10 or Version11 fields of the System.Net.HttpVersion class.");
            }

            //Check if the UploadManager is busy
            //If it is busy throw an InvalidOperationException
            if (_isBusy)
            {
                throw new InvalidOperationException("Cannot set HttpProtocolVersion while UploadManager is busy");
            }

            _httpProtocolVersion = value;
        }
    }

    /// <summary>
    /// Gets or sets KeepAlive request header
    /// </summary>
    /// <value></value>
    /// <returns></returns>
    /// <remarks></remarks>
    public bool KeepAlive
    {
        get { return _keepAlive; }

        set
        {
            //Check if the UploadManager is busy
            //If it is busy throw an InvalidOperationException
            if (_isBusy)
            {
                throw new InvalidOperationException("Cannot set KeepAlive while UploadManager is busy");
            }

            _keepAlive = value;
        }
    }

    public WebHeaderCollection Headers
    {
        get { return _requestHeaders; }
        set
        {
            //Check if the UploadManager is busy
            //If it is busy throw an InvalidOperationException
            if (_isBusy)
            {
                throw new InvalidOperationException("Cannot set Headers while UploadManager is busy");
            }

            _requestHeaders = value;
        }
    }

    public bool IsBusy
    {
        get { return _isBusy; }
    }
    #endregion

}
