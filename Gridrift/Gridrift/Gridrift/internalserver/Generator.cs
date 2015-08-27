using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gridrift
{
    public class Generator
    {
        public Generator()
        {

        }

        public static Chunk generateChunk(Point chunkID)
        {
            if (chunkID.X == 0 && chunkID.Y == 0)
            {
                int test = 1;
            }
            Chunk chunk = new Chunk(chunkID.X, chunkID.Y);

            chunk.lastUpdate = 0;

            chunk.terrainPopulated = 1;

            chunk.structurePopulated = 1;

            chunk.inhabitedTime = 0;

            byte[] biomes = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                biomes[i] = (byte)i;
            }
            chunk.biomes = biomes;

            byte[] blocks = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                blocks[i] = (byte)i;
            }
            chunk.blocks = blocks;

            byte[] objects = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                objects[i] = (byte)i;
            }
            chunk.objects = objects;

            //Tag Entities = new Tag(TagID.List, "Entities", chunk.entities, TagID.Compound);
            //writeTag(Entities, fileStream);

            //Tag Rooms = new Tag(TagID.List, "Rooms", chunk.rooms, TagID.Compound);
            //writeTag(Rooms, fileStream);

            //Tag TileEntities = new Tag(TagID.List, "TileEntities", chunk.tileEntities, TagID.Compound);
            //writeTag(TileEntities, fileStream);

            return chunk;
        }
    }
}
