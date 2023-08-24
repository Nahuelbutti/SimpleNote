using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using MaterialSkin;
using MaterialSkin.Controls;

namespace Bloc_de_Notas
{
    public partial class Form1 : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;
        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();
        private bool isTextChangedByUndoRedo = false;

        public Form1()
        {
            InitializeComponent();
            textBox1.TextChanged += textBox1_TextChanged;
            this.KeyDown += Form1_KeyDown;
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);


            // Elegir un esquema de color y tema
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.BlueGrey800, Primary.BlueGrey900,
                Primary.BlueGrey900, Accent.LightBlue200,
                TextShade.BLACK
            );
        }



        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Archivo de Texto (*.txt)|*.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, textBox1.Text);
                }
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = File.ReadAllText(openFileDialog.FileName);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isTextChangedByUndoRedo)
            {
                undoStack.Push(textBox1.Text);
                redoStack.Clear();
            }
            isTextChangedByUndoRedo = false; // Reset the flag
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 1)
            {
                redoStack.Push(undoStack.Pop());
                textBox1.Text = undoStack.Peek();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(redoStack.Pop());
                textBox1.Text = undoStack.Peek();
                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                deshacerToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                rehacerToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Puedes realizar acciones relacionadas con el menú aquí si es necesario
        }
    }
}
