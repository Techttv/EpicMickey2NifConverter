﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prova_3dviewport.Classes
{
    public class faceBlock
    {
        public int offset { get; set; }
        private List<int> face = new List<int>();
        private int faceAmount;

        public faceBlock(int offset, string[] hex)
        {
            this.offset = offset;
            findFace(hex);
        }

        public void findFace(string[] hex)
        {
            string temp = hex[offset - 12];
            if(hex[offset - 11] != "00"){
                string t = temp;
                temp= hex[offset - 11]+t;
            }
            faceAmount = Convert.ToInt32(temp, 16);
            Trace.WriteLine("Il numero scritto è"+ faceAmount);
            string hexstring;
            for (int i = offset; i < offset+ (faceAmount * 2); i += 2)
            {
                hexstring = hex[i] + hex[i + 1];
                int f = Convert.ToInt32(hexstring, 16);
                face.Add(f);
            }
            faceAmount = faceAmount / 3;
            Trace.WriteLine("Le facce trvate  sono"+face.Count());
        }
    }
}