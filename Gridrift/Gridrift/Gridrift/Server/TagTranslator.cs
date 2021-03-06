﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Gridrift.Utility;

namespace Gridrift.Server
{
    static class TagTranslator
    {
        public static Chunk getChunk(byte[] chunkData)
        {
            int position = 0;
            int length = chunkData.Length;
            int layerDepth = 0;
            Chunk newChunk = new Chunk();

            while (position < length)
            {
                Tag tempTag = readTag(chunkData, ref position);
                configureChunk(newChunk, tempTag, ref layerDepth);
            }
            return newChunk;

        }
        public static void configureChunk(Chunk chunk, Tag tag, ref int layerDepth)
        {
            TagID tagID = tag.getID();
            String tagName = tag.getName();

            switch (tagID)
                    {
                        case TagID.End:
                            layerDepth--;
                            break;
                        case TagID.Byte:
                            if (tagName.Equals("TerrainPopulated"))
                            {
                                chunk.terrainPopulated = tag.getByte();
                            }
                            else if (tagName.Equals("StructurePopulated"))
                            {
                                chunk.structurePopulated = tag.getByte();
                            }
                            break;
                        case TagID.Short:
                            break;
                        case TagID.Int:
                            if (tagName.Equals("XPos"))
                            {
                                chunk.xCoordinate = tag.getInt();
                            }
                            else if (tagName.Equals("YPos"))
                            {
                                chunk.yCoordinate = tag.getInt();
                            }
                            break;
                        case TagID.Long:
                            if (tagName.Equals("LastUpdate"))
                            {
                                chunk.lastUpdate = tag.getLong();
                            }
                            else if (tagName.Equals("InhabitedTime"))
                            {
                                chunk.inhabitedTime = tag.getLong();
                            }
                            break;
                        case TagID.Float:
                            break;
                        case TagID.Double:
                            break;
                        case TagID.ByteArray:
                            if (tagName.Equals("Biomes"))
                            {
                                chunk.biomes = tag.getPayload();
                            }
                            else if (tagName.Equals("Blocks"))
                            {
                                chunk.blocks = tag.getPayload();
                            }
                            else if (tagName.Equals("Objects"))
                            {
                                chunk.objects = tag.getPayload();
                            }
                            break;
                        case TagID.String:
                            break;
                        case TagID.List:
                            if (tagName.Equals("Entities"))
                            {
                                //chunk.entities = (List<List<Tag>>)currentTag.getData();
                            }
                            else if (tagName.Equals("Rooms"))
                            {
                                //newChunk.rooms = (List<List<Tag>>)currentTag.getData();
                            }
                            break;
                        case TagID.Compound:
                            layerDepth++;
                            break;
                        case TagID.IntArray:
                            break;
                        default:
                            layerDepth = 0;
                            break;
                    }
        }
        /*
        public static Chunk getUnloadedChunk(byte[] chunkData)
        {

            int chunkXPos = 0;
            int chunkYPos = 0;
            bool chunkIdentified = false;
            bool chunkChecked = false;
            bool chunkAlreadyLoaded = false;
            int layerDepth = 1;
            Tag regionTag = readTag(regionFile.fileStream); //should be compound


            while (layerDepth > 0)
            {
                Tag currentTag = readTag(regionFile.fileStream);
                String tagName = currentTag.getName();
                TagID tagID = currentTag.getID();


                if (chunkIdentified & !chunkChecked)
                {

                    int localxPos;
                    int localyPos;
                    if (chunkXPos >= 0)
                    {
                        localxPos = chunkXPos % 4;
                    }
                    else
                    {
                        localxPos = 3 + (chunkXPos + 1) % 4;
                    }
                    if (chunkYPos >= 0)
                    {
                        localyPos = chunkYPos % 4;
                    }
                    else
                    {
                        localyPos = 3 + (chunkYPos + 1) % 4;
                    }

                    bool isThisChunkLoaded = regionFile.chunksLoaded[localxPos + localyPos * 4];
                    // nummer = xpos % 4 + (ypos % 4) * 4

                    if (isThisChunkLoaded)
                    {
                        chunkAlreadyLoaded = true;
                    }
                    else
                    {
                        //chunk is not loaded and will be now
                        newChunk.XPos = chunkXPos;
                        newChunk.YPos = chunkYPos;
                        newChunk.innerIndex = (byte)(localxPos + localyPos * 4);
                        regionFile.chunksLoaded[localxPos + localyPos * 4] = true;
                    }

                    chunkChecked = true;

                }

                if (!chunkAlreadyLoaded)
                {

                    switch (tagID)
                    {
                        case TagID.End:
                            layerDepth--;
                            if (layerDepth == 1)
                            {
                                //end of unloaded chunk
                                //this means its now added return new chunk
                                Console.WriteLine("chunk now loaded");
                                return newChunk;
                            }
                            break;
                        case TagID.Byte:
                            if (tagName.Equals("TerrainPopulated"))
                            {
                                newChunk.terrainPopulated = (sbyte)currentTag.getData();
                            }
                            else if (tagName.Equals("StructurePopulated"))
                            {
                                newChunk.structurePopulated = (sbyte)currentTag.getData();
                            }
                            break;
                        case TagID.Short:
                            break;
                        case TagID.Int:
                            if (tagName.Equals("XPos"))
                            {
                                byte[] data = (byte[])currentTag.getRawData();
                                int dataInt = BitConverter.ToInt32(data, 0);
                                chunkXPos = dataInt;
                            }
                            else if (tagName.Equals("YPos"))
                            {
                                byte[] data = (byte[])currentTag.getRawData();
                                int dataInt = BitConverter.ToInt32(data, 0);
                                chunkYPos = dataInt;
                                chunkIdentified = true;
                            }
                            break;
                        case TagID.Long:
                            if (tagName.Equals("LastUpdate"))
                            {
                                byte[] data = (byte[])currentTag.getRawData();
                                long dataLong = BitConverter.ToInt64(data, 0);
                                newChunk.lastUpdate = dataLong;
                            }
                            else if (tagName.Equals("InhabitedTime"))
                            {
                                byte[] data = (byte[])currentTag.getRawData();
                                long dataLong = BitConverter.ToInt64(data, 0);
                                newChunk.inhabitedTime = dataLong;
                            }
                            break;
                        case TagID.Float:
                            break;
                        case TagID.Double:
                            break;
                        case TagID.ByteArray:
                            if (tagName.Equals("Biomes"))
                            {
                                newChunk.biomes = (byte[])currentTag.getData();
                            }
                            else if (tagName.Equals("Blocks"))
                            {
                                newChunk.blocks = (byte[])currentTag.getData();
                            }
                            else if (tagName.Equals("Objects"))
                            {
                                newChunk.objects = (byte[])currentTag.getData();
                            }
                            break;
                        case TagID.String:
                            break;
                        case TagID.List:
                            if (tagName.Equals("Entities"))
                            {
                                newChunk.entities = (List<List<Tag>>)currentTag.getData();
                            }
                            else if (tagName.Equals("Rooms"))
                            {
                                newChunk.rooms = (List<List<Tag>>)currentTag.getData();
                            }
                            break;
                        case TagID.Compound:
                            layerDepth++;
                            break;
                        case TagID.IntArray:
                            break;
                        default:
                            layerDepth = 0;
                            break;
                    }
                }
                else
                {
                    switch (tagID)
                    {
                        case TagID.End:
                            layerDepth--;
                            if (layerDepth == 1)
                            {
                                //end of already loaded chunk
                                chunkAlreadyLoaded = false;
                                chunkIdentified = false;
                                chunkChecked = false;
                            }
                            break;
                        case TagID.Compound:
                            layerDepth++;
                            break;
                        default:
                            break;
                    }
                }
                //}
            }
            return null;
        }
        */

