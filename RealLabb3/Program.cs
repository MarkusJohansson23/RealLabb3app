using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RealLabb3
{
    class Program
    {
        static byte[] pngSignature = { 137, 80, 78, 71, 13, 10, 26, 10 };
        static byte[] bmpSignature = { 66, 77 };
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the searchway for your file");
            string inputFile = Console.ReadLine();
            FileStream file;
            try
            {
                file = new FileStream(inputFile, FileMode.Open); //@"..\..\..\Pictures\Test_400x200.bmp"
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Error!: {0}", e);
                Console.ReadLine();
                return;
            }
            byte[] buffer = new byte[8];
            byte[] sizeBuffer = new byte[8];

            using (file)
            {
                file.Read(buffer, 0, buffer.Length);
                
                bool bufferIsPNG = buffer.SequenceEqual(pngSignature);
                bool bufferIsBMP = (buffer[0] == 66) && (buffer[1] == 77);

                if (bufferIsPNG)
                {
                    file.Position = 16;
                    file.Read(sizeBuffer, 0, sizeBuffer.Length);
                    var width = BitConverter.ToInt32(new byte[] { sizeBuffer[3], sizeBuffer[2], sizeBuffer[1], sizeBuffer[0] });
                    var height = BitConverter.ToInt32(new byte[] { sizeBuffer[7], sizeBuffer[6], sizeBuffer[5], sizeBuffer[4] });
                    Console.WriteLine($"This is a PNG file. Width = {width}, Height = {height}");

                }
                else if(bufferIsBMP)
                {
                    file.Position = 0x12;
                    file.Read(sizeBuffer, 0, sizeBuffer.Length);
                    var width = BitConverter.ToInt32(new byte[] { sizeBuffer[0], sizeBuffer[1], sizeBuffer[2], sizeBuffer[3] });
                    var height = BitConverter.ToInt32(new byte[] { sizeBuffer[4], sizeBuffer[5], sizeBuffer[6], sizeBuffer[7] });
                    Console.WriteLine($"This is a BMP file. Width = {width}, Height = {height}");
                }
                else
                {
                    Console.WriteLine("This is not a valid .bmp or .png file!");
                }

            }
            Console.ReadLine();

        }
        
    }
}
