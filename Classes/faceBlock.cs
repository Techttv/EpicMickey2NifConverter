using System;
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
        public List<int> face = new List<int>();
        public int faceAmount;

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
            
            string hexstring;
            for (int i = offset; i < offset+ (faceAmount * 2); i += 2)
            {
                hexstring = hex[i+1] + hex[i];
                int f = Convert.ToInt32(hexstring, 16);
                face.Add(f);
            }
            faceAmount = faceAmount / 3;
            
        }


        public void Append(int offset, string[] hex)
        {
            string temp = hex[offset - 12];
            if (hex[offset - 11] != "00")
            {
                string t = temp;
                temp = hex[offset - 11] + t;
            }
            int faceAmountAppend = Convert.ToInt32(temp, 16);

            string hexstring;
            for (int i = offset; i < offset + (faceAmount * 2); i += 2)
            {
                hexstring = hex[i + 1] + hex[i];
                int f = Convert.ToInt32(hexstring, 16);
                face.Add(f);
            }
            faceAmount += faceAmountAppend / 3;
        }
        public string writeToObj()
        {

            //TODO
            string s = "";

            return s;
        }
    }
}