        public static byte[] saveChunk(Chunk chunk)
        {
            int position = 0;
            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            List<Tag> chunkTagList = new List<Tag>();
            
            byte[] chunkXpos = BitConverter.GetBytes(chunk.xCoordinate);
            Tag XPos = new Tag(TagID.Int, "XPos", chunkXpos);
            chunkTagList.Add(XPos);

            byte[] chunkYpos = BitConverter.GetBytes(chunk.yCoordinate);
            Tag YPos = new Tag(TagID.Int, "YPos", chunkYpos);
            chunkTagList.Add(YPos);

            byte[] chunkLastUpdate = BitConverter.GetBytes(chunk.lastUpdate);
            Tag LastUpdate = new Tag(TagID.Long, "LastUpdate", chunkLastUpdate);
            chunkTagList.Add(LastUpdate);

            byte terrainByte = (byte)chunk.terrainPopulated;
            byte[] chunkTerrainPopulated = { terrainByte };
            Tag TerrainPopulated = new Tag(TagID.Byte, "TerrainPopulated", chunkTerrainPopulated);
            chunkTagList.Add(TerrainPopulated);

            byte structureByte = chunk.structurePopulated;
            byte[] chunkStructurePopulated = { structureByte };
            Tag StructurePopulated = new Tag(TagID.Byte, "StructurePopulated", chunkStructurePopulated);
            chunkTagList.Add(StructurePopulated);

            byte[] chunkInhabitedTime = BitConverter.GetBytes(chunk.inhabitedTime);
            Tag InhabitedTime = new Tag(TagID.Long, "InhabitedTime", chunkInhabitedTime);
            chunkTagList.Add(InhabitedTime);

            Tag Biomes = new Tag(TagID.ByteArray, "Biomes", chunk.biomes);
            chunkTagList.Add(Biomes);

            Tag Blocks = new Tag(TagID.ByteArray, "Blocks", chunk.blocks);
            chunkTagList.Add(Blocks);

            Tag Objects = new Tag(TagID.ByteArray, "Objects", chunk.objects);
            chunkTagList.Add(Objects);

            //Tag Entities = new Tag(TagID.List, "Entities", chunk.entities, TagID.Compound);
            //writeTag(Entities, fileStream);

            //Tag Rooms = new Tag(TagID.List, "Rooms", chunk.rooms, TagID.Compound);
            //writeTag(Rooms, fileStream);

            //Tag TileEntities = new Tag(TagID.List, "TileEntities", chunk.tileEntities, TagID.Compound);
            //writeTag(TileEntities, fileStream);

            //Tag End = new Tag(TagID.End, null, null);

            Tag chunkTag = new Tag(TagID.Compound, "chunk", chunkTagList, TagID.Compound);

            writeTag(chunkTag, ref buffer, ref position);
            writeTag(XPos, ref buffer, ref position);
            writeTag(YPos, ref buffer, ref position);
            writeTag(LastUpdate, ref buffer, ref position);
            writeTag(TerrainPopulated, ref buffer, ref position);
            writeTag(StructurePopulated, ref buffer, ref position);
            writeTag(InhabitedTime, ref buffer, ref position);
            writeTag(Biomes, ref buffer, ref position);
            writeTag(Blocks, ref buffer, ref position);
            writeTag(Objects, ref buffer, ref position);
            //writeTag(End, ref buffer, ref position);

            byte[] trimmedBuffer = new byte[position];
            Array.Copy(buffer, 0, trimmedBuffer, 0, position);

            return trimmedBuffer;
        }

        public static Tag readTag(byte[] chunkData, ref int position)
        {
            TagID tagID = (TagID)readPayloadBasicType(chunkData, ref position, sizeof(byte), "firsttag")[0];
            Tag returnTag;

            if (tagID.Equals(TagID.End))
            {

                returnTag = new Tag(TagID.End, null, null);
                return returnTag; //No payload
            }

            //get length of string
            byte[] byte2 = readPayloadBasicType(chunkData, ref position, sizeof(short),"secondtag");
            short stringLength = BitConverter.ToInt16(byte2, 0);

            //get string
            byte[] byteString = readPayloadBasicType(chunkData, ref position, sizeof(byte) * stringLength, "thirdtag");
            String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);

