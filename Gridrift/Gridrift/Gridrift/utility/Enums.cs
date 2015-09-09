using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.Utility
{
    public enum Direction { None, North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest };

    public enum TagID { End, Byte, Short, Int, Long, Float, Double, ByteArray, String, List, Compound, IntArray };

    public enum PacketID { empty, connect, keepAlive, requestChunk, sendChunk };

    public enum GameState { menu, playing }

    public static class Enums
    {

    }
}
