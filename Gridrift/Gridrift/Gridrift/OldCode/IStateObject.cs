﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Gridrift.OldCode
{
    public interface IStateObject
    {
        int BufferSize { get; }

        int Id { get; }

        bool Close { get; set; }

        byte[] Buffer { get; }

        Socket Listener { get; }

        string Text { get; }

        void Append(string text);

        void Reset();
    }
}