            byte[] payload;

            //Payload
            switch (tagID)
            {
                case TagID.End:
                    returnTag = new Tag(tagID, null, null);
                    return returnTag; //No payload
                    break;
                case TagID.Byte:

                    payload = readPayloadBasicType(chunkData, ref position, sizeof(byte), tagIdentifier);
                    returnTag = new Tag(tagID, tagIdentifier, payload);
                    return returnTag;

                    break;
                case TagID.Short:

                    payload = readPayloadBasicType(chunkData, ref position, sizeof(short), tagIdentifier);
                    returnTag = new Tag(tagID, tagIdentifier, payload);
                    return returnTag;

                    break;
                case TagID.Int:

                    payload = readPayloadBasicType(chunkData, ref position, sizeof(int), tagIdentifier);
                    returnTag = new Tag(tagID, tagIdentifier, payload);
                    return returnTag;

                    break;
                case TagID.Long:

                    payload = readPayloadBasicType(chunkData, ref position, sizeof(long), tagIdentifier);
                    returnTag = new Tag(tagID, tagIdentifier, payload);
                    return returnTag;

                    break;
                case TagID.Float:

                    payload = readPayloadBasicType(chunkData, ref position, sizeof(float), tagIdentifier);
                    returnTag = new Tag(tagID, tagIdentifier, payload);
                    return returnTag;

                    break;
                case TagID.Double:

                    payload = readPayloadBasicType(chunkData, ref position, sizeof(double), tagIdentifier);
                    returnTag = new Tag(tagID, tagIdentifier, payload);
                    return returnTag;

                    break;
                case TagID.ByteArray:

                    byte[] sizeArray = readPayloadBasicType(chunkData, ref position, sizeof(int), tagIdentifier);
                    int arraySizeNumber = BitConverter.ToInt32(sizeArray, 0);

                    payload = readPayloadBasicType(chunkData, ref position, sizeof(byte) * arraySizeNumber, tagIdentifier);

                    returnTag = new Tag(tagID, tagIdentifier, payload);
                    return returnTag;

                    break;
                case TagID.String:

                    byte[] sizeArray2 = readPayloadBasicType(chunkData, ref position, sizeof(short), tagIdentifier);
                    short stringSizeNumber = BitConverter.ToInt16(sizeArray2, 0);

                    payload = new byte[sizeof(short) + stringSizeNumber]; //get payload of string

                    //copy length header
                    Array.Copy(sizeArray2, 0, payload, 0, sizeof(short));

                    byte[] payloadNoLength2 = readPayloadBasicType(chunkData, ref position, sizeof(char) * stringSizeNumber, tagIdentifier);
                    Array.Copy(payloadNoLength2, 0, payload, sizeof(short), sizeof(char) * stringSizeNumber);

                    returnTag = new Tag(tagID, tagIdentifier, payload);
                    return returnTag;

                    break;
                case TagID.List:

                    //get element type
                    TagID elementType = (TagID)readPayloadBasicType(chunkData, ref position, sizeof(byte), tagIdentifier)[0];

                    //get list length
                    byte[] elementLengthArray = readPayloadBasicType(chunkData, ref position, sizeof(int), tagIdentifier);

                    int elementsInList = BitConverter.ToInt32(elementLengthArray, 0); //number of elements
                    //List<List<Tag>> tagListList = new List<List<Tag>>();
                    List<Tag> returnList = new List<Tag>(elementsInList);
                    for (int i = 0; i < elementsInList; i++)
                    {
                        Tag newTag = readTag(chunkData, ref position);
                        if(newTag.getID().Equals(elementType)) 
                        {
                            returnList.Add(readTag(chunkData, ref position));
                        }
                        else 
                        {
                            throw new Exception("Wrong type of element in List");
                        }
                    }
                    returnTag = new Tag(tagID, tagIdentifier, returnList, elementType);
                    return returnTag;
                    break;

#region oldthings
                    //                switch (elementType)
                    //                {
                    //                case TagID.End:
                    //                        //should not happen
                    //                        break;
                    //                case TagID.Byte:
                    //                        List<Tag> returnList = new List<Tag>(elementsInList);
                    //                        for (int i = 0; i < elementsInList; i++)
                    //                        {
                    //                            returnList.Add(readTag(chunkdata, ref position));
                    //                        }
                    //                        returnTag = new Tag(tagID, tagIdentifier, returnList, elementType);
                    //                        return returnTag;
                    //                        //payload = new byte[sizeof(byte) + sizeof(int) + elementsInList * sizeof(byte)];
                    //                        ////payload[0] = (byte)elementType;
                    //                        //Array.Copy(elementArray, 0, payload, sizeof(byte), sizeof(int));

                    //                        byte[] payloadNoLengthByte = readPayloadBasicType(chunkdata, ref position, sizeof(byte) * elementsInList);
                    //                        Array.Copy(payloadNoLengthByte, 0, payload, sizeof(byte) + sizeof(int), sizeof(byte) * elementsInList);

                    //                        returnTag = new Tag(tagID, tagIdentifier, payload);
                    //                        return returnTag;
                    //                        break;
                    //                case TagID.Short:
                    //                        payload = new byte[sizeof(byte) + sizeof(int) + elementsInList * sizeof(short)];
                    //                        payload[0] = (byte)elementType;
                    //                        Array.Copy(elementArray, 0, payload, sizeof(byte), sizeof(int));

                    //                        byte[] payloadNoLengthShort = readPayloadBasicType(chunkdata, ref position, sizeof(short) * elementsInList);
                    //                        Array.Copy(payloadNoLengthShort, 0, payload, sizeof(byte) + sizeof(int), sizeof(short) * elementsInList);

                    //                        break;
                    //                case TagID.Int:
                    //                        payload = new byte[sizeof(byte) + sizeof(int) + elementsInList * sizeof(int)];
                    //                        payload[0] = (byte)elementType;
                    //                        Array.Copy(elementArray, 0, payload, sizeof(byte), sizeof(int));

                    //                        byte[] payloadNoLengthInt = readPayloadBasicType(chunkdata, ref position, sizeof(int) * elementsInList);
                    //                        Array.Copy(payloadNoLengthInt, 0, payload, sizeof(byte) + sizeof(int), sizeof(int) * elementsInList);
                    //                        break;
                    //                case TagID.Long:
                    //                        payload = new byte[sizeof(byte) + sizeof(int) + elementsInList * sizeof(long)];
                    //                        payload[0] = (byte)elementType;
                    //                        Array.Copy(elementArray, 0, payload, sizeof(byte), sizeof(int));

                    //                        byte[] payloadNoLengthLong = readPayloadBasicType(chunkdata, ref position, sizeof(long) * elementsInList);
                    //                        Array.Copy(payloadNoLengthLong, 0, payload, sizeof(byte) + sizeof(int), sizeof(long) * elementsInList);
                    //                        break;
                    //                case TagID.Float:
                    //                        payload = new byte[sizeof(byte) + sizeof(int) + elementsInList * sizeof(float)];
                    //                        payload[0] = (byte)elementType;
                    //                        Array.Copy(elementArray, 0, payload, sizeof(byte), sizeof(int));

                    //                        byte[] payloadNoLengthFloat = readPayloadBasicType(chunkdata, ref position, sizeof(float) * elementsInList);
                    //                        Array.Copy(payloadNoLengthFloat, 0, payload, sizeof(byte) + sizeof(int), sizeof(float) * elementsInList);
                    //                        break;
                    //                case TagID.Double:
                    //                        payload = new byte[sizeof(byte) + sizeof(int) + elementsInList * sizeof(double)];
                    //                        payload[0] = (byte)elementType;
                    //                        Array.Copy(elementArray, 0, payload, sizeof(byte), sizeof(int));

                    //                        byte[] payloadNoLengthDouble = readPayloadBasicType(chunkdata, ref position, sizeof(double) * elementsInList);
                    //                        Array.Copy(payloadNoLengthDouble, 0, payload, sizeof(byte) + sizeof(int), sizeof(double) * elementsInList);
                    //                        break;
                    //                case TagID.ByteArray:
                    //                        //int payloadLength = 0;
                    //                        List<Tag> tagList = new List<Tag>();

                    //                        for (int i = 0; i < elementsInList; i++)
                    //                        {
                    //                            Tag byteArrayTag = TagTranslator.readTag(chunkdata, ref position);
                    //                            if (byteArrayTag.getID().Equals(TagID.ByteArray))
                    //                            {
                    //                                tagList.Add(byteArrayTag);
                    //                            }
                    //                            else
                    //                            {
                    //                                throw new Exception("Wrong type of element in List");
                    //                            }
                    //                        }

                    //                        break;
                    //                case TagID.String:
                    //                        break;
                    //                case TagID.List:
                    //                        break;
                    //                case TagID.Compound:
                    //                        break;
                    //                case TagID.IntArray:
                    //                        break;
                    //                default:
                    //                        break;
                    //}

                    //                if (elementsInList > 0)
                    //                {
                    //if (!listTagID.Equals(TagID.Compound))
                    //{
                    //    byte payloadElementSize = Globals.dataTypeSizes[listTagID];

                    //    List<byte[]> byteArrayList = new List<byte[]>();

                    //    for (int i = 0; i < elementsInList; i++)
                    //    {
                    //        byte[] element = new byte[payloadElementSize];
                    //        fileStream.Read(element, 0, payloadElementSize);
                    //        byteArrayList.Add(element);
                    //    }

                    //    returnTag = new Tag(tagID, tagIdentifier, byteArrayList, (TagID)listTagID);
                    //    return returnTag;
                    //}
                    //else
                    //{


                    //for (int i = 0; i < elementsInList; i++)
                    //{
                    //    List<Tag> tagList = new List<Tag>();
                    //    bool stillInList = true;
                    //    while (stillInList)
                    //    {
                    //        Tag aTag = TagTranslator.readTag(fileStream);
                    //        tagList.Add(aTag);
                    //        if (aTag.getID().Equals(TagID.End))
                    //        {
                    //            stillInList = false;
                    //        }
                    //    }
                    //    tagListList.Add(tagList);


                    //}

                    //returnTag = new Tag(tagID, tagIdentifier, tagListList, TagID.Compound);
                    //return returnTag;


                    //}
                    //}
                    //returnTag = new Tag(tagID, tagIdentifier, tagListList, TagID.Compound); //change
                    //return returnTag;
#endregion
                    break;
                case TagID.Compound:

                    
                    //List<List<Tag>> tagListList = new List<List<Tag>>();
                    List<Tag> returnCompoundList = new List<Tag>();

                    while(true) 
                    {
                        Tag newTag = readTag(chunkData, ref position);
                        if (newTag.getID().Equals(TagID.End))
                        {
                            break;
                        }
                        returnCompoundList.Add(newTag);
                    }

                    returnTag = new Tag(tagID, tagIdentifier, returnCompoundList, TagID.Compound);
                    return returnTag;
                    break;
                case TagID.IntArray:

                    byte[] sizeArray3 = readPayloadBasicType(chunkData, ref position, sizeof(int), tagIdentifier);
                    int arraySizeNumber3 = BitConverter.ToInt32(sizeArray3, 0);

                    payload = new byte[sizeof(int) + arraySizeNumber3 * sizeof(int)]; //get payload of array

                    //copy length header
                    Array.Copy(sizeArray3, 0, payload, 0, sizeof(int));

                    //copy payload
                    byte[] payloadNoLength3 = readPayloadBasicType(chunkData, ref position, sizeof(int) * arraySizeNumber3, tagIdentifier);
                    Array.Copy(payloadNoLength3, 0, payload, sizeof(int), sizeof(int) * arraySizeNumber3);

                    returnTag = new Tag(tagID, tagIdentifier, payload);
                    return returnTag;

                    break;
                default:
                    returnTag = new Tag(tagID, tagIdentifier, null, tagID);
                    return returnTag; //never happens
                    break;
            }
#region moreOldThings
            //if (tagID.Equals(TagID.Byte))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload



