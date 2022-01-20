using DevExpress.XtraEditors;

using System;
using System.Linq;
using System.Windows.Forms;

namespace QBIX_Test
{
    public partial class dlgEditPosition : XtraForm
    {
        private readonly Action<Guid?> actionAfterClose;
        private Positions Position;
        private readonly QbixDataClassesDataContext dbContext = new QbixDataClassesDataContext();
        private readonly DialogFormType dialogFormType;
        private readonly Guid? positionUid;
        private readonly Guid? departamenUid;

        public dlgEditPosition(DialogFormType dialogFormType, Action<Guid?> actionAfterClose, Guid? departamenUid,  Guid? positionUid)
        {
            InitializeComponent();

            this.actionAfterClose = actionAfterClose;
            this.dialogFormType = dialogFormType;
            this.positionUid = positionUid;
            this.departamenUid = departamenUid;
        }

        private void dlgEditPosition_Load(object sender, EventArgs e)
        {
            if (!this.departamenUid.HasValue)
            {
                XtraMessageBox.Show("Ошибка программиста. Не передан ИД департамента", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            switch (dialogFormType)
            {
                case DialogFormType.AddForm:
                    this.Position = new Positions()
                    {
                        PositionUid = Guid.NewGuid()
                    };

                    break;

                case DialogFormType.EditForm:
                    if (!this.positionUid.HasValue)
                    {
                        XtraMessageBox.Show("Ошибка программиста. Не передан ИД должности", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    this.Position = dbContext.Positions
                        .SingleOrDefault(positions => positions.PositionUid == this.positionUid);
                    if (Position == null)
                    {
                        XtraMessageBox.Show("Не найдена должность для редактирования", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    txtPositionName.EditValue = this.Position?.Name;
                    txtPositionDescription.EditValue = this.Position?.Description;
                    break;

                default:
                    XtraMessageBox.Show("Неизвестный тип диалога", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
            }
            txtPositionName.Select(); 
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void simpleButtonOk_Click(object sender, EventArgs e)
        {
            if (txtPositionName.EditValue.CustomValue().IsNullOrEmptyOrWhiteSpace())
            {
                XtraMessageBox.Show("Не заполнено поле с наименованием должности", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Position.DepartamenUid = this.departamenUid.Value;
            this.Position.Name = txtPositionName.EditValue.CustomValue();
            this.Position.Description = txtPositionDescription.EditValue.CustomValue();
            dbContext.Positions.SaveChange(this.Position);

            this.Close();
            actionAfterClose(this.Position.PositionUid);
        }
    }
}