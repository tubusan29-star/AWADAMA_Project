Public Class Form1
    Dim handDataNameList As New ArrayList(Constant.HandCardNum)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        AllCardData.GetData()

        handDataNameList.AddRange(New String(Constant.HandCardNum - 1) {})
    End Sub

    Private Sub DeckShuffle_Button_Click(sender As Object, e As EventArgs) Handles btShuffleButton.Click

        Dim result As MsgBoxResult = MsgBox("デッキをシャッフルしますか？", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "確認")

        If result = MsgBoxResult.No Then
            Exit Sub
        End If

        Try
            Dim deck As IList(Of IList(Of Object))
            Dim archive As IList(Of IList(Of Object))

            deck = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_DECK, "A2:B" & Constant.MAX_CARD_CELL)

            archive = Common.ListShuffle(Of IList(Of Object))(deck)

            Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, "A2:B" & Constant.MAX_CARD_CELL, archive)

        Catch ex As Exception
            ErrorText.Text = ex.Message
        End Try

    End Sub

    Private Sub Deck_Click(sender As Object, e As EventArgs) Handles btDeck.Click
        Dim deck As IList(Of IList(Of Object))

        deck = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, "A2:C" & Constant.MAX_CARD_CELL)

        Dim index As Integer = 1
        For Each card As IList(Of Object) In deck
            If (card.Count = 2) Then
                handDataNameList.SetRange(index - 1, {card(1).ToString})
                HandCardUpdate()
                Exit For
            End If
            index += 1
        Next

        Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, "C" & (index + 1), "1")
    End Sub

    Private Sub HandCardUpdate()
        ' 1からnumControlsまでのインデックスでループ
        For i As Integer = 1 To Constant.HandCardNum - 1

            Dim foundControls() As Control = Me.Controls.Find("lbHandName" & i.ToString(), True)

            Dim targetControl As Control = foundControls(0)
            Dim handLabel As Label = targetControl

            Dim foundControlsPanel() As Control = Me.Controls.Find("pnHand" & i.ToString(), True)

            Dim targetControlPanel As Control = foundControlsPanel(0)
            Dim handPanel As Panel = targetControlPanel

            If (handDataNameList(i - 1) <> Nothing) Then
                handLabel.Visible = True
                handLabel.Text = handDataNameList(i - 1).ToString()
                handPanel.Visible = True
            Else
                handLabel.Visible = False
                handPanel.Visible = False
            End If
        Next i
    End Sub
End Class