            //    byte[] payload = new byte[Globals.dataTypeSizes[(int)tagID]]; //changed for each tag
            //    payload[0] = (byte)fileStream.ReadByte();



            //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //    return returnTag;
            //}
            //else if (tagID.Equals(TagID.Short))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload


            //    byte[] payload = new byte[Globals.dataTypeSizes[(int)tagID]]; //changed for each tag
            //    fileStream.Read(payload, 0, Globals.dataTypeSizes[(int)tagID]);

            //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //    return returnTag;
            //}
            //else if (tagID.Equals(TagID.Int))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload


            //    byte[] payload = new byte[Globals.dataTypeSizes[(int)tagID]]; //changed for each tag
            //    fileStream.Read(payload, 0, Globals.dataTypeSizes[(int)tagID]);

            //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //    return returnTag;
            //}
            //else if (tagID.Equals(TagID.Long))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload


            //    byte[] payload = new byte[Globals.dataTypeSizes[(int)tagID]]; //changed for each tag
            //    fileStream.Read(payload, 0, Globals.dataTypeSizes[(int)tagID]);

            //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //    return returnTag;
            //}
            //else if (tagID.Equals(TagID.Float))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload


            //    byte[] payload = new byte[Globals.dataTypeSizes[(int)tagID]]; //changed for each tag
            //    fileStream.Read(payload, 0, Globals.dataTypeSizes[(int)tagID]);

