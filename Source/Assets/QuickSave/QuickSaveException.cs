////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace CI.QuickSave
{
    public class QuickSaveException : Exception
    {
        public QuickSaveException()
            : base()
        {
        }

        public QuickSaveException(string message)
            : base(message)
        {
        }

        public QuickSaveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}