using System;
using System.Collections.Generic;
using System.IO;

namespace MyFilesServer
{
    public class test
    {
        static char[] array = "01".ToCharArray();
            
        public static void Test() {
            Append(string.Empty, 16);            
        }

        private static void Append(string value, int length) {
            if (value.Length == length) {
                string folderPath = AppDomain.CurrentDomain.BaseDirectory + "hashes\\";

                string newPath = string.Empty;
                foreach (var c in value) {
                    newPath += c + "\\";
                }
                folderPath += newPath;
                Directory.CreateDirectory(folderPath);

            } else {
                for (int i = 0; i < array.Length; i++) {
                    Append(value + array[i], length);
                }
            }
        }
    }
}
