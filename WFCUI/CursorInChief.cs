using System;
using System.Windows;
using System.Windows.Input;

namespace WFCUI
{
    class CursorInChief : IDisposable
    {
        public CursorInChief(Cursor cursor)
        {
            Mouse.OverrideCursor = cursor;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = null;
        }
    }
}