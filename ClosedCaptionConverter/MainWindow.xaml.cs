using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClosedCaptionConverter.Model;
using System.IO;

namespace ClosedCaptionConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowState = WindowState.Normal;
            this.ResizeMode = ResizeMode.NoResize;

            //string testFileSRT = @"D:\CodeLibrary\2019\ClosedCaptionConverter\ClosedCaptionConverter\ClosedCaptionConverterTest\TestData\WebVTT_sample1.vtt";
            //string testFileVTT = @"C:\Users\v-qixue\Downloads\40458\VTT1\40458_0102-Kasey-Whatisawebapp.vtt";
            //string testFileTTML = @"C:\Users\v-qixue\Downloads\40458\0102 - Kasey - What is a web app.ttml";

            //SRTHandler srtHandler = new SRTHandler();
            //var testCCs = srtHandler.ReadFile(testFileSRT);

            ////TTMLHandler ttmlHandler = new TTMLHandler();
            ////var testTTML = ttmlHandler.ReadFile(testFileTTML);
        }

        private void btnReadFile_Click(object sender, RoutedEventArgs e)
        {
            string ccFilePath = textBoxInputFile.Text.Trim();
            if (!File.Exists(ccFilePath))
            {
                string message = "File doesn't exist";
                DisplayStatusInfo(message);
                return;
            }

            string fileExtension = System.IO.Path.GetExtension(ccFilePath);
            List<ClosedCaptionConverter.Library.ClosedCaption> listOfClosedCaptions = null;
            if(fileExtension.ToLower()==".vtt")
            {
                VTTHandler vttHandler = new VTTHandler();
                listOfClosedCaptions= vttHandler.ReadFile(ccFilePath);
            }
            else if(fileExtension.ToLower()==".srt")
            {
                SRTHandler srtHandler = new SRTHandler();
                listOfClosedCaptions = srtHandler.ReadFile(ccFilePath);
            }
            else if(fileExtension.ToLower()==".ttml"||fileExtension.ToLower()==".xml")
            {
                TTMLHandler ttmlHandler = new TTMLHandler();
                listOfClosedCaptions = ttmlHandler.ReadFile(ccFilePath);
            }
            else
            {
                string message = "Your selected file type doesn't support.";
                DisplayStatusInfo(message);
            }

            if (listOfClosedCaptions != null)
            {
                listViewClosedCaptions.ItemsSource = listOfClosedCaptions;
                string message = $"Transcript loads successfully, total {listOfClosedCaptions.Count} lines.";

                DisplayStatusInfo(message);
            }
        }

        private void DisplayStatusInfo(string message)
        {
            string timestamp = DateTime.Now.ToLocalTime().ToString();
            textb_Status.Text = $"{timestamp}: {message}";
        }

        private void listViewClosedCaptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewClosedCaptions.SelectedIndex == -1)
                return;

            if (listViewClosedCaptions.SelectedItems.Count > 1)
            {
                DisplayStatusInfo($"total {listViewClosedCaptions.SelectedItems.Count} items be selected.");
                return;
            }

            if (e.AddedItems.Count > 0)
            {
                var selectedItem = e.AddedItems[0] as ClosedCaptionConverter.Library.ClosedCaption;
                

                DisplayStatusInfo($"{selectedItem.Transcript}.");
            }
        }

        private void btnConvert2VTT_lick(object sender, RoutedEventArgs e)
        {
            ConvertFiles("vtt");
        }

        private void btnConvert2SRT_Click(object sender, RoutedEventArgs e)
        {
            ConvertFiles("srt");
        }

        private void btnConvert2TTML_Click(object sender, RoutedEventArgs e)
        {
            ConvertFiles("ttml");
        }

        private void ConvertFiles(string targetFormat)
        {
            string folderPath = textBoxInputFolder.Text.Trim();
            if(!Directory.Exists(folderPath))
            {
                string message = "Folder doesn't exist.";
                DisplayStatusInfo(message);
                return;
            }

            string targetFolder = Directory.CreateDirectory(System.IO.Path.Combine(folderPath, targetFormat)).FullName;

            List<string> ccFiles = new List<string>();
            // get closed caption files in this folder
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                if(file.Extension.ToLower()==".vtt"|| file.Extension.ToLower() == ".srt"|| file.Extension.ToLower() == ".ttml"|| file.Extension.ToLower() == ".xml")
                {
                    ccFiles.Add(file.FullName);
                }
            }

            if(ccFiles.Count>0)
            {
                VTTHandler vttHandler = new VTTHandler();
                SRTHandler srtHandler = new SRTHandler();
                TTMLHandler ttmlHandler = new TTMLHandler();

                StringBuilder sbLog = new StringBuilder();
                foreach (string file in ccFiles)
                {
                    string extension = System.IO.Path.GetExtension(file).ToLower();
                    List<ClosedCaptionConverter.Library.ClosedCaption> Transcripts = null;
                    if(extension==".vtt"||extension==".srt")
                    {
                        Transcripts = srtHandler.ReadFile(file);
                    }
                    else if(extension==".ttml"||extension==".xml")
                    {
                        Transcripts = ttmlHandler.ReadFile(file);
                    }

                    if (Transcripts != null && Transcripts.Count > 0)
                    {
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                        if (targetFormat == "vtt")
                        {
                            vttHandler.WriteFile(System.IO.Path.Combine(targetFolder, fileName+".vtt"), Transcripts);
                        }
                        else if (targetFormat == "srt")
                        {
                            srtHandler.WriteFile(System.IO.Path.Combine(targetFolder, fileName+".srt"), Transcripts);
                        }
                        else if (targetFormat == "ttml")
                        {
                            ttmlHandler.WriteFile(System.IO.Path.Combine(targetFolder, fileName+".ttml"), Transcripts);
                        }
                    }
                    else
                    {
                        sbLog.AppendLine($"File failed convert - {file}.");
                    }
                }

                if (sbLog.ToString() != string.Empty)
                {
                    using (StreamWriter writer = new StreamWriter(System.IO.Path.Combine(targetFolder, "log.txt")))
                    {
                        writer.Write(sbLog.ToString());
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            DisplayStatusInfo($"Conversion Complete, total converted {ccFiles.Count} files.");
        }
    }
}
