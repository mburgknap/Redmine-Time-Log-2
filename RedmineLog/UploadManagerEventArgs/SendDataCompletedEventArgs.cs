using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class SendDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
{


    private string _result;
    public SendDataCompletedEventArgs(Exception error, bool cancelled, string sendDataResult)
        : base(error, cancelled, null)
    {
        _result = sendDataResult;
    }

    public string SendDataResult
    {
        get
        {
            // Raise an exception if the operation failed 
            // or was canceled.
            base.RaiseExceptionIfNecessary();

            // If the operation was successful, return 
            // the property value.
            return _result;
        }
    }

}
