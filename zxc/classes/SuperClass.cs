using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace zxc.classes
{
    enum States
    {
        READY = 1, INPROCESS, NOREADY
    }
    delegate void MyDelegate(object sender, EventArgs e);   
    interface Ixmlable
    {
         void XML();
    }
    class PaintXMLBase
    {
        public static event MyDelegate MyDelegate;
        protected void PrintItem(XmlElement item, int indent = 0)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();


            if (saveFileDialog1.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile(), System.Text.Encoding.Default))
                {
                    sw.Write($"{new string('\t', indent)}{item.LocalName} ");
                    foreach (XmlAttribute attr in item.Attributes)
                    {
                        sw.Write($"[{attr.InnerText}]");
                    }
                    foreach (var child in item.ChildNodes)
                    {
                        if (child is XmlElement node)
                        {
                            sw.WriteLine();
                            PrintItem(node, indent + 1);
                        }

                        if (child is XmlText text)
                        {
                            sw.Write($"- {text.InnerText}");
                        }
                    }
                }
            }
        }
    }
    internal class SuperClass : PaintXMLBase,Ixmlable
    {
        void Ixmlable.XML()
        {

            var doc = new XmlDocument();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            string filname = dlg.FileName;
            doc.Load(filname);
            var root = doc.DocumentElement;
            PrintItem(root);
        }
       
    }
}
