using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QBIX_Test
{
    public partial class dlgEditEmloyee : XtraForm
    {
        private readonly Action<Guid?> actionAfterClose;
        //private Employees Employees;
        private readonly QbixDataClassesDataContext dbContext = new QbixDataClassesDataContext();
        private readonly DialogFormType dialogFormType;
        private readonly Guid? departamentUid;
        private readonly Guid? emloyeeUid;

        public dlgEditEmloyee(
            DialogFormType dialogFormType, Action<Guid?> actionAfterClose, Guid? departamentUid, Guid? emloyeeUid)
        {
            InitializeComponent();

            this.actionAfterClose = actionAfterClose;
            this.dialogFormType = dialogFormType;
            this.departamentUid = departamentUid;
            this.emloyeeUid = emloyeeUid;
        }

        private void dlgEditEmloyee_Load(object sender, EventArgs e)
        {
            switch (dialogFormType)
            {
                case DialogFormType.AddForm:
                    //this.Employees = new Employees()
                    //{
                    //    EmployeeUid = Guid.NewGuid()
                    //};

                    break;

                case DialogFormType.EditForm:
                    if (!this.emloyeeUid.HasValue)
                    {
                        XtraMessageBox.Show("Ошибка программиста. Не передан ИД сотрудника", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    if (!dbContext.Employees
                        .Any(employees => employees.EmployeeUid == this.emloyeeUid))
                    {
                        XtraMessageBox.Show("Не найден сотрудник для редактирования", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    break;

                default:
                    XtraMessageBox.Show("Неизвестный тип диалога", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
            }
            ucEmployeeInfo.SetEmployeeInfo(this.departamentUid, this.emloyeeUid);
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void simpleButtonOk_Click(object sender, EventArgs e)
        {
            ucEmployeeInfo.SaveEmployeeInfo(out var employeeUid);

            this.Close();
            actionAfterClose(employeeUid);
        }
    }
}