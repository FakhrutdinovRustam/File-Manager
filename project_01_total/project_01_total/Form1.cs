using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
//using System.IO.Compression;
using Ionic.Zip;
using System.Media;


namespace project_01_total
{
    public partial class Form1 : Form
    {
        private SystemIconsImageList sysIcons = new SystemIconsImageList();
        bool isFocus = true;
        bool isHidden = true;
        bool isNotePad = true;
        string isCopy = "";
        string pastPath = "";
        string contextPath = "";

        ListView currentListView = new ListView();
        TextBox currentTextBox = new TextBox();

        public Form1()
        {
            InitializeComponent();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void callOutNewFolderFunc()
        {

            try
            {
                string fileAddr = currentTextBox.Text + "New folder";
                if (Directory.Exists(fileAddr))
                {
                    int i = 1;
                    for (; Directory.Exists(fileAddr + " (" + i.ToString() + ")"); i++)
                    {
                    };
                    fileAddr += " (" + i.ToString() + ")";

                }
                Directory.CreateDirectory(fileAddr);
                // 

                //currentListView.LabelEdit = true;

                //string[] array = new string[2];
                //array[0] = Path.GetFileName(fileAddr);
                //array[1] = "";
                //ListViewItem a = new ListViewItem(array);
                //a.Selected = true;

                //currentListView.Items.Add(a);
                //currentListView.FocusedItem = a;

                //a.ImageIndex = sysIcons.GetIconIndex(fileAddr);
                //currentListView.SelectedItems[0].BeginEdit();
                refreshListview();
            }
            catch
            {
                MessageBox.Show("The process failed!!");
            }
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            callOutNewFolderFunc();


        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сделано Рустамом с любовью");
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            LoadDisk(drives, comboBox1);
            LoadDisk(drives, comboBox2);
            //if (isFocus)
            //    label1.Text = textBox1.Text;
            //else label1.Text = textBox2.Text;
            currentListView = listView1;
            currentTextBox = textBox1;
            LoadInfo(listView1, textBox1);
            LoadInfo(listView2, textBox2);
           
                SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\hello_mazafa.wav");
                player2.Play();

        }
        private void LoadDisk(DriveInfo[] drives, ComboBox comboBox)
        {
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                    comboBox.Items.Add(drive.Name);
            }
            comboBox.SelectedIndex = 0;
        }
        private void LoadInfo(ListView listView, TextBox textBox)
        {
            listView.Items.Clear();
            pastPath = textBox.Text;

            try
            {
                string[] ourDir = Directory.GetDirectories(textBox.Text);
                if (textBox.Text != Path.GetPathRoot(textBox.Text))
                    backFolder(listView);
                foreach (string folder in ourDir)
                {
                    if (isHidden)
                    {
                        if ((new FileInfo(folder).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                        {

                            string[] array = new string[5];
                            array[0] = Path.GetFileName(folder);
                            array[1] = "";
                            array[2] = "<DIR>";
                            array[3] = File.GetCreationTime(folder).ToString();
                            array[4] = "----";

                            ListViewItem item = new ListViewItem(array);
                            item.ImageIndex = 0;
                            listView.Items.Add(item);
                        }
                    }
                    else
                    {
                        string[] array = new string[5];
                        array[0] = Path.GetFileName(folder);
                        array[1] = "";
                        array[2] = "<DIR>";
                        array[3] = File.GetCreationTime(folder).ToString();
                        array[4] = "H---";
                        ListViewItem item = new ListViewItem(array);
                        item.ImageIndex = 0;
                        listView.Items.Add(item);
                    }
                }
                ourDir = Directory.GetFiles(textBox.Text);
                foreach (string files in ourDir)
                {
                    if (isHidden)
                    {
                        if ((new FileInfo(files).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                        {
                            string[] array = new string[4];
                            array[0] = Path.GetFileNameWithoutExtension(files);
                            array[1] = Path.GetExtension(files);
                            double size = new FileInfo(files).Length;
                            array[2] = size.ToString() + " KB";
                            array[3] = File.GetCreationTime(files).ToString();

                            ListViewItem item = new ListViewItem(array);
                            imageList1.Images.Add(files, Icon.ExtractAssociatedIcon(files));
                            item.ImageIndex = imageList1.Images.Keys.IndexOf(files);
                            listView.Items.Add(item);
                        }
                    }
                    else
                    {
                        string[] array = new string[4];
                        array[0] = Path.GetFileNameWithoutExtension(files);
                        array[1] = Path.GetExtension(files);
                        double size = new FileInfo(files).Length;
                        array[2] = size.ToString() + " KB"; ;
                        array[3] = File.GetCreationTime(files).ToString();

                        ListViewItem item = new ListViewItem(array);
                        imageList1.Images.Add(files, Icon.ExtractAssociatedIcon(files));
                        item.ImageIndex = imageList1.Images.Keys.IndexOf(files);
                        listView.Items.Add(item);
                    }
                }
            }
            catch
            {
                MessageBox.Show(" Not exist !!");
                //textBox.Text = label1.Text;
                LoadInfo(listView, textBox);

            }
            //if (isFocus)
            //    label1.Text = textBox1.Text;
            //else label1.Text = textBox2.Text;
        }

        private void getCurrenPath(ComboBox comboBox)
        {
            //currentPath = comboBox.SelectedItem.ToString();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.Text;
            LoadInfo(listView1, textBox1);
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //  LoadInfo(listView1, textBox1);
            isFocus = false;
        }
        private void backFolder(ListView listView)
        {
            ListViewItem item = new ListViewItem("..");
            item.ImageIndex = 1;
            listView.Items.Add(item);
        }
        private void label1_Click(object sender, EventArgs e)
        {
            //if (isFocus)
            //    label1.Text = textBox1.Text;
            //else label1.Text = textBox2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\xerox.wav");
            player2.Play();
            if (currentListView.SelectedItems.Count > 0)
            {
                int index = currentListView.SelectedItems.Count - 1;
                string sourcePath = currentTextBox.Text;
                while (index != -1)
                {
                    if (currentListView.SelectedItems[index].Text != "..")
                    {
                        string fileName = currentListView.SelectedItems[index].Text + currentListView.SelectedItems[index].SubItems[1].Text;
                        if (askBeforeCopy(fileName))
                        {
                            formCopyFunc(sourcePath + fileName, index);
                        }
                    }
                    else MessageBox.Show(".. is not a file!!");
                    index--;
                }
                refreshListview();
            }
            else MessageBox.Show("Please select your FILE/FOLDER!!");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }



        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
          
        }
        private void selectItem(ListView listView, TextBox textBox)
        {
            try
            {
                int pos = listView.SelectedIndices[0];
                if (pos == 0 && listView.SelectedItems[0].Text == "..")
                {
                    try
                    {
                        if (pastPath != null)
                        {
                            DirectoryInfo di = new DirectoryInfo(textBox.Text);
                            if (di.Parent.FullName != Path.GetPathRoot(textBox.Text))
                                textBox.Text = di.Parent.FullName + (char)92;
                            else textBox.Text = di.Parent.FullName;
                            LoadInfo(listView, textBox);
                        }
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        string addr = getPathFileExt(textBox, listView, 0);
                        if (File.Exists(addr))
                            selectFiles(listView, textBox);
                        else
                            textBox.Text += listView.Items[pos].Text + (char)92;
                        LoadInfo(listView, textBox);
                    }
                    catch
                    {
                        MessageBox.Show("Error!!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Select Wrong File!!");
            }

        }
        private string getPathFileExt(TextBox textBox, ListView listView, int index)
        {
            return textBox.Text + listView.SelectedItems[index].Text + listView.SelectedItems[index].SubItems[1].Text;
        }
        private string getPathFile(TextBox textBox, int index)
        {
            return textBox.Text + (char)92 + getFileName(currentListView, index);
        }
        private string getFileName(ListView listView, int index)
        {
            return listView.SelectedItems[index].Text + listView.SelectedItems[index].SubItems[1].Text;
        }
        private string getFileNameWthoutExt(ListView listView)
        {
            return listView.SelectedItems[0].Text;
        }

        private void selectFiles(ListView listView, TextBox textBox)
        {
            Process.Start(textBox.Text + listView.SelectedItems[0].Text + listView.SelectedItems[0].SubItems[1].Text);
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            selectItem(listView1, textBox1);


        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            isFocus = true;
            currentListView = listView1;
            currentTextBox = textBox1;
            //label1.Text = textBox1.Text;
            if (e.Button == MouseButtons.Right)
            {
                // MessageBox.Show($"{listView1.SelectedItems.Count.ToString()}");
                try
                {
                    if (File.Exists(getPathFileExt(textBox1, listView1, 0)))
                    {
                        setContextMenu("0");
                    }
                    else if (Directory.Exists(getPathFileExt(textBox1, listView1, 0)))
                    {
                        setContextMenu("1");
                    }

                }
                catch
                {
                    setContextMenu("2");
                }
            }
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            isFocus = false;
            currentListView = listView2;
            currentTextBox = textBox2;
            //label1.Text = textBox2.Text;
            if (e.Button == MouseButtons.Right)
            {
                // MessageBox.Show($"{listView1.SelectedItems.Count.ToString()}");
                try
                {
                    if (File.Exists(getPathFileExt(textBox2, listView2, 0)))
                    {
                        setContextMenu("0");
                    }
                    else if (Directory.Exists(getPathFileExt(textBox2, listView2, 0)))
                    {
                        setContextMenu("1");
                    }

                }
                catch
                {
                    setContextMenu("2");
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //  LoadInfo(listView2, textBox2);
            isFocus = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_TextChanged_1(object sender, EventArgs e)
        {
            textBox2.Text = comboBox2.Text;
            LoadInfo(listView2, textBox2);
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            selectItem(listView2, textBox2);

        }

        private void detailButton_Click(object sender, EventArgs e)
        {
            currentListView.View = View.Details;
        }


        private void iconButton_Click(object sender, EventArgs e)
        {

            currentListView.View = View.LargeIcon;

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }
		#region qwerty
		//private void showHiddenFileToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//    if (showHiddenFile.Checked == true)
		//        isHidden = false;
		//    else isHidden = true;
		//    LoadInfo(listView1, textBox1);
		//    LoadInfo(listView2, textBox2);
		//}
		#endregion
		private void showHiddenFile_CheckStateChanged(object sender, EventArgs e)
        {

        }
        private string readFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return "ERROR!!";
                }

                byte[] bytes = File.ReadAllBytes(path);
                for (int i = 0; i < bytes.Length; i++)
                {
                    if (bytes[i] == 0)
                        bytes[i] = 1;
                }

                return Encoding.ASCII.GetString(bytes, 0, bytes.Length);

            }
            catch
            {

                return "Error";
            }
        }
        private void callOutViewFunc()
        {
            if (currentListView.SelectedItems.Count > 0)
            {
                string path = getPathFileExt(currentTextBox, currentListView, 0);

                if (File.Exists(path))
                {
                    string fileContext = readFile(getPathFileExt(currentTextBox, currentListView, 0));
                    Form2 frm = new Form2(fileContext, getFileNameWthoutExt(currentListView));
                    frm.ShowDialog();
                }
                else
                {
                    string folderContext = path + "\n";
                    folderContext += "Creation time: " + Directory.GetCreationTime(path) + "\n";
                    folderContext += "Last Write Time: " + Directory.GetLastWriteTime(path) + "\n";
                    DirectoryInfo info = new DirectoryInfo(path);
                    FileInfo[] files = info.GetFiles("*.*", SearchOption.AllDirectories);
                    long totalSize = files.Sum(f => f.Length);
                    folderContext += "Total Size: " + BytesToString(totalSize) + "\n";
                    Form2 frm = new Form2(folderContext, path);
                    frm.ShowDialog();
                }

            }
            else MessageBox.Show("Please select your FILE/FOLDER!!!!");
        }

        private String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            callOutViewFunc();

        }

        private void listButton_Click(object sender, EventArgs e)
        {
            currentListView.View = View.List;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\Povezlo.wav");
            player2.Play();
            refreshListview();
        }

        private void refreshListview()
        {
            Refresh();
            LoadInfo(listView1, textBox1);
            LoadInfo(listView2, textBox2);
        }
        private bool askBeforeMove(string fileName)
        {
            DialogResult text;

            text = MessageBox.Show("Do you really want to move this " + $"{fileName} ?", "Move", MessageBoxButtons.YesNo);
            if (text == DialogResult.Yes)
                return true;
            return false;
        }

        private bool askForMoveSamePath()
        {
            DialogResult text;

            text = MessageBox.Show("You move in same location.\n Do you want to rename it ?", "Move", MessageBoxButtons.YesNo);
            if (text == DialogResult.Yes)
                return true;
            return false;
        }

        //private void 

        private void moveFunc(string sourcePath, string destinationPath, int pos)
        {
            if (textBox1.Text == textBox2.Text)
            {
                if (askForMoveSamePath())
                {
                    Form4 form4 = new Form4();
                    form4.ShowDialog();
                    string newName = form4.newName();
                    reName(currentTextBox.Text, newName);
                    refreshListview();
                }
            }
            else
            {
                if (!File.Exists(sourcePath))
                    System.IO.File.Move(sourcePath, destinationPath);
                else
                {
                    short index = formCopyFunc(sourcePath, pos);
                    if (index != 4)
                        deleteFunc(sourcePath);
                }

            }
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4.PerformClick();
        }
        private bool askBeforeCopy(string fileName)
        {

            DialogResult text;

            text = MessageBox.Show("Do you really want to copy" + $" {fileName} ?", "Copy", MessageBoxButtons.YesNo);
            if (text == DialogResult.Yes)
                return true;

            return false;
        }


        private short formCopyFunc(string sourcePath, int pos)
        {
            //MessageBox.Show(sourcePath);

            short index = 0;

            string destinationPath = "";
            string text = "";
            if (textBox1.Text == textBox2.Text)
                MessageBox.Show("It is same location!!");
            else
            {
                if (isFocus)
                {
                    text = textBox2.Text;
                    destinationPath = textBox2.Text + Path.GetFileName(sourcePath);
                    // MessageBox.Show(destinationPath);
                }
                else
                {
                    text = textBox1.Text;
                    destinationPath = textBox1.Text + Path.GetFileName(sourcePath);
                    //MessageBox.Show(destinationPath);
                }
                Form3 form3;

                if (File.Exists(sourcePath))
                {
                    if (File.Exists(destinationPath))
                    {
                        form3 = new Form3(destinationPath.Substring(text.Length));
                        form3.ShowDialog();
                        index = form3.applyOrder();
                        if (index == 0 || index == 1)
                            File.Copy(sourcePath, destinationPath, true);
                    }
                    else File.Copy(sourcePath, destinationPath);
                }
                else
                {

                    Directory.CreateDirectory(destinationPath);

                    foreach (string dir in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                    {

                        Directory.CreateDirectory(Path.Combine(destinationPath, dir.Substring(sourcePath.Length + 1)));

                    }

                    foreach (string file_name in System.IO.Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                    {
                        if (File.Exists(Path.Combine(destinationPath, file_name.Substring(sourcePath.Length + 1))))
                        {
                            if (index != 1)
                            {
                                form3 = new Form3(Path.Combine(destinationPath, file_name.Substring(sourcePath.Length + 1)));
                                form3.ShowDialog();
                                index = form3.applyOrder();
                            }
                            if (index == 0)
                            {
                                File.Copy(file_name, Path.Combine(destinationPath, file_name.Substring(sourcePath.Length + 1)), true);
                                continue;
                            }
                            else if (index == 1)
                            {

                                File.Copy(file_name, Path.Combine(destinationPath, file_name.Substring(sourcePath.Length + 1)), true);
                                continue;
                            }
                            else if (index == 3)
                            {
                                foreach (string file_name1 in System.IO.Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                                {
                                    if (!File.Exists(Path.Combine(destinationPath, file_name1.Substring(sourcePath.Length + 1))))
                                        File.Copy(file_name1, Path.Combine(destinationPath, file_name1.Substring(sourcePath.Length + 1)));
                                }
                                break;
                            }
                            else if (index == 2) continue;
                            else
                                break;
                        }
                        File.Copy(file_name, Path.Combine(destinationPath, file_name.Substring(sourcePath.Length + 1)));
                    }

                }

            }
            return index;

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3.PerformClick();
        }
        private bool IsFolderEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        private bool askBeforeDelete(string path, string fileName)
        {
            DialogResult text;
            if (File.Exists(path))
            {
                text = MessageBox.Show("Do you really want to delete this " + $"{fileName} ?", "Delete", MessageBoxButtons.YesNo);
                if (text == DialogResult.Yes)
                    return true;
            }
            else if (!IsFolderEmpty(path))
            {
                text = MessageBox.Show("This is have data inside.Do you really want to delete this " + $"{ fileName} ?", "Delete", MessageBoxButtons.YesNo);
                if (text == DialogResult.Yes)
                    return true;
            }
            else
            {
                text = MessageBox.Show("Do you really want to delete this " + $" {fileName} ?", "Delete", MessageBoxButtons.YesNo);
                if (text == DialogResult.Yes)
                    return true;
            }
            return false;
        }
        private void deleteFunc(string path)
        {

            if (File.Exists(path))
                File.Delete(path);
            else
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(path);
                refreshListview();
            }
        }

        private void callOutDeleteFunc()
        {
            if (currentListView.SelectedItems.Count > 0)
            {
                int index = currentListView.SelectedItems.Count - 1;

                string sourcePath = currentTextBox.Text;
                while (index != -1)
                {
                    if (currentListView.SelectedItems[index].Text != "..")
                    {
                        string fileName = currentListView.SelectedItems[index].Text + currentListView.SelectedItems[index].SubItems[1].Text;
                        string path = getPathFileExt(currentTextBox, currentListView, index);
                        if (askBeforeDelete(path, fileName))
                        {
                            deleteFunc(path);

                        }
                    }
                    else MessageBox.Show(".. is not a file!!");
                    index--;
                }
                refreshListview();
            }
            else
                MessageBox.Show("Please select your FILE/FOLDER!!!!");

        }


        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            callOutDeleteFunc();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\ill-be-back.wav");
            player2.Play();
            callOutDeleteFunc();
        }

        private long DirectoryLength(string path)
        {
            long filesLength = 0;
            try
            {
                DirectoryInfo fDir = new DirectoryInfo(path);
                foreach (FileInfo file in fDir.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    filesLength += file.Length;
                }
            }
            catch (Exception exc) { MessageBox.Show(exc.Message); }
            return filesLength;
        }
        private int DirectoriesCount(string path)
        {
            int dirCount = 0;
            try
            {
                DirectoryInfo fDir = new DirectoryInfo(path);
                foreach (DirectoryInfo dir in fDir.GetDirectories("*.*", SearchOption.AllDirectories))
                {
                    dirCount++;
                }
            }
            catch { }
            return dirCount;
        }
        private int FilesCount(string path)
        {
            int filesCount = 0;
            try
            {
                DirectoryInfo fDir = new DirectoryInfo(path);
                foreach (FileInfo file in fDir.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    filesCount++;
                }
            }
            catch { }
            return filesCount;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\00846.wav");
            player2.Play();
            string strFilePath = currentTextBox.Text
             + currentListView.SelectedItems[0].SubItems[0].Text
             + currentListView.SelectedItems[0].SubItems[1].Text;

            if (File.Exists(strFilePath))
            {
                FileInfo oFileInfo = new FileInfo(strFilePath);
                MessageBox.Show("Name: " + oFileInfo.Name + "\nLength: " + oFileInfo.Length.ToString() + " Bytes", "Properties");
            }
            else
            {
                DirectoryInfo oDirInfo = new DirectoryInfo(strFilePath);
                long filesLength = DirectoryLength(strFilePath);
                int dirCount = DirectoriesCount(strFilePath);
                MessageBox.Show("Name: " + oDirInfo.Name + "\nLength: " + filesLength.ToString() + " Bytes" + "\nDirectories: " + dirCount.ToString() + " | Files: " + FilesCount(strFilePath), "Properties");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\Luntik.wav");
            player2.Play();
            callOutNewFolderFunc();
        }

        private void optionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\00305.wav");
            player2.Play();
            if (currentListView.SelectedItems.Count > 0)
            {
                int index = currentListView.SelectedItems.Count - 1;
                string sourcePath = currentTextBox.Text;
                while (index != -1)
                {
                    if (currentListView.SelectedItems[index].Text != "..")
                    {
                        string fileName = currentListView.SelectedItems[index].Text + currentListView.SelectedItems[index].SubItems[1].Text;
                        if (askBeforeMove(fileName))
                        {
                            // string sourcePath = getPathFileExt(currentTextBox, currentListView);
                            string destinationPath = "";
                            if (isFocus)
                                destinationPath = getPathFile(textBox2, index);
                            else
                                destinationPath = getPathFile(textBox1, index);
                            moveFunc(sourcePath + fileName, destinationPath, index);
                        }
                    }
                    else MessageBox.Show(".. is not a file!!");
                    index--;
                }
                refreshListview();
            }
            else MessageBox.Show("Please select your FILE/FOLDER!!!!");

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoadInfo(listView1, textBox1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoadInfo(listView2, textBox2);
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {


            //*mousedown
            //* e.key = key.rirgt
            //* getitemat(e..at.x, y)
        }

        //private bool checkSameName(string path, string name)
        //{
        //    if(Directory.Exists(Path.Combine(path, name)))
        //    {

        //    }
        //}

        private void reName(string path, string newName)
        {
            if (currentListView.SelectedItems.Count == 1)
            {
                string oldName = currentListView.SelectedItems[0].Text.ToString();

                string newPath = Path.Combine(path, newName);
                string oldPath = getPathFileExt(currentTextBox, currentListView, 0);

                if (newName != oldName)
                {
                    if (File.Exists(oldPath))
                        File.Move(oldPath, newPath + currentListView.SelectedItems[0].SubItems[1].Text);
                    else if (Directory.Exists(oldPath))
                        Directory.Move(oldPath, newPath);
                }
                else
                {
                    MessageBox.Show("This is your old name");
                }

                //
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                if (e.Label == "" || e.Label == "..")
                {
                    e.CancelEdit = true;
                    MessageBox.Show("Invalid Name!!");
                }
                //else if()
                else
                {
                    reName(textBox1.Text, e.Label.ToString());
                    // refreshListview();
                }
                
                    SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\YGDR.wav");
                    player2.Play();
                
            }
            catch
            {
                e.CancelEdit = true;

            }

        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (isFocus)
                {
                    if (listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].Text.ToString() != "[..]")
                        listView1.SelectedItems[0].BeginEdit();
                }
                else
                {
                    if (listView2.SelectedItems.Count == 1 && listView2.SelectedItems[0].Text.ToString() != "[..]")
                        listView2.SelectedItems[0].BeginEdit();
                }
            }
            catch
            {
                MessageBox.Show("Cann't rename!!");
            }
        }

        private void listView2_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                if (e.Label == ".." || e.Label == "")
                {
                    e.CancelEdit = true;
                    MessageBox.Show("Invalid Name!!");
                }
                else
                {
                    reName(textBox2.Text, e.Label.ToString());
                    // refreshListview();
                }
            }
            catch
            {
                e.CancelEdit = true;
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            //label1.Text = textBox1.Text;

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void listView2_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void viewToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\00846.wav");
            player2.Play();
            button1.PerformClick();
        }

        private void callOutEditFunc()
        {
            if (currentListView.SelectedItems.Count > 0)
            {
                try
                {
                    string path = currentTextBox.Text + currentListView.SelectedItems[0].Text + currentListView.SelectedItems[0].SubItems[1].Text;
                    if (File.Exists(path))
                        Process.Start("notepad.exe", path);
                    //Process.Start(@"C:\Users\" + Environment.UserName + @"\AppData\Local\Programs\Microsoft VS Code\Code.exe", path);
                    else
                        MessageBox.Show("No files select!!");
                }
                catch
                {
                    MessageBox.Show("Unable Editing!!");
                }
            }
            else MessageBox.Show("Please select your FILE/FOLDER!!!!");

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            callOutEditFunc();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\Pencil.wav");
            player2.Play();
            callOutEditFunc();
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\Pencil.wav");
            player2.Play();
            button2.PerformClick();
        }

        private void deleteToolStripMenuItem1_Click_2(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\ill-be-back.wav");
            player2.Play();
            button6.PerformClick();
        }

        private void newFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\Luntik.wav");
            player2.Play();
            button5.PerformClick();
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\xerox.wav");
            player2.Play();
            isCopy = "0";
            contextPath = getPathFileExt(currentTextBox, currentListView, 0);
            //MessageBox.Show(contextPath);
        }



        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\Vstavka.wav");
            player2.Play();
            if (isCopy == "0")
            {
                formCopyFunc(contextPath, 0);
            }
            else if (isCopy == "1")
            {
                if (isFocus)
                {
                    moveFunc(contextPath, textBox2.Text + Path.GetFileName(contextPath), 0);
                }
                else
                {
                    moveFunc(contextPath, textBox1.Text + Path.GetFileName(contextPath), 0);
                }

            }
            isCopy = "";
            refreshListview();
        }


        private void listView1_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (currentListView.SelectedItems[0].Text == "..")
                e.CancelEdit = true;
        }

        private void renameToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\SayMyName.wav");
            player2.Play();
            try
            {
                if (isFocus)
                {
                    if (listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].Text.ToString() != "[..]")
                        listView1.SelectedItems[0].BeginEdit();
                    
                }
                else
                {
                    if (listView2.SelectedItems.Count == 1 && listView2.SelectedItems[0].Text.ToString() != "[..]")
                        listView2.SelectedItems[0].BeginEdit();
                }

            }
            catch
            {
                MessageBox.Show("Cann't rename!!");
            }
            //if (e.KeyCode == Keys.Enter)
            //{

