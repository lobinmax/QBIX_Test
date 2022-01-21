using DevExpress.XtraEditors;

using System;
using System.Linq;
using System.Windows.Forms;

namespace QBIX_Test
{
    public partial class frmMain : XtraForm
    {
        private Departaments GetFocuseDepartament => gridViewDepartaments.GetEntityFromFocusRow<Departaments>();
        private Positions GetFocusePosition => gridViewPositions.GetEntityFromFocusRow<Positions>();
        private vEmployeesDs GetFocuseEmployee => gridViewEmployees.GetEntityFromFocusRow<vEmployeesDs>();
        private PositionSkills GetFocusePositionSkills => (PositionSkills) lbPositionSkills.SelectedItem;

        public frmMain()
        {
            InitializeComponent();

            gridViewDepartaments.SetActionSelectionChanged(gridViewDepartaments_SelectionChanged);
            gridViewPositions.SetActionSelectionChanged(gridViewPositions_SelectionChanged);
            gridViewEmployees.SetActionSelectionChanged(gridViewEmployees_SelectionChanged);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            DepartamentsUpdate();

        }
        private void DepartamentsUpdate(Guid? entityUid = null)
        {
            var focuseDepartament = this.GetFocuseDepartament;
            var dbContext = new QbixDataClassesDataContext();
            gridControlDepartaments.DataSource = dbContext.Departaments.OrderBy(departaments => departaments.Name);

            gridViewDepartaments.SetFocuseRow(gridColDepartamentUid, entityUid, focuseDepartament?.DepartamentUid);
        }

        private void btnAddDepartament_Click(object sender, EventArgs e)
        {
            var dlg = new dlgEditDepartament
            (
                DialogFormType.AddForm,
                DepartamentsUpdate, 
                null 
            );
            dlg.ShowDialog();
        }

        private void btnEditDepartament_Click(object sender, EventArgs e)
        {
            if (this.GetFocuseDepartament == null) { return; }
            var dlg = new dlgEditDepartament
            (
                DialogFormType.EditForm,
                DepartamentsUpdate,
                this.GetFocuseDepartament?.DepartamentUid
            );
            dlg.ShowDialog();
        }

