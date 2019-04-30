using System;
using System.Collections.Generic;
using System.Text;

namespace DTL.Shared.Models
{
    public class DTLException : Exception
    {
        public DTLException()
        {
            ErrorList = new List<string>();
        }

        public DTLException(List<string> list)
        {
            ErrorList = list ?? new List<string>();
        }

        private List<string> ErrorList { get; }

        public string ErrMsg
        {
            get
            {
                if (ErrorList == null || ErrorList.Count == 0) return "";
                var i = 1;
                var str = "";
                ErrorList.ForEach(s => { str += i++ + ". " + s + "\n"; });
                return str;
            }
        }
    }
}