            //	SoundPlayer player1 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\YGDR.wav");
            //	player1.Play();
            //}
        }


		#region hu
		//private void notepadToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//    isNotePad = true;
		//    notePadButton.Checked = true;
		//    visualCodeButton.Checked = false;

		//}

		//private void visualCodeToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//    isNotePad = false;
		//    notePadButton.Checked = false;
		//    visualCodeButton.Checked = true;
		//}
		#endregion
		private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void setContextMenu(string type)
        {
            if (type == "0") // voi file
            {
                viewToolStrip.Enabled = true;
                editToolStrip.Enabled = true;

                copyToolStrip.Enabled = true;
                pasteToolStrip.Enabled = false;
                renameToolStrip.Enabled = true;
                deleteToolStrip.Enabled = true;
                newFolderToolStrip.Enabled = false;
            }
            else if (type == "1")// voi folder
            {
                viewToolStrip.Enabled = true;
                editToolStrip.Enabled = false;

                copyToolStrip.Enabled = true;
                pasteToolStrip.Enabled = false;
                renameToolStrip.Enabled = true;
                deleteToolStrip.Enabled = true;
                newFolderToolStrip.Enabled = false;
            }
            else if (type == "2")// khoang trong
            {
                viewToolStrip.Enabled = false;
                editToolStrip.Enabled = false;

                copyToolStrip.Enabled = false;
                pasteToolStrip.Enabled = false;
                renameToolStrip.Enabled = false;
                deleteToolStrip.Enabled = false;
                newFolderToolStrip.Enabled = true;
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // MessageBox.Show($"{listView1.SelectedItems.Count.ToString()}");
                try
                {
                    if (File.Exists(getPathFileExt(textBox1, listView1, 0)))
                    {
                        setContextMenu("0");
                    }
                    else if (Directory.Exists(getPathFileExt(textBox1, listView1, 0)))
                    {
                        setContextMenu("1");
                    }

                }
                catch
                {
                    if (isCopy == "0" || isCopy == "1")
                    {
                        viewToolStrip.Enabled = false;
                        editToolStrip.Enabled = false;
                        copyToolStrip.Enabled = false;
                        pasteToolStrip.Enabled = true;
                        renameToolStrip.Enabled = false;
                        deleteToolStrip.Enabled = false;
                        newFolderToolStrip.Enabled = true;
                    }
                    else
                        setContextMenu("2");
                }
            }
        }

