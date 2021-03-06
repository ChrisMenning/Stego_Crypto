﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{
    public static class WorkerExtension
    {
        // By Toni Patrina https://social.msdn.microsoft.com/Forums/windowsapps/en-US/a9330b2a-9552-4722-a238-3a6d24f0c3a0/quotawaitquot-for-backgroundworker?forum=wpdevelop
        public static Task<object> RunWorkerTaskAsync(this BackgroundWorker backgroundWorker)
        {
            var tcs = new TaskCompletionSource<object>();

            RunWorkerCompletedEventHandler handler = null;
            handler = (sender, args) =>
            {
                if (args.Cancelled)
                    tcs.TrySetCanceled();
                else if (args.Error != null)
                    tcs.TrySetException(args.Error);
                else
                    tcs.TrySetResult(args.Result);
            };

            backgroundWorker.RunWorkerCompleted += handler;
            try
            {
                backgroundWorker.RunWorkerAsync();
            }
            catch
            {
                backgroundWorker.RunWorkerCompleted -= handler;
                throw;
            }

            return tcs.Task;
        }
    }
}
