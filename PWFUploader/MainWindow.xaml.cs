using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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
using System.Net.Http.Handlers;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace PWFUploader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



        //    // Set filter for file extension and default file extension 
        //    dlg.DefaultExt = ".png";
        //    dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


        //    // Display OpenFileDialog by calling ShowDialog method 
        //    Nullable<bool> result = dlg.ShowDialog();


        //    // Get the selected file name and display in a TextBox 
        //    if (result == true)
        //    {
        //        // Open document 
        //        string filename = dlg.FileName;
        //        textbox1.Text = filename;
        //    }
        //}

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{

        //    //using (var httpClient = new HttpClient())
        //    //{
        //    //    string fileName = textbox1.Text;
        //    //    using (var content = new MultipartFormDataContent())
        //    //    {

        //    //        FileStream fileStream = new FileStream(fileName, FileMode.Open);
        //    //        HttpContent fileStreamContent = new StreamContent(fileStream);


        //    //        //FileStream filestream = new FileStream(file, FileMode.Open);
        //    //        //string fileName = System.IO.Path.GetFileName(file);
        //    //        //content.Add(new StreamContent(filestream), "file", fileName);

        //    //        fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
        //    //        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        //    //        content.Add(fileStreamContent);

        //    //        using (var response = httpClient.PostAsync("https://localhost:7054/BufferedFileUpload", content).Result)
        //    //        {
        //    //            string apiResponse = response.Content.ReadAsStringAsync().Result;
        //    //            textbox1_Copy.Text = apiResponse;
        //    //        }

        //    //    }
        //    //}
        //    string[] fileName = { textbox1.Text };
        //    UploadFiles(fileName);
        //}
        private void DropBox_DragLeave(object sender, DragEventArgs e)
        {
            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }
        private void DropBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                var listbox = sender as ListBox;
                listbox.Background = new SolidColorBrush(Color.FromRgb(155, 155, 155));
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DropBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                _files.Clear();
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string filePath in files)
                {
                    _files.Add(new FileDesc() { path = filePath , name= System.IO.Path.GetFileName(filePath),status="to be uploaded" });
                }

                UploadFilesSingle(files);
            }

            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }
        public ObservableCollection<FileDesc> Files
        {
            get
            {
                return _files;
            }
        }
        private ObservableCollection<FileDesc> _files = new ObservableCollection<FileDesc>();

        private void UploadFiles(string[] files)
        {
            ProgressMessageHandler progress = new ProgressMessageHandler();

            HttpRequestMessage message = new HttpRequestMessage();
            MultipartFormDataContent content = new MultipartFormDataContent();

            try
            {
                foreach (var file in files)
                {
                    FileStream filestream = new FileStream(file, FileMode.Open);
                    string fileName = System.IO.Path.GetFileName(file);
                    content.Add(new StreamContent(filestream), "files", fileName);
                }

                message.Method = HttpMethod.Post;
                message.Content = content;
                message.RequestUri = new Uri("https://localhost:7054/BufferedFileUpload");

                var client = HttpClientFactory.Create(progress);
                client.SendAsync(message).ContinueWith(task =>
                {
                    if (task.Result.IsSuccessStatusCode)
                    {
                        var response = task.Result.Content.ReadAsStringAsync().Result;
                        dynamic json = JsonConvert.DeserializeObject<FileDesc>(response);
                  
                    }
                    else
                    {
                    }
                });
            }
            catch (Exception e)
            {
                //Handle exceptions - file not found, access denied, no internet connection etc etc
            }
        }
        private void UploadFilesSingle(string[] files)
        {
     

            try
            {
                foreach (var file in files)
                {
                    ProgressMessageHandler progress = new ProgressMessageHandler();

                    HttpRequestMessage message = new HttpRequestMessage();
                    MultipartFormDataContent content = new MultipartFormDataContent();
                    FileStream filestream = new FileStream(file, FileMode.Open);
                    string fileName = System.IO.Path.GetFileName(file);
                    content.Add(new StreamContent(filestream), "file", fileName);
                    message.Method = HttpMethod.Post;
                    message.Content = content;
                    message.RequestUri = new Uri("https://localhost:7054/BufferedFileUpload");

                    var client = HttpClientFactory.Create(progress);
                    client.SendAsync(message).ContinueWith(task =>
                    {
                        if (task.Result.IsSuccessStatusCode)
                        {
                            var response = task.Result.Content.ReadAsStringAsync().Result;
                            FileDesc fileDesc = JsonConvert.DeserializeObject<FileDesc>(response);
                            Files.Single(_=>_.name== fileDesc.name).status="Uploaded";
                        }
                        else
                        {
                        }
                    });
                }


            }
            catch (Exception e)
            {
                //Handle exceptions - file not found, access denied, no internet connection etc etc
            }
        }
        private void HttpSendProgress(object sender, HttpProgressEventArgs e)
        {
            HttpRequestMessage request = sender as HttpRequestMessage;
            ProgressBar.Dispatcher.BeginInvoke(
                  DispatcherPriority.Normal, new DispatcherOperationCallback(delegate
                  {
                      ProgressBar.Value = e.ProgressPercentage;
                      return null;
                  }), null);
        }

        private void ThreadSafeUpdateStatus(string status)
        {
            StatusIndicator.Dispatcher.BeginInvoke(
            DispatcherPriority.Normal, new DispatcherOperationCallback(delegate
            {
                StatusIndicator.Text = status;
                return null;
            }), null);
        }
        public class FileDesc
        {
            public string name { get; set; }
            public string path { get; set; }
            public long size { get; set; }
            public string status { get; set; }
        }

    }
}
