using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tx_mailmerger_conditional
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataSet ds;
        int iCurrentDataRow = 0;

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // load XML file as data source
            ds = new DataSet();
            ds.ReadXml("data.xml", XmlReadMode.Auto);

            mailMerge1.Merge(ds.Tables[0]);
        }

        private void mailMerge1_DataRowMerged(object sender, TXTextControl.DocumentServer.MailMerge.DataRowMergedEventArgs e)
        {
            iCurrentDataRow = e.DataRowNumber + 1;
        }

        private void mailMerge1_IncludeTextMerging(object sender, TXTextControl.DocumentServer.MailMerge.IncludeTextMergingEventArgs e)
        {
            byte[] data;

            // create blank document as byte array
            using(TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
            {
                tx.Create();
                tx.Save(out data, TXTextControl.BinaryStreamType.InternalUnicodeFormat);
            }

            // remove the include text field in case the data contains a "0"
            if (ds.Tables[0].Rows[iCurrentDataRow][e.Filename].ToString() == "0")
                e.IncludeTextDocument = data;
        }
    }
}