            //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //    return returnTag;
            //}
            //else if (tagID.Equals(TagID.Double))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload


            //    byte[] payload = new byte[Globals.dataTypeSizes[(int)tagID]]; //changed for each tag
            //    fileStream.Read(payload, 0, Globals.dataTypeSizes[(int)tagID]);

            //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //    return returnTag;
            //}
            //else if (tagID.Equals(TagID.ByteArray))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload



            //    byte[] sizeArray = new byte[4]; //changed for each tag
            //    fileStream.Read(sizeArray, 0, 4);
            //    int arraySizeNumber = BitConverter.ToInt32(sizeArray, 0);

            //    byte[] payload = new byte[4 + arraySizeNumber]; //changed for each tag
            //    payload[0] = sizeArray[0];
            //    payload[1] = sizeArray[1];
            //    payload[2] = sizeArray[2];
            //    payload[3] = sizeArray[3];
            //    fileStream.Read(payload, 4, arraySizeNumber);


            //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //    return returnTag;
            //}
            //else if (tagID.Equals(TagID.String))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload



            //    byte[] sizeArray = new byte[2]; //changed for each tag
            //    fileStream.Read(sizeArray, 0, 2);
            //    short stringSizeNumber = BitConverter.ToInt16(sizeArray, 0);

            //    byte[] payload = new byte[2 + stringSizeNumber]; //changed for each tag
            //    payload[0] = sizeArray[0];
            //    payload[1] = sizeArray[1];
            //    fileStream.Read(payload, 2, stringSizeNumber);


            //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //    return returnTag;
            //}
            //else if (tagID.Equals(TagID.List)) //måste fixas så att compound tag sparar extra.   typ, om de e compund tag så körs readTag här inne i en whileloop som breakas genom end tag sen läggs datan mellan varje compund och end in i en bytearray som läggs i en lista av bytearrays.
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload



            //    byte listTagID = (byte)fileStream.ReadByte();
            //    byte[] elementArray = new byte[4];
            //    fileStream.Read(elementArray, 0, 4);
            //    int elementsInList = BitConverter.ToInt32(elementArray, 0);

            //    if (elementsInList > 0)
            //    {
            //        if (!listTagID.Equals(TagID.Compound))
            //        {
            //            byte payloadElementSize = Globals.dataTypeSizes[listTagID];

            //            List<byte[]> byteArrayList = new List<byte[]>();

            //            for (int i = 0; i < elementsInList; i++)
            //            {
            //                byte[] element = new byte[payloadElementSize];
            //                fileStream.Read(element, 0, payloadElementSize);
            //                byteArrayList.Add(element);
            //            }

            //            returnTag = new Tag(tagID, tagIdentifier, byteArrayList, (TagID)listTagID);
            //            return returnTag;
            //        }
            //        else
            //        {

            //        }
            //    }

            //    //if (!listTagID.Equals(TagID.Compound))
            //    //{

            //    //    byte[] sizeArray = new byte[4];
            //    //    fileStream.Read(sizeArray, 0, 4);
            //    //    int arraySizeNumber = BitConverter.ToInt32(sizeArray, 0);

            //    //    byte payloadElementSize = Globals.dataTypeSizes[listTagID];

            //    //    byte[] payload = new byte[1 + 4 + arraySizeNumber * payloadElementSize]; //ID + length + payload

            //    //    payload[0] = listTagID;
            //    //    payload[1] = sizeArray[0];
            //    //    payload[2] = sizeArray[1];
            //    //    payload[3] = sizeArray[2];
            //    //    payload[4] = sizeArray[3];
            //    //    fileStream.Read(payload, 5, arraySizeNumber * payloadElementSize);


