using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class SendDataProgressChangedEventArgs : System.ComponentModel.ProgressChangedEventArgs
{

    #region "Fields"

    private long _bytesSent;
    private string _fileName;

    private long _totalBytesToSend;
    #endregion
    public SendDataProgressChangedEventArgs(int progressPercentage, long bytesSent, long totalBytesToSend, string fileName)
        : base(progressPercentage, null)
    {
        _bytesSent = bytesSent;
        _totalBytesToSend = totalBytesToSend;
        _fileName = fileName;
    }

    public long BytesSent
    {
        get { return _bytesSent; }
    }

    public long TotalBytesToSend
    {
        get { return _totalBytesToSend; }
    }

    public string FileName
    {
        get { return _fileName; }
    }
}
