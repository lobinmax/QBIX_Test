using DevExpress.XtraEditors;

using System;
using System.Linq;
using System.Windows.Forms;

namespace QBIX_Test
{
    public partial class dlgEditPositionSkill : XtraForm
    {
        private readonly Action<Guid?> actionAfterClose;
        private PositionSkills PositionSkills;
        private readonly QbixDataClassesDataContext dbContext = new QbixDataClassesDataContext();
        private readonly DialogFormType dialogFormType;
        private readonly Guid? positionUid;
        private readonly Guid? skillUid;

        public dlgEditPositionSkill(
            DialogFormType dialogFormType, Action<Guid?> actionAfterClose, Guid? positionUid, Guid? skillUid)
        {
            InitializeComponent(); 

            this.actionAfterClose = actionAfterClose;
            this.dialogFormType = dialogFormType;
            this.positionUid = positionUid;
            this.skillUid = skillUid;
        }

        private void dlgEditDepartament_Load(object sender, EventArgs e)
        {
            if (!this.positionUid.HasValue)
            {
                XtraMessageBox.Show("Ошибка программиста. Не передан ИД должности", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            switch (dialogFormType)
            {
                case DialogFormType.AddForm:
                    this.PositionSkills = new PositionSkills()
                    {
                        SkillUid = Guid.NewGuid()
                    };

                    break;

                case DialogFormType.EditForm:
                    if (!this.skillUid.HasValue)
                    {
                        XtraMessageBox.Show("Ошибка программиста. Не передан ИД навыка", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    this.PositionSkills = dbContext.PositionSkills
                        .SingleOrDefault(skills => skills.SkillUid == this.skillUid);
                    if (this.PositionSkills == null)
                    {
                        XtraMessageBox.Show("Не найден навык для редактирования", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    txtSkillName.EditValue = this.PositionSkills?.Name;
                    break;

                default:
                    XtraMessageBox.Show("Неизвестный тип диалога", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
            }
            txtSkillName.Select();
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void simpleButtonOk_Click(object sender, EventArgs e)
        {
            if (txtSkillName.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace())
            {
                XtraMessageBox.Show("Не заполнено поле с наименованием навыка", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.PositionSkills.PositionUid = this.positionUid.Value;
            this.PositionSkills.Name = txtSkillName.EditValue.CustomValue();
            dbContext.PositionSkills.SaveChange(this.PositionSkills);

            this.Close();
            actionAfterClose(this.PositionSkills.SkillUid); 
        }
    }
}