            //    //    returnTag = new Tag(tagID, tagIdentifier, payload);
            //    //}
            //    //else
            //    //{
            //    returnTag = new Tag(tagID, tagIdentifier, null, tagID); //change
            //    //}
            //    return returnTag;
            //}
            //else if (tagID.Equals(TagID.Compound))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    // no Payload

            //    returnTag = new Tag(tagID, tagIdentifier, null, tagID);
            //    return returnTag;


            //}
            //else if (tagID.Equals(TagID.IntArray))
            //{
            //    byte[] byte2 = new byte[2];
            //    fileStream.Read(byte2, 0, 2);
            //    short stringLength = BitConverter.ToInt16(byte2, 0);
            //    byte[] byteString = new byte[stringLength];
            //    fileStream.Read(byteString, 0, stringLength);
            //    String tagIdentifier = Encoding.UTF8.GetString(byteString, 0, stringLength);
            //    //Payload



            //    byte[] sizeArray = new byte[4]; //changed for each tag
            //    fileStream.Read(sizeArray, 0, 4);
            //    int arraySizeNumber = BitConverter.ToInt32(sizeArray, 0);

            //    byte[] payload = new byte[4 + arraySizeNumber * 4]; //changed for each tag
            //    payload[0] = sizeArray[0];
            //    payload[1] = sizeArray[1];
            //    payload[2] = sizeArray[2];
            //    payload[3] = sizeArray[3];
            //    fileStream.Read(payload, 4, arraySizeNumber * 4);


            //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //    return returnTag;
            //}
            //else
            //{
            //    returnTag = new Tag(tagID, null, null, tagID);
            //    return returnTag;
            //}
#endregion

        }


        private static byte[] readPayloadBasicType(byte[] chunkData, ref int position, int sizeOf, String tagIdentifier)
        {
            byte[] payload = new byte[sizeOf];
            Array.Copy(chunkData, position, payload, 0, sizeOf);
            position += sizeOf;
            return payload;
        }

        private static void writePayloadBasicType(byte[] toBeWritten, ref byte[] chunkData, ref int position, int sizeOf)
        {
            Array.Copy(toBeWritten, 0, chunkData, position, sizeOf);
            position += sizeOf;
        }

