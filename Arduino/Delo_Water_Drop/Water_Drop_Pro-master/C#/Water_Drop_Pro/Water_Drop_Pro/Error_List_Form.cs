using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Water_Drop_Pro
{
    public partial class Error_List_Form : Form
    {
        #region VARAIBLES

        List<string> Error_Log = Main_Form.Error_Log;

        #endregion

        #region START/END FORM

        public Error_List_Form()
        {
            InitializeComponent();

            textBoxErrorList.Text = String.Join(Environment.NewLine, Error_Log);
        }

        #endregion

        #region BUTTONS

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
