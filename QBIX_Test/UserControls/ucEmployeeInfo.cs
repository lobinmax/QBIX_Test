using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace QBIX_Test
{
    public partial class ucEmployeeInfo : XtraUserControl
    {
        private QbixDataClassesDataContext dbContext;
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

        private Employees employee;

        public ucEmployeeInfo()
        {
            InitializeComponent();

            dtBirth.EditValue = DateTime.Today;
            dtHired.EditValue = DateTime.Today;
        }

        public void SetEmployeeInfo(Guid? departamentUid, Guid? employeeUid)
        {
            this.dbContext = new QbixDataClassesDataContext();
            FillPositions(departamentUid);
            if (employeeUid.HasValue)
            {
                this.employee = dbContext.Employees.SingleOrDefault(employees => employees.EmployeeUid == employeeUid);
                DataToControl(employee);
            }
            else
            {
                this.employee = new Employees() {EmployeeUid = Guid.NewGuid(), DepartamentUid = departamentUid.Value };
                ClearControls();
                luPosition.ItemIndex = 0;
            }
            luPosition_EditValueChanged(this.luPosition, EventArgs.Empty);
            txtLastName.Select();
        }

        private void FillPositions(Guid? departamentUid)
        {
            if (!departamentUid.HasValue)
            {
                luPosition.Properties.DataSource = null;
                return;
            }

            var depPositions = dbContext.Positions.Where(positions => positions.DepartamenUid == departamentUid).ToList();

            luPosition.Properties.DataSource = depPositions;
            luPosition.Properties.ValueMember = "PositionUid";
            luPosition.Properties.KeyMember = "PositionUid";
            luPosition.Properties.DisplayMember = "Name";

            luPosition.Properties.PopulateColumns();
            foreach (LookUpColumnInfo propertiesColumn in luPosition.Properties.Columns)
            {
                propertiesColumn.Visible = (propertiesColumn.FieldName == "Name");
            }

            if (!depPositions.Any())
            {
                luPosition.Properties.DataSource = null;
                luPosition.EditValue = null;
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
            dtBirth.EditValue = DateTime.Today;
            dtHired.EditValue = DateTime.Today;
            dtDismissed.EditValue = null;
            txtPhone.EditValue = null;
            txtEmail.EditValue = null;
            txtRoom.EditValue = null;
            txtTabelianNumber.EditValue = null; 
            luPosition.EditValue = null; 
        }

        public Guid? SaveEmployeeInfo(out Guid? employeeUid, bool validateData = true)
        {
            employeeUid = this.employee.EmployeeUid;

            if (validateData && 
                txtLastName.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace() ||
                txtName.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace() ||
                txtPatronymic.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace() ||
                !luPosition.EditValue.CustomValue<Guid>().HasValue)
            {
                XtraMessageBox.Show("Не все обязательные поля заполнены.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return employeeUid;
            }

            if (luPosition.EditValue.CustomValueNn<Guid>() != this.employee.PositionUid)
            {
                dbContext.EmployeeSkills.DeleteAllOnSubmit(employee.EmployeeSkills);
            }

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
            
            var dsSkills = (List<EmployeeSkillsCheckedResult>)lbSkills.DataSource;
            foreach (var lbSkillsItem in dsSkills)
            {
                var checkenSkill = dbContext.PositionSkills
                    .SingleOrDefault(skills => skills.SkillUid == lbSkillsItem.SkillUid.CustomValue<Guid>());

                if (lbSkillsItem.EmployeeSkillsBit.Value &&
                    this.employee.EmployeeSkills.All(skills => skills.SkillUid != checkenSkill?.SkillUid))
                {
                    this.employee.EmployeeSkills.Add
                    (
                        new EmployeeSkills()
                        {
                            EmployeeSkillUid = Guid.NewGuid(),
                            EmployeeUid = this.employee.EmployeeUid,
                            SkillUid = lbSkillsItem.SkillUid
                        }
                    );
                    continue;
                }

                if (!lbSkillsItem.EmployeeSkillsBit.Value)
                {
                    var delEmployeeSkill =
                        this.employee.EmployeeSkills.SingleOrDefault(
                            skills => skills.SkillUid == checkenSkill?.SkillUid);
                    if (delEmployeeSkill != null)
                    {
                        dbContext.EmployeeSkills.DeleteOnSubmit(delEmployeeSkill);
                    }
                }
            }

            dbContext.Employees.SaveChange(this.employee);
            return employeeUid;
        }

        private void luPosition_EditValueChanged(object sender, EventArgs e)
        {
            var selectedPositionUid = luPosition.EditValue.CustomValue<Guid>();
            if (!selectedPositionUid.HasValue)
            {
                lbSkills.DataSource = null;
                return;
            }

            lbSkills.DataSource =
                dbContext.EmployeeSkillsChecked(luPosition.EditValue.CustomValue<Guid>(), this.employee?.EmployeeUid).ToList();
        }
    }
}
