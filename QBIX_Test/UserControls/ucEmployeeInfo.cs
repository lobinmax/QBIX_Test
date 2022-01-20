using DevExpress.XtraEditors;

using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors.Controls;

namespace QBIX_Test
{
    public partial class ucEmployeeInfo : XtraUserControl
    {
        //[Browsable(false)] private QbixDataClassesDataContext dbContext;

        [Browsable(false)] private bool mControlsReadOnly;

        [Browsable(true), Category("Qbix"), DisplayName("Qbix.ControlsReadOnly")]
        public bool ControlsReadOnly
        {
            get => mControlsReadOnly;
            set
            {
                txtLastName.ReadOnly = value;
                txtName.ReadOnly = value;
                txtPatronymic.ReadOnly = value;
                dtBirth.ReadOnly = value;
                txtAddress.ReadOnly = value;
                dtHired.ReadOnly = value;
                dtDismissed.ReadOnly = value;
                txtPhone.ReadOnly = value;
                txtEmail.ReadOnly = value;
                txtRoom.ReadOnly = value;
                txtTabelianNumber.ReadOnly = value;
                luPosition.Properties.ReadOnly = value;
                lbSkills.ReadOnly = value;

                mControlsReadOnly = value;
            }
        }

        private Guid? EmployeeUid;
        private Employees employee;

        public ucEmployeeInfo()
        {
            InitializeComponent();
        }

        public void SetEmployeeInfo(Guid? departamentUid, Guid? employeeUid)
        {
            this.EmployeeUid = employeeUid ?? Guid.NewGuid();

            var dbContext = new QbixDataClassesDataContext();
            var employee = dbContext.Employees.SingleOrDefault(employees => employees.EmployeeUid == employeeUid);

            FillPositions(employee?.DepartamentUid);
            DataToControl(employee);

            txtLastName.Select();
        }

        public void SetEmployeeInfo(Employees employee)
        {
            this.employee = employee;

            FillPositions(employee?.DepartamentUid);
            DataToControl(employee);

            txtLastName.Select();
        }

        private void FillPositions(Guid? departamentUid)
        {
            if (!departamentUid.HasValue)
            {
                luPosition.Properties.DataSource = null;
                return;
            }

            var dbContext = new QbixDataClassesDataContext();
            luPosition.Properties.DataSource = dbContext.Positions
                .Where(positions => positions.DepartamenUid == departamentUid).ToList();
            luPosition.Properties.ValueMember = "PositionUid";
            luPosition.Properties.KeyMember = "PositionUid";
            luPosition.Properties.DisplayMember = "Name";

            luPosition.Properties.PopulateColumns();
            foreach (LookUpColumnInfo propertiesColumn in luPosition.Properties.Columns)
            {
                propertiesColumn.Visible = (propertiesColumn.FieldName == "Name");
            }
        }

        private void DataToControl(Employees employees)
        {
            if (employees == null)
            {
                ClearControls();
                return;
            }

            txtLastName.EditValue = employees.Surname;
            txtName.EditValue = employees.Name;
            txtPatronymic.EditValue = employees.Patronymic;
            txtAddress.EditValue = employees.Address;
            dtBirth.EditValue = employees.DtOfBirth;
            dtHired.EditValue = employees.DtHired;
            dtDismissed.EditValue = employees.DtDismissed;
            txtPhone.EditValue = employees.PhoneNumber;
            txtEmail.EditValue = employees.Email;
            txtRoom.EditValue = employees.Room;
            txtTabelianNumber.EditValue = employees.TabelianNumber;
            luPosition.EditValue = employees.PositionUid;
        }

        private void ClearControls()
        {
            txtLastName.EditValue = null;
            txtName.EditValue = null;
            txtPatronymic.EditValue = null;
            txtAddress.EditValue = null;
            dtBirth.EditValue = null;
            dtHired.EditValue = null;
            dtDismissed.EditValue = null;
            txtPhone.EditValue = null;
            txtEmail.EditValue = null;
            txtRoom.EditValue = null;
            txtTabelianNumber.EditValue = null;
        }

        public Employees GetEmployeeInfo(bool validateData = true)
        {
            var employeeInfo = new Employees();

            if (validateData && 
                txtLastName.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace() ||
                txtName.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace() ||
                txtPatronymic.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace() ||
                !luPosition.EditValue.CustomValue<Guid>().HasValue)
            {
                throw new Exception("Не все обязательные поля заполнены.");
            }

            //employeeInfo.EmployeeUid = this.employee.EmployeeUid;
            this.employee.Surname = txtLastName.EditValue.CustomValue();
            this.employee.Name = txtName.EditValue.CustomValue();
            this.employee.Patronymic = txtPatronymic.EditValue.CustomValue();
            this.employee.Address = txtAddress.EditValue.CustomValue();
            this.employee.DtOfBirth = dtBirth.EditValue.CustomValueNn<DateTime>();
            this.employee.DtHired = dtHired.EditValue.CustomValueNn<DateTime>();
            this.employee.DtDismissed = dtDismissed.EditValue.CustomValue<DateTime>();
            this.employee.PhoneNumber = txtPhone.EditValue.CustomValue();
            this.employee.Email = txtEmail.EditValue.CustomValue();
            this.employee.Room = txtRoom.EditValue.CustomValue();
            this.employee.TabelianNumber = txtTabelianNumber.EditValue.CustomValue();
            this.employee.PositionUid = luPosition.EditValue.CustomValueNn<Guid>();

            foreach (var lbSkillsCheckedItem in lbSkills.CheckedItems)
            {
                employeeInfo.EmployeeSkills.Add((EmployeeSkills) lbSkillsCheckedItem);
            }

            return employeeInfo;
        }

        private void luPosition_EditValueChanged(object sender, EventArgs e)
        {
             //luPosition.EditValue;
        }
    }
}
