using System;
using System.Collections;
using System.IO;

namespace Coal.CopyDir
{
    public class CopyDirectory {
        public void CopyDir(DirectoryInfo from, DirectoryInfo to) {
            try {
                ArrayList contains = new ArrayList();
                contains.AddRange(from.GetDirectories());
                contains.AddRange(from.GetFiles());
                foreach (var item in contains) {
                    if (item is DirectoryInfo) {
                        to.CreateSubdirectory((item as DirectoryInfo).Name);
                        CopyDir(item as DirectoryInfo, to.GetDirectories((item as DirectoryInfo).Name)[0]);
                    }
                    else if (item is FileInfo) {
                        (item as FileInfo).CopyTo(to.FullName + "\\" + (item as FileInfo).Name);
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
