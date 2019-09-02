using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Net;
using System.IO;
using System.Web;
using System.Runtime.InteropServices;

namespace Calc
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            PDBHabilitados();
            textBox1.Select();
        }

        string Servidor00(string parametros)
        {
            WebRequest docRequest = WebRequest.Create("http://localhost/Calculadora2.php" + parametros);
            docRequest.Method = "GET";            
            docRequest.ContentType = "application/x-www-form-urlencoded";
            string data = parametros; /*data = conteúdo*/

            try
            {
                WebResponse docData = docRequest.GetResponse();
                StreamReader streamDados = new StreamReader(docData.GetResponseStream(), Encoding.ASCII);
                data = WebUtility.HtmlDecode(streamDados.ReadToEnd());
                /*Convertendo os dados contidos na source code html que estão 'encodados' para algo mais legível*/

                streamDados.Close();
                docData.Close();
            }
            catch 
            {
                return "Falha de sincronização.";
            }


            return data;
        }

        bool ErrorW = false;

        void PDBHabilitados()
        {
            button16.Enabled = false;
            button17.Enabled = false;
            button18.Enabled = true;
            button19.Enabled = false;
            button20.Enabled = false;
        }

        void AddToTextBox(string text, bool button)
        {
            if (text == ",")
                text = ".";

            string Param = "?TIPO=1";

            if (textBox1.Text.Contains("=") || (ErrorW && textBox1.Text.Length > 0 && !char.IsNumber(textBox1.Text[textBox1.TextLength - 1])))
                textBox1.Text   = string.Empty;

            ErrorW = false;
            if (button)
            {
                textBox1.Text += text;
                textBox1.Select(textBox1.Text.Length, textBox1.Text.Length);
            }

            if (text == "=")
            {
                int Cont = 0;
                string  []  numbers = new string[1024];
                string  []  operato  = new string[1024];

                try
                {
                    for (int i = 0; i < textBox1.Text.Length; i++)
                    {
                        if (char.IsNumber(textBox1.Text[i]) || textBox1.Text[i] == '.' || i == 0 && textBox1.Text[0] == '-')
                            numbers[Cont] += textBox1.Text[i];
                        else if (textBox1.Text[i] == ',')
                            numbers[Cont] += '.';
                        else
                        {
                            if (textBox1.Text[i] != '=')
                            {
                                operato[Cont] = textBox1.Text[i].ToString();

                                string Operador = operato[Cont];
                                string Numero = numbers[Cont];
                                if (operato[Cont] == "+")
                                    Operador = "%2B";/*Código em html do caractere '+'*/

                                int Posicao = numbers[Cont].IndexOf('.');
                                if (Posicao > 0)
                                {
                                    Numero.Remove(Posicao);
                                    Numero.Insert(Posicao, "&#44;"); /*HTML char '.'*/
                                }

                                Cont++;

                                Param += string.Format("&OP{0}={1}", Cont.ToString(), Operador);
                                Param += string.Format("&N{0}={1}", Cont.ToString(), Numero);
                            }
                        }
                    }

                    Param += string.Format("&N{0}={1}", (Cont + 1).ToString(), numbers[Cont]);

                    textBox1.Text = Servidor00(Param);
                }
                catch
                {
                    textBox1.Text = "Erro na formatação do texto.";
                }
                finally {

                    textBox1.Select(textBox1.Text.Length, textBox1.Text.Length);

                    if (!char.IsNumber(textBox1.Text[textBox1.TextLength - 1]))
                        ErrorW = true;
                    numbers = null;
                    operato = null;
                    Param = string.Empty;

                    PDBHabilitados();
                }
            }          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Verificacao();
        }

        void Verificacao()
        {
            bool habilita = false;
            if (textBox1.TextLength == 0)
                PDBHabilitados();
            else
            {
                if (char.IsNumber(textBox1.Text[textBox1.TextLength - 1]))
                    habilita = true;
                button17.Enabled = habilita;
                button18.Enabled = habilita;
                button19.Enabled = habilita;
                button20.Enabled = habilita;
            }

            habilita = false;

            if (textBox1.Text.Length > 0)
            {
                if (textBox1.Text.Contains("+") || textBox1.Text.Contains("-") || textBox1.Text.Contains("*") || textBox1.Text.Contains("/"))
                {
                    if (char.IsNumber(textBox1.Text[textBox1.TextLength - 1]))
                        habilita = true;
                }
            }

            button16.Enabled = habilita;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (char.IsNumber(e.KeyChar))
                AddToTextBox(e.KeyChar.ToString(), false);
            else if (e.KeyChar == 13 || e.KeyChar == '=')
            {
                if (button16.Enabled)
                    AddToTextBox('='.ToString(), false);
                
                e.Handled = true;
            }
            else if (e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '/' || e.KeyChar == '*')
            {
                switch (e.KeyChar)
                {
                    case '+':
                        e.Handled = !button17.Enabled; /*Se o botão estiver habilitado, a tecla será habilitada*/
                        break;
                    case '-':
                        e.Handled = !button18.Enabled;
                        break;
                    case '*':
                        e.Handled = !button19.Enabled;
                        break;
                    case '/':
                        e.Handled = !button20.Enabled;
                        break;
                }
                AddToTextBox(e.KeyChar.ToString(), false);/*'Enviar notificação'*/
            }
            else if (textBox1.Text.Length > 0 && e.KeyChar == 8)
                e.Handled = false;
            else if (e.KeyChar == ',' || e.KeyChar == '.')
                e.KeyChar = '.';

            else if (e.KeyChar == 27)
                textBox1.Text = string.Empty;
            else
                e.Handled = true;
        }

        private void BotaoGlobal(object sender, EventArgs e)
        {
            Button BotaoElemento = (Button)/*Cast simples*/sender;
            /*As informações dos elementos são passados no object(sender)*/

            if (BotaoElemento.Name == "button20")
                AddToTextBox("/", true);
            else if(BotaoElemento.Name == "button14" || BotaoElemento.Name == "button13")
                textBox1.Text = string.Empty;
            else if (BotaoElemento.Name == "button21" || BotaoElemento.Name == "button22")
            {
                if (BotaoElemento.Name == "button21")
                    textBox7.Text = RadPot(textBox2.Text, textBox3.Text,2);
                if (BotaoElemento.Name == "button22")
                    textBox6.Text = RadPot(textBox5.Text, textBox4.Text,3);
            }
            else if (BotaoElemento.Name == "button23")
            {
                if (button16.Enabled)
                    AddToTextBox("=", true);
            }

            else
                AddToTextBox(BotaoElemento.Text, true);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
                textBox1.Text = textBox1.Text.Substring(0, textBox1.TextLength - 1); /*Apagar último caractere*/
        }

        string RadPot(string Numero1, string Numero2, float tipo) /*Radiciação ou potenciação*/
        {
            string kks = "?TIPO="+tipo.ToString();
            kks += string.Format("&Number1={0}&Number2={1}", Numero1, Numero2);
            
            return Servidor00(kks);
        }
    }
}
