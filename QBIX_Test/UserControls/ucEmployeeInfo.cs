using DevExpress.XtraEditors;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace QBIX_Test
{
    public partial class ucEmployeeInfo : XtraUserControl
    {

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

        public ucEmployeeInfo()
        {
            InitializeComponent();
        }

        public void SetEmployeeInfo(Guid? departamentUid, Guid? employeeUid)
        {
            this.EmployeeUid = employeeUid ?? Guid.NewGuid();

            var dbContext = new QbixDataClassesDataContext();
            var employee = dbContext.Employees.SingleOrDefault(employees => employees.EmployeeUid == employeeUid);

            FillPositions(dbContext, employee?.DepartamentUid);
            DataToControl(employee);

            txtLastName.Select();
        }

        private void FillPositions(QbixDataClassesDataContext dataContext, Guid? departamentUid)
        {
            if (!departamentUid.HasValue)
            {
                luPosition.Properties.DataSource = null;
                return;
            }

            luPosition.Properties.DataSource = dataContext.Positions
                .Where(positions => positions.DepartamenUid == departamentUid).ToList();
            luPosition.Properties.ValueMember = "PositionUid";
            luPosition.Properties.KeyMember = "PositionUid";
            luPosition.Properties.DisplayMember = "Name";
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
                dtBirth.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace() ||
                luPosition.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace() ||
                dtHired.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace())
            {
                throw new Exception("Не все обязательные поля заполнены.");
            }

            employeeInfo.EmployeeUid = this.EmployeeUid.Value;
            employeeInfo.Surname = txtLastName.EditValue.CustomValue();
            employeeInfo.Name = txtName.EditValue.CustomValue();
            employeeInfo.Patronymic = txtPatronymic.EditValue.CustomValue();
            employeeInfo.Address = txtAddress.EditValue.CustomValue();
            employeeInfo.DtOfBirth = dtBirth.EditValue.CustomValueNn<DateTime>();
            employeeInfo.DtHired = dtHired.EditValue.CustomValueNn<DateTime>();
            employeeInfo.DtDismissed = dtDismissed.EditValue.CustomValue<DateTime>();
            employeeInfo.PhoneNumber = txtPhone.EditValue.CustomValue();
            employeeInfo.Email = txtEmail.EditValue.CustomValue();
            employeeInfo.Room = txtRoom.EditValue.CustomValue();
            employeeInfo.TabelianNumber = txtTabelianNumber.EditValue.CustomValue();
            employeeInfo.PositionUid = luPosition.EditValue.CustomValueNn<Guid>();

            foreach (var lbSkillsCheckedItem in lbSkills.CheckedItems)
            {
                employeeInfo.EmployeeSkills.Add((EmployeeSkills) lbSkillsCheckedItem);
            }

            return employeeInfo;
        }

        private void luPosition_EditValueChanged(object sender, EventArgs e)
        {
             luPosition.EditValue;
        }
    }
}