        public static void writeTag(Tag tag, ref byte[] chunkData, ref int position)
        {
            TagID tagID = tag.getID();
            byte[] tagIDbytes = new byte[1];
            tagIDbytes[0] = (byte)tagID;
            writePayloadBasicType(tagIDbytes, ref chunkData, ref position, sizeof(byte));

            if(tagID.Equals(TagID.End)) 
            {
                return;
            }

            String tagName = tag.getName();
            short stringLength = (short)tagName.Length;

            byte[] tagNameLengthBytes = BitConverter.GetBytes(stringLength);
            writePayloadBasicType(tagNameLengthBytes, ref chunkData, ref position, sizeof(short));

            byte[] tagNameBytes = Encoding.UTF8.GetBytes(tagName);
            writePayloadBasicType(tagNameBytes, ref chunkData, ref position, tagNameBytes.Length);

            byte[] payload;
                switch (tagID)
                {
                    case TagID.End:
                        //using payload byte array
                        return; //never happens
                        break;
                    case TagID.Byte:
                        //using payload byte array
                        payload = tag.getPayload();
                        writePayloadBasicType(payload, ref chunkData, ref position, sizeof(byte));
                        break;
                    case TagID.Short:
                        //using payload byte array
                        payload = tag.getPayload();
                        writePayloadBasicType(payload, ref chunkData, ref position, sizeof(short));
                        break;
                    case TagID.Int:
                        //using payload byte array
                        payload = tag.getPayload();
                        writePayloadBasicType(payload, ref chunkData, ref position, sizeof(int));
                        break;
                    case TagID.Long:
                        //using payload byte array
                        payload = tag.getPayload();
                        writePayloadBasicType(payload, ref chunkData, ref position, sizeof(long));
                        break;
                    case TagID.Float:
                        //using payload byte array
                        payload = tag.getPayload();
                        writePayloadBasicType(payload, ref chunkData, ref position, sizeof(float));
                        break;
                    case TagID.Double:
                        //using payload byte array
                        payload = tag.getPayload();
                        writePayloadBasicType(payload, ref chunkData, ref position, sizeof(double));
                        break;
                    case TagID.ByteArray:
                        //using payload byte array
                        payload = tag.getPayload();
                        int payloadLength = payload.Length;
                        byte[] payloadLengthBytes = BitConverter.GetBytes(payloadLength);
                        writePayloadBasicType(payloadLengthBytes, ref chunkData, ref position,sizeof(int));

                        writePayloadBasicType(payload, ref chunkData, ref position, (sizeof(byte) * payloadLength));
                        break;
                    case TagID.List:
                        //using payloadList
                        TagID tagListType = tag.getListType();
                        byte[] tagListTypeBytes = new byte[0];
                        tagListTypeBytes[0] = (byte)tagListType;
                        writePayloadBasicType(tagListTypeBytes, ref chunkData, ref position, sizeof(byte));

                        List<Tag> payloadList = tag.getPayloadList();
                        int listLength = payloadList.Count;
                        byte[] listLengthBytes = BitConverter.GetBytes(listLength);
                        writePayloadBasicType(listLengthBytes, ref chunkData, ref position, sizeof(int));

                        foreach (Tag e in payloadList)
	                    {
                            if(e.getID().Equals(tagListType)) 
                            {
                                TagTranslator.writeTag(e, ref chunkData, ref position);
                            }
                            else 
                            { 
                                throw new Exception("Wrong type of element in List");
                            }
		                    
	                    }
                        break;
                    case TagID.Compound:
                        //using payloadList

                        List<Tag> payloadList2 = tag.getPayloadList();
                        foreach (Tag e in payloadList2)
	                    {
                                TagTranslator.writeTag(e, ref chunkData, ref position);
	                    }
                        Tag endTag = new Tag(TagID.End, null, null);
                        TagTranslator.writeTag(endTag, ref chunkData, ref position);

                        break;
                    case TagID.String:
                        //using payload byte array
                        payload = tag.getPayload();
                        int tempPosition2 = 0;
                        byte[] arrayLength2 = readPayloadBasicType(payload, ref tempPosition2, sizeof(short), tagName);
                        short arraySizeNumber2 = BitConverter.ToInt16(arrayLength2, 0);

                        writePayloadBasicType(payload, ref chunkData, ref position, sizeof(short) + (sizeof(char) * arraySizeNumber2));
                        break;
                    case TagID.IntArray:
                        //using payload byte array
                        payload = tag.getPayload();
                        int tempPosition3 = 0;
                        byte[] arrayLength3 = readPayloadBasicType(payload, ref tempPosition3, sizeof(int), tagName);
                        int arraySizeNumber3 = BitConverter.ToInt32(arrayLength3, 0);

                        writePayloadBasicType(payload, ref chunkData, ref position, sizeof(int) + (sizeof(int) * arraySizeNumber3));
                        break;
                    default:
                        break;
               
#region oldcode
            //fileStream.WriteByte((byte)tagID);

            //if (tagID.Equals(TagID.End))
            //{
            //    return; //No payload
            //}

            //byte[] byteArray3 = BitConverter.GetBytes((short)tagName.Length);
            //byte[] buffer3 = Encoding.UTF8.GetBytes(tagName);



            //fileStream.WriteByte(byteArray3[0]);
            //fileStream.WriteByte(byteArray3[1]);
            //fileStream.Write(buffer3, 0, (short)tagName.Length);

            //byte[] dataInBytes;
            ////Payload
            //switch (tagID)
            //{
            //    case TagID.End:
            //        return; //No payload
            //        break;
            //    case TagID.Byte:
            //        unchecked
            //        {
            //            fileStream.Write((byte[])data, 0, 1);
            //        }
            //        return;
            //        break;
            //    case TagID.Short:
            //        //dataInBytes = BitConverter.GetBytes((short)data);
            //        fileStream.Write((byte[])data, 0, 2);
            //        return;

            //        break;
            //    case TagID.Int:

            //        //dataInBytes = BitConverter.GetBytes((int)data);
            //        fileStream.Write((byte[])data, 0, 4);
            //        return;

            //        break;
            //    case TagID.Long:
            //        //dataInBytes = BitConverter.GetBytes((long)data);
            //        fileStream.Write((byte[])data, 0, 8);
            //        return;

            //        break;
            //    case TagID.Float:
            //        //dataInBytes = BitConverter.GetBytes((float)data);
            //        fileStream.Write((byte[])data, 0, 4);
            //        return;

            //        break;
            //    case TagID.Double:
            //        //dataInBytes = BitConverter.GetBytes((double)data);
            //        fileStream.Write((byte[])data, 0, 8);
            //        return;

            //        break;
            //    case TagID.ByteArray:

            //        int sizeArray = ((byte[])data).Length; //changed for each tag
            //        byte[] sizeArrayArray = BitConverter.GetBytes((int)sizeArray);

            //        fileStream.Write(sizeArrayArray, 0, 4);
            //        fileStream.Write((byte[])data, 0, sizeArray);
            //        return;
            //        break;
            //    case TagID.String:
            //        short stringLength = (short)((byte[])data).Length;
            //        byte[] stringArrayLength = BitConverter.GetBytes(stringLength);


            //        fileStream.WriteByte(stringArrayLength[0]);
            //        fileStream.WriteByte(stringArrayLength[1]);
            //        fileStream.Write((byte[])data, 0, stringLength);


            //        return;

            //        break;
            //    case TagID.List:
            //        //fix this

            //        List<List<Tag>> theData = ((List<List<Tag>>)data);

            //        //fileStream.WriteByte((byte)tag.getListID());
            //        int elementsInList = theData.Count;
            //        byte[] elementArray = BitConverter.GetBytes(elementsInList);
            //        fileStream.Write(elementArray, 0, 4);

            //        if (elementsInList > 0)
            //        {
            //            if (tag.getListID().Equals(TagID.Compound))
            //            {
            //                foreach (List<Tag> list in theData)
            //                {
            //                    foreach (Tag innerTag in list)
            //                    {
            //                        TagTranslator.writeTag(innerTag, fileStream);
            //                    }
            //                }
            //            }
            //        }

            //        break;
            //    case TagID.Compound:
            //        return; //No payload
            //        break;
            //    case TagID.IntArray:
            //        //    fix this

            //        //    byte[] sizeArray3 = new byte[4]; //changed for each tag
            //        //    fileStream.Read(sizeArray3, 0, 4);
            //        //    int arraySizeNumber2 = BitConverter.ToInt32(sizeArray3, 0);

            //        //    payload = new byte[4 + arraySizeNumber2 * 4]; //changed for each tag
            //        //    payload[0] = sizeArray3[0];
            //        //    payload[1] = sizeArray3[1];
            //        //    payload[2] = sizeArray3[2];
            //        //    payload[3] = sizeArray3[3];
            //        //    fileStream.Read(payload, 4, arraySizeNumber2 * 4);
            //        //    returnTag = new Tag(tagID, tagIdentifier, payload, tagID);
            //        //    return returnTag;

            //        //    break;
            //        //default:
            //        //    returnTag = new Tag(tagID, tagIdentifier, null, tagID);
            //        return; //No payload
            //        break;
#endregion
                }
        }
        /*
        public static void overwriteRegionStream(Region region, int index)
        {

            if (region != null)
            {
                FileStream fileStream = region.fileStream;
                bool allChunksLoaded = !Array.Exists(region.chunksLoaded, delegate(bool x) { return !x; }); //checks if all chunks are loaded

                if (allChunksLoaded)
                {



                    fileStream.SetLength(0);
                    fileStream.Position = 0;
                    Tag chunkTag = new Tag(TagID.Compound, "region", null, TagID.Compound);
                    writeTag(chunkTag, fileStream);

                    int xindex = index % 3;
                    int yindex = index / 3;

                    for (int innerIndex = 0; innerIndex < 16; innerIndex++)
                    {
                        int xinnedIndex = innerIndex % 4;
                        int yinnedIndex = innerIndex / 4;

                        Chunk currentChunk = ChunkManager.chunkArray[(((yindex * 4) + yinnedIndex) * 12) + ((xindex * 4) + xinnedIndex)];
                        if (currentChunk != null)
                        {
                            saveChunk(currentChunk, fileStream);
                        }
                    }

                    Tag endTag = new Tag(TagID.End, null, null, TagID.End);
                    writeTag(endTag, fileStream);


                }

                fileStream.Close();
            }

        }
        public static Entity getUnloadedEntity(List<Tag> entity)
        {
            Entity newEntity = new Goblin(Vector2.Zero); // will never be used
            bool isReturnable = false;
            for (int i = 0; i < entity.Count; i++)
            {

                Tag e = entity[i];
                String tagName = e.getName();
                TagID tagID = e.getID();
                var data = e.getRawData();


                switch (tagID)
                {
                    case TagID.End:
                        //end of unloaded chunk
                        //this means its now added return new chunk
                        //Console.WriteLine("entity now loaded");
                        isReturnable = true;
                        break;
                    case TagID.Byte:

                        break;
                    case TagID.Short:
                        break;
                    case TagID.Int:
                        if (tagName.Equals("ID"))
                        {
                            byte[] byteData = (byte[])e.getRawData();
                            int dataInt = BitConverter.ToInt32(byteData, 0);
                            EntityID ID = (EntityID)dataInt;
                            switch (ID)
                            {
                                case EntityID.Player:
                                    break;
                                case EntityID.Goblin:
                                    newEntity = new Goblin(Vector2.Zero);
                                    break;
                                case EntityID.Zombie:
                                    newEntity = new Zombie(Vector2.Zero);
                                    break;
                                case EntityID.Deer:
                                    newEntity = new Deer(Vector2.Zero);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (tagName.Equals("EntityXPos"))
                        {
                            byte[] byteData = (byte[])e.getRawData();
                            int dataInt = BitConverter.ToInt32(byteData, 0);
                            newEntity.position.X = (float)dataInt;
                            newEntity.oldPosition.X = (float)dataInt;
                        }
                        else if (tagName.Equals("EntityYPos"))
                        {
                            byte[] byteData = (byte[])e.getRawData();
                            int dataInt = BitConverter.ToInt32(byteData, 0);
                            newEntity.position.Y = (float)dataInt;
                            newEntity.oldPosition.Y = (float)dataInt;
                            //byte[] data = (byte[])e.getRawData();
                            //float dataFloat = BitConverter.ToSingle(data, 0);
                            //newEntity.position.Y = dataFloat;
                        }
                        break;
                    case TagID.Long:
                        break;
                    case TagID.Float:

                        break;
                    case TagID.Double:
                        break;
                    case TagID.ByteArray:
                        break;
                    case TagID.String:
                        break;
                    case TagID.List:
                        break;
                    case TagID.Compound:
                        break;
                    case TagID.IntArray:
                        break;
                    default:
                        break;
                }
            }

            if (isReturnable)
            {
                return newEntity;

            }
            else
            {
                return null;
            }
        }

        public static Room getUnloadedRoom(List<Tag> room)
        {
            Room newRoom = new Room(0, 0, 0, 0, 0); // will never be used
            bool isReturnable = false;
            for (int i = 0; i < room.Count; i++)
            {

                Tag e = room[i];
                String tagName = e.getName();
                TagID tagID = e.getID();
                var data = e.getRawData();


                switch (tagID)
                {
                    case TagID.End:
                        //end of unloaded chunk
                        //this means its now added return new chunk
                        //Console.WriteLine("entity now loaded");
                        isReturnable = true;
                        break;
                    case TagID.Byte:

                        break;
                    case TagID.Short:
                        break;
                    case TagID.Int:
                        if (tagName.Equals("ID"))
                        {
                            byte[] byteData = (byte[])e.getRawData();
                            int dataInt = BitConverter.ToInt32(byteData, 0);
                            uint ID = (uint)dataInt;
                            newRoom.structureID = ID;
                        }
                        else if (tagName.Equals("StructureXPos"))
                        {
                            byte[] byteData = (byte[])e.getRawData();
                            int dataInt = BitConverter.ToInt32(byteData, 0);
                            newRoom.area.X = dataInt;
                        }
                        else if (tagName.Equals("StructureYPos"))
                        {
                            byte[] byteData = (byte[])e.getRawData();
                            int dataInt = BitConverter.ToInt32(byteData, 0);
                            newRoom.area.Y = dataInt;
                        }
                        else if (tagName.Equals("StructureWidth"))
                        {
                            byte[] byteData = (byte[])e.getRawData();
                            int dataInt = BitConverter.ToInt32(byteData, 0);
                            newRoom.area.Width = dataInt;
                        }
                        else if (tagName.Equals("StructureHeight"))
                        {
                            byte[] byteData = (byte[])e.getRawData();
                            int dataInt = BitConverter.ToInt32(byteData, 0);
                            newRoom.area.Height = dataInt;
                        }
                        break;
                    case TagID.Long:
                        break;
                    case TagID.Float:

                        break;
                    case TagID.Double:
                        break;
                    case TagID.ByteArray:
                        break;
                    case TagID.String:
                        break;
                    case TagID.List:
                        break;
                    case TagID.Compound:
                        break;
                    case TagID.IntArray:
                        break;
                    default:
                        break;
                }
            }

            if (isReturnable)
            {
                return newRoom;

            }
            else
            {
                return null;
            }
        }*/
    }
}
