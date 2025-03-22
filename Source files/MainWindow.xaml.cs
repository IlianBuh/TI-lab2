using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TI2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    const int MAX_LEN = 320;
    string fileName;
    byte[] fileContent;
    byte[] encrFile;

    public MainWindow()
    {
        InitializeComponent();
    }


    private void BtnClear_Click(object sender, RoutedEventArgs e)
    {
        this.txtInput.Text = "";
        this.txtOutput.Text = "";
        this.txtKeyValue.Text = "";
        this.txtLSFRValue.Text = "";
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (this.encrFile == null) {
            MessageBox.Show("Nothing to save");
            return;
        }

        var openFileDialog = new SaveFileDialog();

        if (openFileDialog.ShowDialog() == false) return;

        File.WriteAllBytes(openFileDialog.FileName, this.encrFile);

        this.fileName = "";
        this.fileContent = null;
        this.encrFile = null;

        txtStatus.Text = "File saved successfully";

    }

    private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
    {
        if (this.fileContent == null) {
            MessageBox.Show("Nothing to process!");
            return;
        }

        string lfsrInitVal = LFSR.getBinValue(this.txtLSFRValue.Text, 4);

        LFSR register = new(lfsrInitVal, LFSR.InitLength);

        this.txtLSFRValue.Text = register.ToString();

        var res = Encoder.Encode(this.fileContent, register);

        if (res.binStr.Length > MAX_LEN) {
            this.txtOutput.Text = this.cutString(res.binStr, MAX_LEN);
            this.txtKeyValue.Text = this.cutString(res.keyStr, MAX_LEN);
        }
        else { 
            this.txtOutput.Text = res.binStr;
            this.txtKeyValue.Text = res.keyStr;
        }

        this.encrFile = res.bytesRes;
    }
    private string cutString(string src, int len) {
        return src.Substring(0, len / 2) + " ... " + src.Substring(src.Length - len / 2);
    }
    private void BtnReadFile_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();

        if (openFileDialog.ShowDialog() == true)
        {
            this.fileName = openFileDialog.FileName;
            this.fileContent = Converter.ReadBytesFromFile(this.fileName);
            if (fileContent.Length > MAX_LEN) {
                this.txtInput.Text = this.cutString(Converter.BinString(this.fileContent), MAX_LEN);
            } else {
                this.txtInput.Text = Converter.BinString(this.fileContent);
            }
            this.txtStatus.Text = "File loaded successfully";
        }

    }
    private void InputText_Click(object sender, RoutedEventArgs e)
    {
        this.txtInput.Focus();
    }
    private void InputLFSR_Click(object sender, RoutedEventArgs e)
    {
        this.txtLSFRValue.Focus();
    }

    private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
    {
        this.BtnEncrypt_Click(sender, e);
    }
}