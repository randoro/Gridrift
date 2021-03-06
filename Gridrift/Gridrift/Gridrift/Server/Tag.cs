﻿using Gridrift.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift.Server
{

    public class Tag
    {
        TagID tagID;
        String tagIdentifier;
        byte[] payload;
        TagID listType;
        List<Tag> payloadList;

        public Tag(TagID tagID, String tagIdentifier, byte[] payload)
        {
            this.tagID = tagID;
            this.tagIdentifier = tagIdentifier;
            this.payload = payload;


        }

        public Tag(TagID tagID, String tagIdentifier, List<Tag> payloadList, TagID listType)
        {
            this.tagID = tagID;
            this.tagIdentifier = tagIdentifier;
            this.payloadList = payloadList;
            this.listType = listType;


        }

        public byte[] getPayload()
        {
            return payload;
        }

        public TagID getListType()
        {
            return listType;
        }

        public byte getByte()
        {
            return payload[0];
        }

        public short getShort()
        {
            return BitConverter.ToInt16(payload, 0);
        }

        public int getInt()
        {
            return BitConverter.ToInt32(payload, 0);
        }

        public long getLong()
        {
            return BitConverter.ToInt64(payload, 0);
        }
        
        public float getFloat()
        {
            return BitConverter.ToSingle(payload, 0);
        }
        
        public double getDouble()
        {
            return BitConverter.ToDouble(payload, 0);
        }

        public List<Tag> getPayloadList()
        {
            return payloadList;
        }






        //public object getData()
        //{
        //    if (tagID.Equals(TagID.Byte))
        //    {
        //        unchecked
        //        {
        //            byte unsigned = ((byte[])data)[0];
        //            sbyte returnData = (sbyte)unsigned;
        //            return returnData;
        //        }

        //    }
        //    else if (tagID.Equals(TagID.Short))
        //    {
        //        short returnData = BitConverter.ToInt16((byte[])data, 0);
        //        return returnData;
                
        //    }
        //    else if (tagID.Equals(TagID.Int))
        //    {
        //        int returnData = BitConverter.ToInt32((byte[])data, 0);
        //        return data;
                
        //    }
        //    else if (tagID.Equals(TagID.Long))
        //    {
        //        long returnData = BitConverter.ToInt64((byte[])data, 0);
        //        return returnData;
        //    }
        //    else if (tagID.Equals(TagID.Float))
        //    {
        //        float returnData = BitConverter.ToSingle((byte[])data, 0);
        //        return returnData;
        //    }
        //    else if (tagID.Equals(TagID.Double))
        //    {
        //        double returnData = BitConverter.ToDouble((byte[])data, 0);
        //        return returnData;
        //    }
        //    else if (tagID.Equals(TagID.ByteArray))
        //    {
        //        byte[] arraySizeInt = new byte[4];
        //        Array.Copy((byte[])data, arraySizeInt, 4);
        //        int arraySizeNumber = BitConverter.ToInt32(arraySizeInt, 0);
        //        byte[] returnData = new byte[arraySizeNumber];
        //        Array.Copy((byte[])data, 4, returnData, 0, arraySizeNumber);
        //        return returnData;
        //    }
        //    else if (tagID.Equals(TagID.String))
        //    {
        //        byte[] arraySizeShort = new byte[2];
        //        Array.Copy((byte[])data, arraySizeShort, 2);
        //        short arraySizeNumber = BitConverter.ToInt16(arraySizeShort, 0);
        //        byte[] returnCharData = new byte[arraySizeNumber];
        //        Array.Copy((byte[])data, 2, returnCharData, 0, arraySizeNumber);
        //        string returnData = Encoding.UTF8.GetString(returnCharData);
        //        return returnData; //ej testad
        //    }
        //    else if (tagID.Equals(TagID.List))
        //    {
        //        //returns a list of bytearrays 
        //            return data;

        //    }
        //    else if (tagID.Equals(TagID.Compound))
        //    {
        //        //no data (compound tag)
        //        return null;
        //    }
        //    else if (tagID.Equals(TagID.IntArray))
        //    {
        //        byte[] arraySizeInt = new byte[4];
        //        Array.Copy((byte[])data, arraySizeInt, 4);
        //        int arraySizeNumber = BitConverter.ToInt32(arraySizeInt, 0);
        //        int[] returnData = new int[arraySizeNumber];
        //        Array.Copy((byte[])data, 4, returnData, 0, arraySizeNumber * 4);
        //        return returnData; //ej testad
        //    }
        //    else
        //    {
        //        //no data (end tag)
        //        return null;
        //    }
        //}

        public TagID getID()
        {
            return tagID;
        }

        public String getName()
        {
            return tagIdentifier;
        }

        //unchecked
        //        {
        //            sbyte derpa =  -127;
        //            byte b = (byte)derpa;
        //        }
        //till sen när vi måste få ut signed byte

    }
}
