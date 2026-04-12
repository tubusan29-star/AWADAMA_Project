Public Class FrmLogin
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' 1. Form2のインスタンスを作成
        Dim f2 As New FrmBattle()

        ' 2. 値を渡す
        f2.SetPlayerNo(Convert.ToInt32(txtLoginId.Text))

        ' 3. Form2を表示
        f2.Show()

        ' 4. Form1を消す（以下のどちらかを選択）
        Me.Close() ' 完全に閉じる（※注意点あり）
    End Sub
End Class