        private void listView2_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // MessageBox.Show($"{listView1.SelectedItems.Count.ToString()}");
                try
                {
                    if (File.Exists(getPathFileExt(textBox2, listView2, 0)))
                    {
                        setContextMenu("0");
                    }
                    else if (Directory.Exists(getPathFileExt(textBox2, listView2, 0)))
                    {
                        setContextMenu("1");
                    }

                }
                catch
                {

                    if (isCopy == "0" || isCopy == "1")
                    {
                        viewToolStrip.Enabled = false;
                        editToolStrip.Enabled = false;

                        copyToolStrip.Enabled = false;
                        pasteToolStrip.Enabled = true;
                        renameToolStrip.Enabled = false;
                        deleteToolStrip.Enabled = false;
                        newFolderToolStrip.Enabled = true;
                    }
                    else
                        setContextMenu("2");
                }
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private async Task DirectoryCopy(string src, string dstn)
        {
            DirectoryInfo srcDir = new DirectoryInfo(src);
            DirectoryInfo dstnDir = new DirectoryInfo(dstn);
            Directory.CreateDirectory(dstn);
            foreach (DirectoryInfo dir in srcDir.GetDirectories())
            {
                await DirectoryCopy(dir.FullName, Path.Combine(dstn, dir.Name));
                if (textBox1.Text == dir.FullName)
                {
                    LoadInfo(listView1, textBox1);
                }
                if (textBox2.Text == dir.FullName)
                {
                    LoadInfo(listView2, textBox2);
                }
            }
            try
            {
                foreach (FileInfo file in srcDir.GetFiles())
                {
                    await Task.Run(() => file.CopyTo(Path.Combine(dstnDir.FullName, file.Name), false));
                }
            }
			catch
			{
                MessageBox.Show("Уже существует");
			}
        }
        private async void listView1_DragDrop(object sender, DragEventArgs e)
        {
            Clipboard.SetData(DataFormats.FileDrop, e.Data.GetData(DataFormats.FileDrop));

            foreach (string file in Clipboard.GetFileDropList())
            {
                string fileName = Path.GetFileName(file);
                if (File.Exists(file))
                {
                    try
                    {
                        await Task.Run(() => File.Copy(file, textBox1.Text + fileName));
                    }
					catch { MessageBox.Show("нельзя");
                    };
                }
                else
                {
                    Directory.CreateDirectory(textBox1.Text + Path.GetFileName(file));
                    await DirectoryCopy(file, textBox1.Text + Path.GetFileName(file));
                }

            }
            refreshListview();

        }

