<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        ErrorText = New TextBox()
        btShuffleButton = New Button()
        gbEnemyInfo = New GroupBox()
        gbAllyInfo = New GroupBox()
        pnBattleArea = New Panel()
        btDeck = New Button()
        pnHand1 = New Panel()
        lbHandName1 = New Label()
        pnHand2 = New Panel()
        lbHandName2 = New Label()
        pnHand3 = New Panel()
        lbHandName3 = New Label()
        pnHand4 = New Panel()
        lbHandName4 = New Label()
        pnHand5 = New Panel()
        lbHandName5 = New Label()
        pnHand6 = New Panel()
        lbHandName6 = New Label()
        pnHand1.SuspendLayout()
        pnHand2.SuspendLayout()
        pnHand3.SuspendLayout()
        pnHand4.SuspendLayout()
        pnHand5.SuspendLayout()
        pnHand6.SuspendLayout()
        SuspendLayout()
        ' 
        ' ErrorText
        ' 
        ErrorText.Location = New Point(12, 801)
        ErrorText.Multiline = True
        ErrorText.Name = "ErrorText"
        ErrorText.Size = New Size(1144, 67)
        ErrorText.TabIndex = 0
        ' 
        ' btShuffleButton
        ' 
        btShuffleButton.Location = New Point(912, 647)
        btShuffleButton.Name = "btShuffleButton"
        btShuffleButton.Size = New Size(116, 28)
        btShuffleButton.TabIndex = 1
        btShuffleButton.Text = "シャッフル"
        btShuffleButton.UseVisualStyleBackColor = True
        ' 
        ' gbEnemyInfo
        ' 
        gbEnemyInfo.Location = New Point(912, 12)
        gbEnemyInfo.Name = "gbEnemyInfo"
        gbEnemyInfo.Size = New Size(244, 245)
        gbEnemyInfo.TabIndex = 2
        gbEnemyInfo.TabStop = False
        gbEnemyInfo.Text = "Enemy"
        ' 
        ' gbAllyInfo
        ' 
        gbAllyInfo.Location = New Point(912, 263)
        gbAllyInfo.Name = "gbAllyInfo"
        gbAllyInfo.Size = New Size(244, 249)
        gbAllyInfo.TabIndex = 3
        gbAllyInfo.TabStop = False
        gbAllyInfo.Text = "Ally"
        ' 
        ' pnBattleArea
        ' 
        pnBattleArea.BorderStyle = BorderStyle.FixedSingle
        pnBattleArea.Location = New Point(142, 12)
        pnBattleArea.Name = "pnBattleArea"
        pnBattleArea.Size = New Size(737, 500)
        pnBattleArea.TabIndex = 4
        ' 
        ' btDeck
        ' 
        btDeck.Font = New Font("Yu Gothic UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
        btDeck.Location = New Point(910, 518)
        btDeck.Name = "btDeck"
        btDeck.Size = New Size(116, 123)
        btDeck.TabIndex = 5
        btDeck.Text = "30"
        btDeck.UseVisualStyleBackColor = True
        ' 
        ' pnHand1
        ' 
        pnHand1.BorderStyle = BorderStyle.FixedSingle
        pnHand1.Controls.Add(lbHandName1)
        pnHand1.Location = New Point(142, 518)
        pnHand1.Name = "pnHand1"
        pnHand1.Size = New Size(114, 123)
        pnHand1.TabIndex = 6
        pnHand1.Visible = False
        ' 
        ' lbHandName1
        ' 
        lbHandName1.AutoSize = True
        lbHandName1.Location = New Point(3, 0)
        lbHandName1.Name = "lbHandName1"
        lbHandName1.Size = New Size(0, 20)
        lbHandName1.TabIndex = 0
        ' 
        ' pnHand2
        ' 
        pnHand2.BorderStyle = BorderStyle.FixedSingle
        pnHand2.Controls.Add(lbHandName2)
        pnHand2.Location = New Point(262, 518)
        pnHand2.Name = "pnHand2"
        pnHand2.Size = New Size(114, 123)
        pnHand2.TabIndex = 7
        pnHand2.Visible = False
        ' 
        ' lbHandName2
        ' 
        lbHandName2.AutoSize = True
        lbHandName2.Location = New Point(3, 0)
        lbHandName2.Name = "lbHandName2"
        lbHandName2.Size = New Size(0, 20)
        lbHandName2.TabIndex = 1
        ' 
        ' pnHand3
        ' 
        pnHand3.BorderStyle = BorderStyle.FixedSingle
        pnHand3.Controls.Add(lbHandName3)
        pnHand3.Location = New Point(382, 518)
        pnHand3.Name = "pnHand3"
        pnHand3.Size = New Size(114, 123)
        pnHand3.TabIndex = 8
        pnHand3.Visible = False
        ' 
        ' lbHandName3
        ' 
        lbHandName3.AutoSize = True
        lbHandName3.Location = New Point(3, 0)
        lbHandName3.Name = "lbHandName3"
        lbHandName3.Size = New Size(0, 20)
        lbHandName3.TabIndex = 2
        ' 
        ' pnHand4
        ' 
        pnHand4.BorderStyle = BorderStyle.FixedSingle
        pnHand4.Controls.Add(lbHandName4)
        pnHand4.Location = New Point(502, 518)
        pnHand4.Name = "pnHand4"
        pnHand4.Size = New Size(114, 123)
        pnHand4.TabIndex = 9
        pnHand4.Visible = False
        ' 
        ' lbHandName4
        ' 
        lbHandName4.AutoSize = True
        lbHandName4.Location = New Point(-1, 0)
        lbHandName4.Name = "lbHandName4"
        lbHandName4.Size = New Size(0, 20)
        lbHandName4.TabIndex = 3
        ' 
        ' pnHand5
        ' 
        pnHand5.BorderStyle = BorderStyle.FixedSingle
        pnHand5.Controls.Add(lbHandName5)
        pnHand5.Location = New Point(622, 518)
        pnHand5.Name = "pnHand5"
        pnHand5.Size = New Size(114, 123)
        pnHand5.TabIndex = 10
        pnHand5.Visible = False
        ' 
        ' lbHandName5
        ' 
        lbHandName5.AutoSize = True
        lbHandName5.Location = New Point(3, 0)
        lbHandName5.Name = "lbHandName5"
        lbHandName5.Size = New Size(0, 20)
        lbHandName5.TabIndex = 4
        ' 
        ' pnHand6
        ' 
        pnHand6.BorderStyle = BorderStyle.FixedSingle
        pnHand6.Controls.Add(lbHandName6)
        pnHand6.Location = New Point(742, 518)
        pnHand6.Name = "pnHand6"
        pnHand6.Size = New Size(114, 123)
        pnHand6.TabIndex = 11
        pnHand6.Visible = False
        ' 
        ' lbHandName6
        ' 
        lbHandName6.AutoSize = True
        lbHandName6.Location = New Point(-1, 0)
        lbHandName6.Name = "lbHandName6"
        lbHandName6.Size = New Size(0, 20)
        lbHandName6.TabIndex = 5
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1168, 880)
        Controls.Add(btShuffleButton)
        Controls.Add(pnHand6)
        Controls.Add(pnHand5)
        Controls.Add(pnHand4)
        Controls.Add(pnHand3)
        Controls.Add(pnHand2)
        Controls.Add(pnHand1)
        Controls.Add(btDeck)
        Controls.Add(pnBattleArea)
        Controls.Add(gbAllyInfo)
        Controls.Add(gbEnemyInfo)
        Controls.Add(ErrorText)
        Name = "Form1"
        Text = "バトル"
        pnHand1.ResumeLayout(False)
        pnHand1.PerformLayout()
        pnHand2.ResumeLayout(False)
        pnHand2.PerformLayout()
        pnHand3.ResumeLayout(False)
        pnHand3.PerformLayout()
        pnHand4.ResumeLayout(False)
        pnHand4.PerformLayout()
        pnHand5.ResumeLayout(False)
        pnHand5.PerformLayout()
        pnHand6.ResumeLayout(False)
        pnHand6.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ErrorText As TextBox
    Friend WithEvents btShuffleButton As Button
    Friend WithEvents gbEnemyInfo As GroupBox
    Friend WithEvents gbAllyInfo As GroupBox
    Friend WithEvents pnBattleArea As Panel
    Friend WithEvents btDeck As Button
    Friend WithEvents pnHand1 As Panel
    Friend WithEvents pnHand2 As Panel
    Friend WithEvents pnHand3 As Panel
    Friend WithEvents pnHand4 As Panel
    Friend WithEvents pnHand5 As Panel
    Friend WithEvents pnHand6 As Panel
    Friend WithEvents lbHandName1 As Label
    Friend WithEvents lbHandName2 As Label
    Friend WithEvents lbHandName3 As Label
    Friend WithEvents lbHandName4 As Label
    Friend WithEvents lbHandName5 As Label
    Friend WithEvents lbHandName6 As Label

End Class
