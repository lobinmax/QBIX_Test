using DevExpress.XtraEditors;

using System;
using System.Linq;
using System.Windows.Forms;

namespace QBIX_Test
{
    public partial class dlgEditDepartament : XtraForm
    {
        private readonly Action<Guid?> actionAfterClose;
        private Departaments Departament;
        private readonly QbixDataClassesDataContext dbContext = new QbixDataClassesDataContext();
        private readonly DialogFormType dialogFormType;
        private readonly Guid? deportamentUid;

        public dlgEditDepartament(DialogFormType dialogFormType, Action<Guid?> actionAfterClose, Guid? deportamentUid)
        {
            InitializeComponent(); 

            this.actionAfterClose = actionAfterClose;
            this.dialogFormType = dialogFormType;
            this.deportamentUid = deportamentUid;
        }

        private void dlgEditDepartament_Load(object sender, EventArgs e)
        {
            switch (dialogFormType)
            {
                case DialogFormType.AddForm:
                    this.Departament = new Departaments()
                    {
                        DepartamentUid = Guid.NewGuid()
                    };

                    break;

                case DialogFormType.EditForm:
                    if (!this.deportamentUid.HasValue)
                    {
                        XtraMessageBox.Show("Ошибка программиста. Не передан ИД департамента", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    this.Departament = dbContext.Departaments
                        .SingleOrDefault(departaments => departaments.DepartamentUid == this.deportamentUid);
                    if (Departament == null)
                    {
                        XtraMessageBox.Show("Не найден департамент для редактирования", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    txtDepartamentName.EditValue = this.Departament?.Name;
                    break;

                default:
                    XtraMessageBox.Show("Неизвестный тип диалога", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
            }
            txtDepartamentName.Select();
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void simpleButtonOk_Click(object sender, EventArgs e)
        {
            if (txtDepartamentName.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace())
            {
                XtraMessageBox.Show("Не заполнено поле с наименованием департамента", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            this.Departament.Name = txtDepartamentName.EditValue.CustomValue();
            dbContext.Departaments.SaveChange(this.Departament);

            this.Close();
            actionAfterClose(this.Departament.DepartamentUid); 
        }
    }
}