        private void btnDelDepartament_Click(object sender, EventArgs e)
        {
            if (this.GetFocuseDepartament == null || 
                XtraMessageBox.Show($"Депортамент '{this.GetFocuseDepartament.Name}' и все связанные с ним данные будут удалены.{Environment.NewLine}" +
                                    $"Вы согласны?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                                    DialogResult.Yes) return;

            var dbContext = new QbixDataClassesDataContext();
            dbContext.DeportamentDelete(this.GetFocuseDepartament.DepartamentUid);
            
            DepartamentsUpdate();
        }

        private void btnDepartamentsUpdate_Click(object sender, EventArgs e)
        {
            DepartamentsUpdate();
        }

        private void gridViewDepartaments_SelectionChanged()
        {
            PositionUpdate();
            EmployeesUpdate();
        }

        private void PositionUpdate(Guid? entityUid = null)
        {
            var focuseDepartament = this.GetFocuseDepartament;
            var focusePosition = this.GetFocusePosition;
            if (focuseDepartament?.DepartamentUid == null)
            {
                gridControlPositions.DataSource = null;
                return;
            }

            var dbContext = new QbixDataClassesDataContext();
            gridControlPositions.DataSource = dbContext.Positions
                .Where(position => position.DepartamenUid == focuseDepartament.DepartamentUid)
                .OrderBy(ds => ds.Name);

            gridViewPositions.SetFocuseRow(gridColPositionUid, entityUid, focusePosition?.PositionUid);
        }

        private void gridViewPositions_SelectionChanged()
        {
            txtPositionDescription.EditValue = this.GetFocusePosition?.Description.CustomValue();
            PositionSkillsUpdate();
        }

        private void btnAddPosition_Click(object sender, EventArgs e)
        {
            var dlg = new dlgEditPosition
            (
                DialogFormType.AddForm,
                PositionUpdate,
                this.GetFocuseDepartament?.DepartamentUid,
                null
            );
            dlg.ShowDialog();
        }

        private void btnEditPosition_Click(object sender, EventArgs e)
        {
            if (this.GetFocusePosition == null) { return; }
            var dlg = new dlgEditPosition
            (
                DialogFormType.EditForm,
                PositionUpdate,
                this.GetFocuseDepartament?.DepartamentUid,
                this.GetFocusePosition?.PositionUid
            );
            dlg.ShowDialog();
        }

        private void btnDelPosition_Click(object sender, EventArgs e)
        {
            if (this.GetFocusePosition == null ||
                XtraMessageBox.Show($"Должность '{this.GetFocusePosition.Name}' и все связанные с ней данные будут удалены.{Environment.NewLine}" +
                                    $"Вы согласны?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) return;

            var dbContext = new QbixDataClassesDataContext();
            dbContext.PositionDelete(this.GetFocusePosition?.PositionUid);
            
            PositionUpdate();
        }

        private void PositionSkillsUpdate(Guid? entityUid = null)
        {
            var focusePosition = this.GetFocusePosition;
            if (focusePosition?.PositionUid == null)
            {
                lbPositionSkills.DataSource = null;
                return;
            }

            var dbContext = new QbixDataClassesDataContext();
            lbPositionSkills.DataSource = dbContext.PositionSkills
                .Where(skills => skills.PositionUid == focusePosition.PositionUid)
                .OrderBy(ds => ds.Name);
            lbPositionSkills.DisplayMember = "Name";
            lbPositionSkills.ValueMember = "SkillUid";
            lbPositionSkills.SelectedValue = entityUid;
        }

        private void btnAddPositionSkill_Click()
        {
            if (this.GetFocusePosition == null) { return; }
            var dlg = new dlgEditPositionSkill
            (
                DialogFormType.AddForm,
                PositionSkillsUpdate,
                this.GetFocusePosition.PositionUid,
                null
            );
            dlg.ShowDialog();
        }

        private void btnEditPositionSkill_Click()
        {
            if (this.GetFocusePositionSkills == null) { return; }
            var dlg = new dlgEditPositionSkill
            (
                DialogFormType.EditForm,
                PositionSkillsUpdate,
                this.GetFocusePosition?.PositionUid,
                this.GetFocusePositionSkills.SkillUid
            );
            dlg.ShowDialog();
        }

        private void btnDelPositionSkill_Click()
        {
            if (this.GetFocusePositionSkills == null ||
                XtraMessageBox.Show($"Навык '{this.GetFocusePositionSkills.Name}' и все связанные с ним данные будут удалены.{Environment.NewLine}" +
                                    $"Вы согласны?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) return;

            var dbContext = new QbixDataClassesDataContext();
            dbContext.PositionSkills.DeleteOnSubmit(
                dbContext.PositionSkills.Single(skills => skills.SkillUid == this.GetFocusePositionSkills.SkillUid));
            dbContext.SubmitChanges();

            PositionUpdate();
        }


        private void lbPositionSkills_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            var btn = e.Item;
            switch (btn.Name)
            {
                case "cbEditPositionSkill":
                    btnEditPositionSkill_Click();
                    break;

                case "cbDelPositionSkill":
                    btnDelPositionSkill_Click();
                    break;
            }
        }

        private void gcPositionSkills_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            var btn = e.Button;
            switch (btn.Properties.VisibleIndex)
            {
                case 0:
                    btnAddPositionSkill_Click();
                    break;
            }
        }

        private void EmployeesUpdate(Guid? entityUid = null)
        {
            var focuseDepartament = this.GetFocuseDepartament;
            var focuseEmployee = this.GetFocuseEmployee;
            if (focuseDepartament?.DepartamentUid == null)
            {
                gridControlEmployees.DataSource = null;
                return;
            }

            var dbContext = new QbixDataClassesDataContext();
            gridControlEmployees.DataSource = dbContext.vEmployeesDs
                .Where(employee => employee.DepartamentUid == focuseDepartament.DepartamentUid)
                .OrderBy(ds => ds.SNP);

            gridViewEmployees.SetFocuseRow(gridColEmployeeUid, entityUid, focuseEmployee?.EmployeeUid);
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            var dlg = new dlgEditEmloyee
            (
                DialogFormType.AddForm,
                EmployeesUpdate,
                this.GetFocuseDepartament?.DepartamentUid,
                null
            );
            dlg.ShowDialog();
        }

        private void btnEditEmployee_Click(object sender, EventArgs e)
        {
            if (this.GetFocuseEmployee == null) { return; }
            var dlg = new dlgEditEmloyee
            (
                DialogFormType.EditForm,
                EmployeesUpdate,
                this.GetFocuseDepartament?.DepartamentUid,
                this.GetFocuseEmployee.EmployeeUid
            );
            dlg.ShowDialog();
        }

        private void btnDelEmployee_Click(object sender, EventArgs e)
        {
            if (this.GetFocuseEmployee == null ||
                XtraMessageBox.Show($"Сотрудник '{this.GetFocusePosition.Name}' и все связанные с ним данные будут удалены.{Environment.NewLine}" +
                                    $"Вы согласны?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) return;

            var dbContext = new QbixDataClassesDataContext();
            dbContext.EmployeeDelete(this.GetFocuseEmployee?.EmployeeUid);

            EmployeesUpdate();
        }

        private void gridViewEmployees_SelectionChanged()
        {
            if (this.GetFocuseDepartament == null) { return; }
            ucEmployeeInfo.SetEmployeeInfo(this.GetFocuseDepartament.DepartamentUid, this.GetFocuseEmployee?.EmployeeUid);
        }

    }
}
