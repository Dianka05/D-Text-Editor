using System.Text.Json;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace D_Text_Editor
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            TextBoxField.TextChanged += TextBoxField_TextChanged;
        }

        private string? currentFilePath;
        private bool isTextModified = false;
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxField.Clear();
        }
        //public void OpenFile(string filePath)
        //{
        //    if (File.Exists(filePath))
        //    {
        //        string fileExtension = Path.GetExtension(filePath);
        //        if (fileExtension.Equals(".dtef", StringComparison.OrdinalIgnoreCase))
        //        {
        //            MessageBox.Show("File opened via association: " + filePath);
        //            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        //            {
        //                MyFileData? data = JsonSerializer.Deserialize<MyFileData>(fileStream);

        //                TextBoxField.Clear();

        //                foreach (TextSegment segment in data?.TextSegments ?? Enumerable.Empty<TextSegment>())
        //                {
        //                    // Убедимся, что есть содержимое в сегменте, иначе не загружаем
        //                    if (!string.IsNullOrEmpty(segment.Content))
        //                    {
        //                        byte[] contentBytes = Convert.FromBase64String(segment.Content);

        //                        using (MemoryStream memoryStream = new MemoryStream(contentBytes))
        //                        {
        //                            // Загружаем данные из потока в формате RTF
        //                            TextBoxField.LoadFile(memoryStream, RichTextBoxStreamType.RichText);
        //                        }
        //                    }
        //                }
        //            }
        //            currentFilePath = filePath; // устанавливаем текущий путь
        //            isTextModified = false;  // Сбрасываем флаг изменений при открытии файла
        //            UpdateFormTitle();
        //        }
        //        else if (fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
        //        {
        //            TextBoxField.Text = File.ReadAllText(filePath);
        //            currentFilePath = openFileDialog1.FileName; // устанавливаем текущий путь
        //            isTextModified = false;  // Сбрасываем флаг изменений при открытии файла
        //            UpdateFormTitle();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Unsupported file format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show($"File not found: {filePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                string fileExtension = Path.GetExtension(openFileDialog1.FileName);

                if (fileExtension.Equals(".dtef", StringComparison.OrdinalIgnoreCase))
                {
                    using (FileStream fileStream = new FileStream(openFileDialog1.FileName, FileMode.Open))
                    {
                        MyFileData? data = JsonSerializer.Deserialize<MyFileData>(fileStream);
                   
                        TextBoxField.Clear();
                        foreach (TextSegment segment in data?.TextSegments ?? Enumerable.Empty<TextSegment>())
                        {
                            if (!string.IsNullOrEmpty(segment.Content))
                            {
                                byte[] contentBytes = Convert.FromBase64String(segment.Content);

                                using (MemoryStream memoryStream = new MemoryStream(contentBytes))
                                {
                                    TextBoxField.LoadFile(memoryStream, RichTextBoxStreamType.RichText);
                                }
                            }
                        }

                    }
                    currentFilePath = openFileDialog1.FileName; 
                    isTextModified = false; 
                    UpdateFormTitle();
                }
                else if (fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    TextBoxField.Text = File.ReadAllText(openFileDialog1.FileName);
                    currentFilePath = openFileDialog1.FileName; ; 
                    isTextModified = false;
                    UpdateFormTitle();
                }
                else
                {
                    MessageBox.Show("Unsupported file format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
        //public void Save()
        //{
        //    saveFileDialog1.DefaultExt = ".dtef";
        //    saveFileDialog1.Filter = "Diana Text Editor File|*.dtef|Text File|*.txt";
        //    DialogResult dr = saveFileDialog1.ShowDialog();

        //    if (dr == DialogResult.OK)
        //    {
        //        string fileExtension = Path.GetExtension(saveFileDialog1.FileName);

        //        if (fileExtension.Equals(".dtef", StringComparison.OrdinalIgnoreCase))
        //        {
        //            MyFileData data = new MyFileData();
        //            data.TextSegments = new List<TextSegment>();

        //            using (MemoryStream memoryStream = new MemoryStream())
        //            {
        //                TextBoxField.SaveFile(memoryStream, RichTextBoxStreamType.RichText);

        //                TextSegment segment = new TextSegment
        //                {
        //                    Content = Convert.ToBase64String(memoryStream.ToArray())
        //                };

        //                data.TextSegments.Add(segment);
        //            }

        //            using (FileStream fileStream = new FileStream(saveFileDialog1.FileName, FileMode.Create))
        //            {
        //                JsonSerializer.Serialize(fileStream, data);
        //            }
        //        }
        //        else if (fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
        //        {
        //            File.WriteAllText(saveFileDialog1.FileName, TextBoxField.Text);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Unsupported file format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

        private void Save()
        {
            if (!string.IsNullOrEmpty(currentFilePath)) 
            {
                SaveToFile(currentFilePath);
                isTextModified = false;  
                UpdateFormTitle();
            }
            else
            {
                SaveAs();
            }
        }

        private void SaveAs()
        {
            saveFileDialog1.DefaultExt = ".dtef";
            saveFileDialog1.Filter = "Diana Text Editor File|*.dtef|Text File|*.txt";
            DialogResult dr = saveFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName;
                currentFilePath = filePath; 
                SaveToFile(filePath);

                isTextModified = false;
                UpdateFormTitle();
            }
        }
        private void SaveToFile(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);

            if (fileExtension.Equals(".dtef", StringComparison.OrdinalIgnoreCase))
            {
                MyFileData data = new MyFileData();
                data.TextSegments = new List<TextSegment>();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    TextBoxField.SaveFile(memoryStream, RichTextBoxStreamType.RichText);

                    TextSegment segment = new TextSegment
                    {
                        Content = Convert.ToBase64String(memoryStream.ToArray())
                    };

                    data.TextSegments.Add(segment);
                }

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    JsonSerializer.Serialize(fileStream, data);
                }
            }
            else if (fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
            {
                File.WriteAllText(filePath, TextBoxField.Text);
            }
            else
            {
                MessageBox.Show("Unsupported file format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                TextBoxField.SelectionFont = fontDialog1.Font;
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                TextBoxField.SelectionColor = colorDialog1.Color;
            }
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxField.SelectionAlignment = HorizontalAlignment.Right;
            TextBoxField.DeselectAll();
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxField.SelectionAlignment = HorizontalAlignment.Center;
            TextBoxField.DeselectAll();
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBoxField.SelectionAlignment = HorizontalAlignment.Left;
            TextBoxField.DeselectAll();
        }

        private void TextBoxField_TextChanged(object sender, EventArgs e)
        {
            isTextModified = true;
            UpdateFormTitle();
        }
        private void UpdateFormTitle()
        {
            // Обновляем заголовок формы, добавляя "*" к названию файла, если текст изменен
            string fileName = string.IsNullOrEmpty(currentFilePath) ? "Untitled" : Path.GetFileName(currentFilePath);
            this.Text = $"{fileName} {(isTextModified ? "*" : "")} - D Text Editor";
        }
    }




    [Serializable]
    public class MyFileData
    {
        public List<TextSegment>? TextSegments { get; set; }
    }

    [Serializable]
    public class TextSegment
    {
        public TextSegment()
        {
            Content = string.Empty;
            FontFamily = string.Empty;
            FontSize = 0;
        }

        public int StartIndex { get; set; }
        public int Length { get; set; }
        public string Content { get; set; }
        public string FontFamily { get; set; }
        public float FontSize { get; set; }
        public Color BackColor { get; set; } 
        public Color ForeColor { get; set; } 
        public Color TextColor { get; set; }
    }
}