        private async Task Compress()
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.Always;
                    zip.AlternateEncoding = Encoding.GetEncoding(866);
                    foreach (ListViewItem item in currentListView.SelectedItems)
                    {
                        string path = currentTextBox.Text + item.SubItems[0].Text + item.SubItems[1].Text;
                        if (File.Exists(path))
                        {
                            await Task.Run(() => zip.AddFile(path, ""/*Path.GetFileName(path)*/));

                            //zip.AddFile(path);
                        }
                        else
                        {

                            await Task.Run(() => zip.AddDirectory(path, Path.GetFileName(path)));
                            //zip.AddDirectory(path);
                        }
                    }
                    ArchiveForm archiveForm = new ArchiveForm(zip, currentTextBox, currentListView);
                    archiveForm.ShowDialog();
                    LoadInfo(currentListView, currentTextBox);

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private async void cumpressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\WOO.wav");
            player2.Play();
            await Compress();

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

		private void listView1_KeyDown(object sender, KeyEventArgs e)
		{
  //          if ((e.KeyCode == Keys.Enter))
  //          {
  //              SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\YGDR.wav");
  //              player2.Play();
  //          }
        }

		private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
           
                //SoundPlayer player2 = new SoundPlayer(@"C:\С#\project_01_total\project_01_total\Sounds\YGDR.wav");
                //player2.Play();
            
        }

	}
}
