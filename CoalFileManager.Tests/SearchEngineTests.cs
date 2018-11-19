using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coal.Search;
using System.IO;

namespace CoalFileManager.Tests {
    [TestClass]
    public class SearchEngineTests {
        [TestMethod]
        public void SearchByExpression_FindSomeTxtFile_VoidReturned() {
            //arrange
            string expression = @"\w*coalsearchexpr\w*.txt";
            string folderName = @"kljaADoiewj290_SALKJ1290FSKLFJEAdslkfjazeegwf";
            string fileName = DriveInfo.GetDrives()[0].RootDirectory.FullName + folderName + "\\slkrjhcoalsearchexpressiondskf.txt";
            string expected = fileName;
            //act
            Directory.CreateDirectory(DriveInfo.GetDrives()[0].RootDirectory.FullName + folderName);
            using(FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate)) {}
            SearchEngine.SearchByExpression(expression, new DirectoryInfo(DriveInfo.GetDrives()[0].RootDirectory.FullName + folderName));
            //assert
            Assert.AreEqual(expected, (SearchEngine.GetSearchResult()[0] as FileInfo).FullName);
            Directory.Delete(DriveInfo.GetDrives()[0].RootDirectory.FullName + folderName, true);
        }

        [TestMethod]
        public void SearchBySize_FindFileBySize_VoidReturned() {
            //arrange
            long expression = 9;
            string folderName = @"sklrjdghfalweafkaewfhqoiwaejlszfkdkjfakisudjf239pr8fw43ifjqp98231";
            string fileName = "file.txt";
            string expected = fileName;
            //act
            Directory.CreateDirectory(DriveInfo.GetDrives()[0].RootDirectory.FullName + folderName);
            using(StreamWriter sw = new StreamWriter(File.Create(DriveInfo.GetDrives()[0].RootDirectory.FullName + folderName + "\\" + fileName))) {
                sw.Write("Unit test");
            }
            SearchEngine.SearchBySize(expression, new DirectoryInfo(DriveInfo.GetDrives()[0].RootDirectory.FullName + folderName));
            //assert
            Assert.AreEqual(expected, (SearchEngine.GetSearchResult()[0] as FileInfo).Name);
            Directory.Delete(DriveInfo.GetDrives()[0].RootDirectory.FullName + folderName, true);
        }

        [TestMethod]
        public void SearchInsideFiles_ExprAndWhereArgs_VoidReturned() {
            //arrange
            string expression = @"\w*asdasdasdasd$";
            string folderName = "sdfijw9823qpiofj9psdijfq9ndkcjd39498qwajfoq938jfndifajgfq";
            string fileName = "efiu9qresiewqq34ijfier.txt";
            string expected = fileName;
            //act
            Directory.CreateDirectory(DriveInfo.GetDrives()[0].RootDirectory + folderName);
            using (StreamWriter sw = new StreamWriter(File.Create(DriveInfo.GetDrives()[0].RootDirectory + folderName + "\\" + fileName))) {
                sw.Write("airgsdknclaioj290ewpfaielkzsdjfi4aifj93ijflawjw93asdasdasdasd");
            }
            SearchEngine.SearchInsideFiles(expression, new DirectoryInfo(DriveInfo.GetDrives()[0].RootDirectory + folderName));
            //assert
            Assert.AreEqual(expected, (SearchEngine.GetSearchResult()[0] as FileInfo).Name);
            Directory.Delete(DriveInfo.GetDrives()[0].RootDirectory.FullName + folderName, true);
        }
        [TestCleanup]
        public void CleanUp() {
            SearchEngine.ClearSearchResult();
        }
    }
}
